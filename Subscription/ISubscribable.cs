namespace Subscription{
    public interface ISubscribable<T>{
        public void Subscribe(ISubscriber<T> subscriber);
        public void NotifySubscribers(T value);
    }
}