using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using BlokMap.Search.Interfaces;
using MapViewer.Mapping;
using ReactiveUI;

namespace BlokMap.ViewModels
{
    public class BlokMapSearchViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _canSearch;
        private readonly ISearchProvider _searchProvider;

        private string _searchQuery;

        public BlokMapSearchViewModel(ISearchProvider SearchProvider, IMappingService MappingService)
        {
            _searchProvider = SearchProvider;

            SearchResults = new ReactiveList<SearchResultViewModel>();

            this.WhenAny(x => x.SearchQuery, x => x != null)
                .CombineLatest(_searchProvider.CanSearch,
                               (a, b) => a && b)
                .ToProperty(this, x => x.CanSearch, out _canSearch);

            ReactiveCommand<List<SearchResultViewModel>> search =
                ReactiveCommand.CreateAsyncTask(this.WhenAnyValue(x => x.CanSearch),
                                                async _ =>
                                                      {
                                                          IList<SearchResult> results = await _searchProvider.Search(SearchQuery);
                                                          return results.Select(sr => new SearchResultViewModel(sr, MappingService)).ToList();
                                                      });

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
    }
}
