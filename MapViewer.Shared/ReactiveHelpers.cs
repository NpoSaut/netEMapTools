using System;
using System.Reactive.Linq;

namespace MapViewer
{
    public static class ReactiveHelpers
    {
        public static IObservable<T> DisposePrevious<T>(this IObservable<T> Source)
            where T : IDisposable
        {
            return Source.Scan((previous, current) =>
                               {
                                   previous.Dispose();
                                   return current;
                               });
        }
    }
}
