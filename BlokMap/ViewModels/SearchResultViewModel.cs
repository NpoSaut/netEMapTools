using System.Windows.Input;
using BlokMap.Search.Interfaces;
using MapViewer.Mapping;
using Prism.Commands;

namespace BlokMap.ViewModels
{
    public class SearchResultViewModel
    {
        public SearchResultViewModel(SearchResult SearchResult, IMappingService MappingService)
        {
            Title = SearchResult.Title;
            Navigate = new DelegateCommand(() => MappingService.Navigate(SearchResult.Point));
        }

        public string Title { get; private set; }
        public ICommand Navigate { get; private set; }

        public override string ToString() { return Title; }
    }
}
