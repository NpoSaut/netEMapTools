using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MapViewer.Emulation.Msul.Entities;
using MapViewer.Emulation.Msul.ViewModels;
using ReactiveUI;

namespace SimpleMsul.ViewModels
{
    public class ConstructorViewModel : ReactiveObject, IMainWindowPage
    {
        private static readonly Dictionary<char, CarriageKind> _carriageKinds = new Dictionary<char, CarriageKind>
        {
            { 't', CarriageKind.TractionHead },
            { 'n', CarriageKind.Normal },
            { 'h', CarriageKind.HighVoltage }
        };

        private readonly ObservableAsPropertyHelper<List<IpAddressInputViewModel>> _addresses;

        private readonly CompositeDisposable _cleanUp = new CompositeDisposable();

        private readonly ObservableAsPropertyHelper<IList<CarriageParametersViewModel>> _trainViewModel;

        private string _trainConfigurationText = "thnht thnht";

        public ConstructorViewModel()
        {
            this.WhenAnyValue(x => x.TrainConfigurationText)
                .Select(ParseConfiguration)
                .ToProperty(this, x => x.TrainViewModel, out _trainViewModel)
                .DisposeWith(_cleanUp);

            this.WhenAnyValue(x => x.TrainViewModel)
                .Select(t => t.Where(c => c.Kind == CarriageKind.TractionHead)
                              .Select(c => new IpAddressInputViewModel(c.Number))
                              .ToList())
                .ToProperty(this, x => x.Addresses, out _addresses);

            Commit = ReactiveCommand.Create().DisposeWith(_cleanUp);
        }

        public List<IpAddressInputViewModel> Addresses => _addresses.Value;

        public ReactiveCommand<object> Commit { get; }

        public IList<CarriageParametersViewModel> TrainViewModel => _trainViewModel.Value;

        public string TrainConfigurationText
        {
            get => _trainConfigurationText;
            set => this.RaiseAndSetIfChanged(ref _trainConfigurationText, value);
        }

        public IDictionary<int, IPAddress> AddressesDictionary =>
            Addresses.ToDictionary(a => a.Number,
                                   a => IPAddress.Parse(a.Address));

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        private IList<CarriageParametersViewModel> ParseConfiguration(string Text)
        {
            var train = ParseTrain(Text.ToLower()).ToList();
            return train;
        }

        private IEnumerable<CarriageParametersViewModel> ParseTrain(string Text)
        {
            for (var i = 0; i < Text.Length; i++)
            {
                var code = Text[i];

                if (code == ' ')
                    continue;
                
                if (!_carriageKinds.TryGetValue(code, out var kind))
                    throw new ArgumentException($"Неизвестный код вагона: {code}", nameof(Text));

                var position =
                    i == 0
                        ? CarriagePosition.Left
                        : i == Text.Length - 1
                            ? CarriagePosition.Right
                            : CarriagePosition.Middle;

                yield return new CarriageParametersViewModel(i, kind, position);
            }
        }
    }

    public class IpAddressInputViewModel : ReactiveObject
    {
        private string _address;

        public IpAddressInputViewModel(int Number)
        {
            this.Number = Number;
            _address    = $"192.168.0.{Number}";
        }

        public int Number { get; }

        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }
    }
}