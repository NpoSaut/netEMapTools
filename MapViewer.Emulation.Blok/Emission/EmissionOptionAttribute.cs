using System;
using JetBrains.Annotations;

namespace MapViewer.Emulation.Blok.Emission
{
    [BaseTypeRequired(typeof (IBlokEmitterFactory))]
    public class EmissionOptionAttribute : Attribute
    {
        public EmissionOptionAttribute(Type OptionType) { this.OptionType = OptionType; }
        public Type OptionType { get; private set; }
    }
}
