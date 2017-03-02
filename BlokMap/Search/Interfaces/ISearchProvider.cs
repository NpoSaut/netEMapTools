using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlokMap.Search.Interfaces
{
    public interface ISearchProvider
    {
        IObservable<bool> CanSearch { get; }
        Task<IList<SearchResult>> Search(string SearchQuery);
    }
}
