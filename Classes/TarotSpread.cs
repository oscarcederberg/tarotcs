using System;

namespace tarot{
    public class TarotSpread : ISpread<TarotDeck, TarotCard>{
        public readonly string[] Positions;

        private TarotCard[] _spread_cards;

        public TarotSpread(string[] positions){
            this.Positions = positions;
            this._spread_cards = new TarotCard[Length()];
        }

        public void AddCards(TarotDeck deck){
            for (int i = 0; i < Length(); i++){
                this._spread_cards[i] = deck.RequeueCard();
            }
        }

        public string SerializeSpread(){
            return Utilities.Serialize<TarotSpread>(this);
        }

        public string SerializeCards(){
            return Utilities.Serialize<TarotCard[]>(this._spread_cards);
        }

        public void PrintSpread(){
            for (int i = 0; i < Length(); i++){
                Console.WriteLine($"{i+1}. {Positions[i]}:\n\t{_spread_cards[i]}");
            }
        }

        public void PrintPositions(){
            for (int i = 0; i < Length(); i++){
                Console.WriteLine($"{i+1}. {Positions[i]}");
            }
        }

        public int Length(){
            return Positions.Length;
        }
    }
}