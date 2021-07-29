namespace tarot{
    public interface ISpread<T, U> where T : IDeck<U> where U : ICard {
        public void AddCards(T deck);

        public string SerializeSpread();

        public void PrintSpread();

        public int GetLength();
    }
}