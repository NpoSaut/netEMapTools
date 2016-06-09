using System.Collections.Generic;

namespace MapViewer.InterfaceElements
{
    public class ToolbarCommandGroup : IToolbarCommandGroup
    {
        /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.Object" />.</summary>
        public ToolbarCommandGroup(params IToolbarCommand[] Commands) { this.Commands = Commands; }

        public IList<IToolbarCommand> Commands { get; private set; }
    }
}
