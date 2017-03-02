using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BlokMap.Search.Interfaces;
using Geographics;
using MapViewer.Mapping;
using MapVisualization.Annotations;
using Prism.Commands;

namespace BlokMap.ViewModels
{
    public class SearchResultViewModel : INotifyPropertyChanged
    {
        public SearchResultViewModel(SearchResult SearchResult, IMappingService MappingService, Task<string> PlacementRequest)
        {
            PlacementRequest.ContinueWith(UpdateCity);
            Title = SearchResult.Title;
            Position = SearchResult.Point;
            Navigate = new DelegateCommand(() => MappingService.Navigate(Position));
        }

        public EarthPoint Position { get; private set; }
        public string Placement { get; private set; }
        public string Title { get; private set; }
        public ICommand Navigate { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateCity(Task<string> RequestTask)
        {
            Placement = RequestTask.Status == TaskStatus.RanToCompletion
                            ? RequestTask.Result
                            : "---";
            OnPropertyChanged("Placement");
        }

        public override string ToString() { return Title; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
