namespace tarot{
    public interface IObserver<T>{
        public void Notify(T value);
    }
}
