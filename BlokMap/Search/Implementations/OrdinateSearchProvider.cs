using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using BlokMap.Search.Interfaces;

namespace BlokMap.Search.Implementations
{
    public class OrdinateSearchProvider : ISearchProvider
    {
        private readonly IBlokMapService _blokMapService;

        public OrdinateSearchProvider(IBlokMapService BlokMapService)
        {
            _blokMapService = BlokMapService;
            CanSearch =
                Observable.FromEventPattern(h => BlokMapService.CurrentMapChanged += h,
                                            h => BlokMapService.CurrentMapChanged -= h)
                          .Select(_ => BlokMapService.CurrentMap != null);
        }

        public IObservable<bool> CanSearch { get; private set; }

        public Task<IList<SearchResult>> Search(string SearchQuery)
        {
            return Task.Factory.StartNew((Func<object, IList<SearchResult>>)SearchRoutine, SearchQuery);
        }

        private IList<SearchResult> SearchRoutine(object SearchQueryObject)
        {
            var searchQuery = (string)SearchQueryObject;
            double ordinate;
            if (!double.TryParse(searchQuery, out ordinate))
                return new List<SearchResult>();
            ordinate = Math.Floor(ordinate);

            if (_blokMapService.CurrentMap == null)
                return new List<SearchResult>();

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return _blokMapService.CurrentMap.Sections
                                  .SelectMany(section => section.Posts)
                                  .Where(post => ordinate == Math.Floor(post.Ordinate / 1000.0))
                                  .Select(post => new SearchResult(string.Format("Столб {0} км", post.Ordinate / 1000.0),
                                                                   post.Point))
                                  .ToList();
        }
    }
}
