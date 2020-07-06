using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace SimpleMsul
{
    internal static class Extensions
    {
        public static IObservable<T> DisposePrevious<T>(this IObservable<T> Source)
            where T : class, IDisposable
        {
            return Observable.Using(() => new SerialDisposable(),
                                    d => Source.Do(i => d.Disposable = i));
        }

        public static T DisposeWith<T>(this T Target, CompositeDisposable Disposable)
            where T : IDisposable
        {
            Disposable.Add(Target);
            return Target;
        }
    }
}