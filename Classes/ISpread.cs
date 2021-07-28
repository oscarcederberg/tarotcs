using System.Collections.Generic;

namespace tarot{
    public interface ISpread<T> where T : ICard{
        public void AddCards(IEnumerable<T> cards);

        public string SerializeSpread();

        public void PrintSpread();

        public int Size();
    }
}