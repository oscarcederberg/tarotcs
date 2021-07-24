namespace tarot{
    public interface ILogger<T> : IObserver<T>{
        public void AddLog(Log log);
    }
}
