using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using MapViewer;
using MapVisualization.Annotations;
using ReactiveUI;

namespace EMapNavigator
{
    [UsedImplicitly]
    public class WindowTitleManager : IWindowTitleManager
    {
        private readonly ReactiveList<TitlePiece> _pieces;

        public WindowTitleManager()
        {
            _pieces = new ReactiveList<TitlePiece>(new[] { new TitlePiece(0, "MapViewer") });
            _pieces.Changed
                   .StartWith(new object())
                   .Select(l => string.Join(" ", _pieces.OrderBy(p => p.Order).Select(p => p.Text)))
                   .Subscribe(title => Application.Current.MainWindow.Title = title);
        }

        public IDisposable PutText(int Order, string Text)
        {
            var piece = new TitlePiece(Order, Text);
            _pieces.Add(piece);
            return new PieceToken(this, piece);
        }

        public string Title
        {
            get { return string.Join(" ", _pieces); }
        }

        private void Delete(TitlePiece Piece) { _pieces.Remove(Piece); }

        private class TitlePiece
        {
            public TitlePiece(int Order, string Text)
            {
                this.Order = Order;
                this.Text = Text;
            }

            public int Order { get; }
            public string Text { get; }
        }

        private class PieceToken : IDisposable
        {
            private readonly TitlePiece _item;
            private readonly WindowTitleManager _manager;

            public PieceToken(WindowTitleManager Manager, TitlePiece Item)
            {
                _manager = Manager;
                _item = Item;
            }

            public void Dispose() { _manager.Delete(_item); }
        }
    }
}
