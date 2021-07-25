namespace Subscription{
    public interface ISubscriber<T>{
        public void Notify(T value);
    }
}
