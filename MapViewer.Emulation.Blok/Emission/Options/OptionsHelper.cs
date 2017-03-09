using System;
using System.Collections.Generic;
using System.Linq;

namespace MapViewer.Emulation.Blok.Emission.Options
{
    public static class OptionsHelper
    {
        public static TOption Of<TOption>(this ICollection<IEmissionOption> OptionsCollection)
            where TOption : IEmissionOption
        {
            TOption res = OptionsCollection.OfType<TOption>().FirstOrDefault();
            if (res == null)
            {
                throw new ApplicationException(string.Format("Фабрике эмиттеров не была предоставлена опция необходимого типа ({0})",
                                                             typeof (TOption).Name));
            }
            return res;
        }
    }
}
