using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MapViewer.Emulation.Msul.Encoding;
using MapViewer.Emulation.Msul.Entities;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public static class MsulEmulationParametersHelper
    {
        public static IDictionary<int, IObservable<MsulData>> ToMsulDataAllFlows(this MsulEmulationParametersViewModel ViewModel)
        {
            return ViewModel.Carriages
                            .Where(c => c.Kind == CarriageKind.TractionHead)
                            .ToDictionary(
                                 c => c.Number,
                                 c => ViewModel.ToMsulDataFlow(c.Number));
        }

        public static IObservable<MsulData> ToMsulDataFlow(
            this MsulEmulationParametersViewModel ViewModel, int ForSection)
        {
            var initialization =
                ViewModel.WhenAnyValue(x => x.ActiveSection)
                         .Select(a => a == ForSection
                                     ? InitializationKind.HeadSection
                                     : InitializationKind.TailSection);

            return ViewModel.Changed
                            .Merge(ViewModel.Carriages.Select(cvm => cvm.Changed).Merge())
                            .Select((_, i) => 0)
                            .StartWith(0)
                            .CombineLatest(initialization, (_, init) => init)
                            .Select(ViewModel.ToMsulData);
        }

        public static MsulData ToMsulData(
            this MsulEmulationParametersViewModel ViewModel, InitializationKind InitializationKind)
        {
            return new MsulData(
                InitializationKind,
                ViewModel.Time,
                ViewModel.TrainNumber,
                ViewModel.Speed,
                ViewModel.OutdoorTemperature,
                ViewModel.Position,
                ViewModel.Altitude,
                ViewModel.EmergencyStop,
                ViewModel.LeftDoorLocked,
                ViewModel.RightDoorLocked,
                ViewModel.LeftDoorOpened,
                ViewModel.RightDoorOpened,
                ViewModel.LightOn,
                ViewModel.Carriages
                         .Select(c => new MsulData.Carriage(
                                     c.Number,
                                     c.Kind,
                                     c.IndoorTemperature,
                                     c.EmergencyValueReleased,
                                     c.Toilet1Occupied,
                                     c.Toilet2Occupied))
                         .ToList());
        }
    }
}