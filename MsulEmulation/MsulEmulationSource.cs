﻿using System;
using System.Linq;
using System.Reactive.Linq;
using MsulEmulation.Encoding;
using MsulEmulation.ViewModels;

namespace MsulEmulation
{
    public class MsulEmulationSource
    {
        private readonly IMsulMessageEncoder _messageEncoder;
        public MsulEmulationSource(IMsulMessageEncoder MessageEncoder) { _messageEncoder = MessageEncoder; }

        public IObservable<MsulMessage> EmulationSource(MsulEmulationParametersViewModel EmulationParameters)
        {
            return
                EmulationParameters.Changed
                                   .Merge(EmulationParameters.Carriages.Select(cvm => cvm.Changed).Merge())
                                   .Select(_ => _messageEncoder.GetMessage(EmulationParameters))
                                   .StartWith(_messageEncoder.GetMessage(EmulationParameters));
        }
    }
}
