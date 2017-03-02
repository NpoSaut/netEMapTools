using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BlokMap.Search.Interfaces;
using MapViewer.Geocoding;
using MapViewer.Mapping;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class BlokMapSearchViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _canSearch;
        private readonly IGeocodingService _geocodingService;
        private readonly IMappingService _mappingService;
        private readonly ISearchProvider _searchProvider;

        private string _searchQuery;

        public BlokMapSearchViewModel(ISearchProvider SearchProvider, IMappingService MappingService, IGeocodingService GeocodingService)
        {
            _searchProvider = SearchProvider;
            _mappingService = MappingService;
            _geocodingService = GeocodingService;

            SearchResults = new ReactiveList<SearchResultViewModel>();

            this.WhenAny(x => x.SearchQuery, x => x != null)
                .CombineLatest(_searchProvider.CanSearch,
                               (a, b) => a && b)
                .ToProperty(this, x => x.CanSearch, out _canSearch);

            ReactiveCommand<IList<SearchResultViewModel>> search =
                ReactiveCommand.CreateAsyncTask(this.WhenAnyValue(x => x.CanSearch),
                                                GetSearchResults);

            search.Subscribe(results =>
                             {
                                 SearchResults.Clear();
                                 using (SearchResults.SuppressChangeNotifications())
                                 {
                                     foreach (SearchResultViewModel result in results)
                                         SearchResults.Add(result);
                                 }
                             });

            Search = search;

            this.WhenAnyValue(x => x.SearchQuery)
                .InvokeCommand(this, x => x.Search);
        }

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

        public ReactiveList<SearchResultViewModel> SearchResults { get; private set; }

        private async Task<IList<SearchResultViewModel>> GetSearchResults(object Parameter)
        {
            IList<SearchResult> results = await _searchProvider.Search(SearchQuery);

            return await Task.WhenAll(results.Select(async sr =>
                                                           {
                                                               var cityTask = _geocodingService.GetCity(sr.Point);
                                                               return new SearchResultViewModel(sr, _mappingService, cityTask);
                                                           }).ToArray());
        }
    }
}
