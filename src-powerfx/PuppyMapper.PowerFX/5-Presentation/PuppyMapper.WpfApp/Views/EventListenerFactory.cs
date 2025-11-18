using System.Reactive.Linq;
using ReactiveUI;

namespace PuppyMapper.WPFApp.Views;

public static class EventListenerFactory
{
    public static IObservable<T> AddThrottleListener<T>(this IObservable<T> observable)
    {
        return observable.Throttle(TimeSpan.FromMilliseconds(300)) // adjust interval as needed
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
            ;
    }
}