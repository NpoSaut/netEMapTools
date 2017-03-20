using System;
using System.Collections.Generic;
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
        private readonly IMapLoadingService _mapLoadingService;
        private readonly IMappingService _mappingService;
        private readonly MapPresenter _mapPresenter;
        private readonly ITrackSource _trackSource;

        private MapToken _currentMapToken;

        public BlokMapService(MapPresenter MapPresenter, IMappingService MappingService, ITrackSource TrackSource,
                              IStartupArgumentsProvider StartupArgumentsProvider, IMapLoadingService MapLoadingService)
        {
            _mapPresenter = MapPresenter;
            _mappingService = MappingService;
            _trackSource = TrackSource;
            _mapLoadingService = MapLoadingService;

            var startupMap = StartupArgumentsProvider.FindFilePath("gps");
            if (startupMap != null)
                LoadMap(startupMap);
        }

        public GMap CurrentMap
        {
            get { return _currentMapToken != null ? _currentMapToken.Map : null; }
        }

        public event EventHandler<MapChangedEventArgs> CurrentMapChanged;

        public async Task LoadMap(string MapFileName)
        {
            var map = await _mapLoadingService.LoadBlokMap(MapFileName);
            SwitchMap(map, MapFileName);
        }

        private void SwitchMap(GMap Map, string MapFileName)
        {
            if (_currentMapToken != null)
                DetachMap(_currentMapToken);
            _currentMapToken = AttachMap(Map);
            OnCurrentMapChanged(MapFileName);
        }

        protected virtual void OnCurrentMapChanged(string MapFile)
        {
            var handler = CurrentMapChanged;
            if (handler != null) handler(this, new MapChangedEventArgs(MapFile));
        }

        private MapToken AttachMap(GMap Map)
        {
            var posts = _mapPresenter.PrintPosts(Map).ToList();
            _mappingService.Display(posts);

            var selectTrackSubscription =
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

            public GMap Map { get; }
            public IList<MapElement> Posts { get; }
            public IDisposable Subscriptions { get; }
        }
    }
}
