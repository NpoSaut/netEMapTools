using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BlokMap.MapElements;
using BlokMap.Search.Interfaces;
using Geographics;
using MapViewer.Geocoding;
using MapViewer.Mapping;
using Prism.Commands;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class BlokMapSearchViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _canSearch;

        private readonly Dictionary<SearchResult, SearchHighlightMapElement> _highlights =
            new Dictionary<SearchResult, SearchHighlightMapElement>();

        private readonly IMappingService _mappingService;
        private readonly ISearchProvider _searchProvider;

        private readonly ReactiveList<SearchResult> _searchResults;
        private string _searchQuery;

        public BlokMapSearchViewModel(ISearchProvider SearchProvider, IMappingService MappingService, IGeocodingService GeocodingService)
        {
            _searchProvider = SearchProvider;
            _mappingService = MappingService;

            this.WhenAny(x => x.SearchQuery, x => x != null)
                .CombineLatest(_searchProvider.CanSearch,
                               (a, b) => a && b)
                .ToProperty(this, x => x.CanSearch, out _canSearch);

            Clear = new DelegateCommand(() => SearchQuery = string.Empty);

            ReactiveCommand<IList<SearchResult>> search =
                ReactiveCommand.CreateAsyncTask(this.WhenAnyValue(x => x.CanSearch),
                                                GetSearchResults);

            _searchResults = new ReactiveList<SearchResult>();

            search.Subscribe(results =>
                             {
                                 _searchResults.Clear();
                                 foreach (SearchResult result in results)
                                     _searchResults.Add(result);
                             });

            Search = search;

            this.WhenAnyValue(x => x.SearchQuery)
                .InvokeCommand(this, x => x.Search);

            SearchResults = _searchResults.CreateDerivedCollection(
                sr => new SearchResultViewModel(sr, _mappingService, GeocodingService.GetPlacementName(sr.Point)),
                orderer: (a, b) => _mappingService.MapCenter.DistanceTo(a.Position)
                                                  .CompareTo(_mappingService.MapCenter.DistanceTo(b.Position)));

            _searchResults.ItemsAdded.Subscribe(Highlight);
            _searchResults.ItemsRemoved.Subscribe(Unhighlight);
            _searchResults.ShouldReset
                          .Subscribe(_ =>
                                     {
                                         foreach (var highlight in _highlights)
                                             _mappingService.Remove(highlight.Value);
                                         _highlights.Clear();
                                         foreach (SearchResult result in _searchResults)
                                             Highlight(result);
                                     });
        }

        public ICommand Clear { get; private set; }

        public bool CanSearch
        {
            get { return _canSearch.Value; }
        }

        public ICommand Search { get; private set; }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set { this.RaiseAndSetIfChanged(ref _searchQuery, value); }
        }

        public IReactiveDerivedList<SearchResultViewModel> SearchResults { get; private set; }

        private void Unhighlight(SearchResult result)
        {
            SearchHighlightMapElement element;
            if (_highlights.TryGetValue(result, out element))
            {
                _mappingService.Remove(element);
                _highlights.Remove(result);
            }
        }

        private void Highlight(SearchResult Result)
        {
            var element = new SearchHighlightMapElement(Result.Point);
            _mappingService.Display(element);
            _highlights.Add(Result, element);
        }

        private async Task<IList<SearchResult>> GetSearchResults(object Parameter) { return await _searchProvider.Search(SearchQuery); }
    }
}
