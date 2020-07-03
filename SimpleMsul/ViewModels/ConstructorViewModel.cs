using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MapViewer.Emulation.Msul.Entities;
using MapViewer.Emulation.Msul.ViewModels;
using ReactiveUI;

namespace SimpleMsul.ViewModels
{
    public class ConstructorViewModel : ReactiveObject
    {
        private static readonly Dictionary<char, CarriageKind> _carriageKinds = new Dictionary<char, CarriageKind>
        {
            { 't', CarriageKind.TractionHead },
            { 'n', CarriageKind.Normal },
            { 'h', CarriageKind.HighVoltage }
        };

        private string _trainConfigurationText = "thnht thnht";
        private readonly ObservableAsPropertyHelper<List<List<CarriageParametersViewModel>>> _trainViewModel;

        public ConstructorViewModel()
        {
            this.WhenAnyValue(x => x.TrainConfigurationText)
                .Select(ParseConfiguration)
                .ToProperty(this, x => x.TrainViewModel, out _trainViewModel);

            Commit = ReactiveCommand.Create();
        }

        public ReactiveCommand<object> Commit { get; }

        public List<List<CarriageParametersViewModel>> TrainViewModel => _trainViewModel.Value;

        public string TrainConfigurationText
        {
            get => _trainConfigurationText;
            set => this.RaiseAndSetIfChanged(ref _trainConfigurationText, value);
        }

        private List<List<CarriageParametersViewModel>> ParseConfiguration(string Text)
        {
            var trains = Text.ToLower()
                             .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(train => ParseTrain(train).ToList())
                             .ToList();
            return trains;
        }

        private IEnumerable<CarriageParametersViewModel> ParseTrain(string Text)
        {
            for (var i = 0; i < Text.Length; i++)
            {
                var code = Text[i];

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
}