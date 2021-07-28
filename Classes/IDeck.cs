namespace tarot{
    public interface IDeck<T> where T : ICard{
        public void DeserializeDeck(string filePath);

        public string SerializeDeck();

        public T RequeueCard();

        public void PrintDeck();

        public void ShuffleDeck(ShuffleType shuffleType);
    }
}