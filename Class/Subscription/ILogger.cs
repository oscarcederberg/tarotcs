namespace tarot.Subscription{
    public interface ILogger<T> : ISubscriber<T>{
        public void AddLog(Log log);
    }
}
