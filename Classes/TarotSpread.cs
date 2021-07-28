using System;
using System.Collections.Generic;

namespace tarot{
    public class TarotSpread : ISpread<TarotCard>{
        private List<TarotCard> _spread;

        public TarotSpread(){
            this._spread = new List<TarotCard>();
        }

        public void AddCards(IEnumerable<TarotCard> cards){
            this._spread.AddRange(cards);
        }

        public string SerializeSpread(){
            return Utilities.Serialize<List<TarotCard>>(this._spread);
        }

        public void PrintSpread(){
            foreach(TarotCard card in this._spread){
                Console.WriteLine(card.ToString());
            }
        }

        public int Size(){
            return _spread.Count;
        }
    }
}