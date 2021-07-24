namespace tarot.Subscription{
    public interface ISubscriber<T>{
        public void Notify(T value);
    }
}
