using System;
using MapViewer.Settings.Interfaces;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    public class MappingToolbarViewModel : ReactiveObject
    {
        private bool _jumpOnOpen;

        public MappingToolbarViewModel(IMapBehaviorSettings BehaviorSettings)
        {
            JumpOnOpen = BehaviorSettings.JumpOnOpen;
            this.WhenAnyValue(x => x.JumpOnOpen).Subscribe(v => BehaviorSettings.JumpOnOpen = v);
        }

        public bool JumpOnOpen
        {
            get { return _jumpOnOpen; }
            set { this.RaiseAndSetIfChanged(ref _jumpOnOpen, value); }
        }
    }
}
