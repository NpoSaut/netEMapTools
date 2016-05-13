using System;
using Geographics;

namespace EMapNavigator.Emition
{
    public class LanEmitter : IEmitter
    {
        public void EmitPosition(EarthPoint Position)
        {
            Console.WriteLine("~ ~ ~ ~ {0} ~ ~ ~ ~", Position);
        }
    }
}
