using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MapViewer
{
    public interface IMainMenuService
    {
        void RegisterCommand(MenuPath Path, ICommand Command);
        void RegisterCheckbox(MenuPath Path, Func<bool> Getter, Action<bool> Setter);
    }

    public class MenuPath
    {
        public MenuPath() : this(new List<string>()) { }

        public MenuPath(IList<string> Segments)
        {
            this.Segments = Segments;
        }

        public IList<string> Segments { get; }

        public static implicit operator MenuPath(string FirstSegment)
        {
            return new MenuPath(new[] { FirstSegment });
        }

        public static MenuPath operator /(MenuPath Path, string Segment)
        {
            return new MenuPath(Path.Segments.Concat(new[] { Segment }).ToList());
        }
    }
}
