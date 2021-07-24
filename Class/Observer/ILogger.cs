namespace tarot.Observer{
    public interface ILogger<T> : IObserver<T>{
        public void AddLog(Log log);
    }
}
