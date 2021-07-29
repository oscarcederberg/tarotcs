using System;

namespace tarot{
    public class TarotSpread : ISpread<TarotDeck, TarotCard>{
        private TarotCard[] _spread_cards;
        private string[] _spread_positions;

        public TarotSpread(string[] positions, TarotDeck deck){
            this._spread_positions = positions;
            this._spread_cards = new TarotCard[GetLength()];
            AddCards(deck);
        }

        public void AddCards(TarotDeck deck){
            for (int i = 0; i < GetLength(); i++){
                this._spread_cards[i] = deck.RequeueCard();
            }
        }

        public string SerializeSpread(){
            return Utilities.Serialize<TarotCard[]>(this._spread_cards);
        }

        public void PrintSpread(){
            for (int i = 0; i < GetLength(); i++){
                Console.WriteLine($"{i}. {_spread_positions[i]}:\n\t{_spread_cards[i]}");
            }
        }

        public int GetLength(){
            return _spread_positions.Length;
        }
    }
}