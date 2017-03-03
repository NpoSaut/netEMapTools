using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GMapElements;
using MapViewer.Environment;
using MapViewer.Mapping;
using MapVisualization.Elements;

namespace BlokMap
{
    public class BlokMapService : IBlokMapService
    {
        private readonly MapPresenter _mapPresenter;
        private readonly IMappingService _mappingService;
        private readonly ITrackSource _trackSource;

        private MapToken _currentMapToken;

        public BlokMapService(MapPresenter MapPresenter, IMappingService MappingService, ITrackSource TrackSource,
                              IStartupArgumentsProvider StartupArgumentsProvider)
        {
            _mapPresenter = MapPresenter;
            _mappingService = MappingService;
            _trackSource = TrackSource;

            string startupMap = StartupArgumentsProvider.FindFilePath("gps");
            if (startupMap != null)
            {
                Observable.FromAsync(() => Task.Factory.StartNew(() => GMap.Load(File.OpenRead(startupMap))))
                          .ObserveOnDispatcher()
                          .Subscribe(SwitchMap);
            }
        }

        public GMap CurrentMap
        {
            get { return _currentMapToken != null ? _currentMapToken.Map : null; }
        }

        public void SwitchMap(GMap Map)
        {
            if (_currentMapToken != null)
                DetachMap(_currentMapToken);
            _currentMapToken = AttachMap(Map);
            OnCurrentMapChanged();
        }

        public event EventHandler CurrentMapChanged;

        protected virtual void OnCurrentMapChanged()
        {
            EventHandler handler = CurrentMapChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private MapToken AttachMap(GMap Map)
        {
            List<MapElement> posts = _mapPresenter.PrintPosts(Map).ToList();
            _mappingService.Display(posts);

            IDisposable selectTrackSubscription =
                _trackSource.Track
                            .Select(t => _mapPresenter.PrintObjects(Map, t).ToList())
                            .Scan(new { prev = new List<MapElement>(), current = new List<MapElement>() },
                                  (a, n) => new { prev = a.current, current = n })
                            .Subscribe(x =>
                                       {
                                           _mappingService.Remove(x.prev);
                                           _mappingService.Display(x.current);
                                       });

            return new MapToken(Map, posts, selectTrackSubscription);
        }

        private void DetachMap(MapToken Token)
        {
            _mappingService.Remove(Token.Posts);
            Token.Subscriptions.Dispose();
        }

        private class MapToken
        {
            public MapToken(GMap Map, IList<MapElement> Posts, IDisposable Subscriptions)
            {
                this.Map = Map;
                this.Posts = Posts;
                this.Subscriptions = Subscriptions;
            }

            public GMap Map { get; private set; }
            public IList<MapElement> Posts { get; private set; }
            public IDisposable Subscriptions { get; private set; }
        }
    }
}
