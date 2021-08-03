namespace tarot{
    public interface ISpread<T, U> where T : IDeck<U> where U : ICard {
        public void EnqueueCards(T deck);

        public string SerializeSpread();

        public string SerializeCards();

        public void PrintSpread();

        public void PrintPositions();

        public int Length();
    }
}