using System.Windows.Input;

namespace MapViewer.InterfaceElements
{
    public class ToolbarCommand : IToolbarCommand
    {
        public ToolbarCommand(string Name, ICommand Command) : this(Name, null, Command) { }

        public ToolbarCommand(string Name, string Description, ICommand Command)
        {
            this.Name = Name;
            this.Description = Description;
            this.Command = Command;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICommand Command { get; private set; }
    }
}