using System;
using System.Collections.Generic;

namespace tarot{
    public enum ShuffleType{
        FisherYates,
        Overhand,
        Riffle
    }

    public class TarotDeck : IDeck<TarotCard>{
        private List<TarotCard> _deck;

        public TarotDeck(){
            this._deck = new List<TarotCard>();
        }

        public void DeserializeDeck(string filePath){
            _deck = new List<TarotCard>();
            _deck.AddRange(Utilities.Deserialize<List<TarotCard>>(filePath));
        }

        public string SerializeDeck(){
            return Utilities.Serialize<List<TarotCard>>(_deck);
        }

        public TarotCard RequeueCard() {
            TarotCard card = _deck.Dequeue();
            _deck.Add(card);
            return card;
        }

        public void PrintDeck(){
            foreach(TarotCard card in this._deck){
                Console.WriteLine(card.ToString());
            }
        }

        public void ShuffleDeck(ShuffleType shuffleType){
            switch (shuffleType){
                case ShuffleType.FisherYates:
                    ShuffleFisherYates();
                    break;
                case ShuffleType.Overhand:
                    ShuffleOverhand();
                    break;
                case ShuffleType.Riffle:
                    ShuffleRiffle();
                    break; 
            }
        }

        private void ShuffleFisherYates(){  
            for (int i = _deck.Count - 1; i >= 0; i--){
                int k = Utilities.RNG.Next(i + 1);
                TarotCard card = _deck[k];
                _deck[k] = _deck[i];
                _deck[i] = card;
            }
        }

        private void ShuffleOverhand(){
            int cut = Utilities.RNG.Next(_deck.Count);
            for (int i = 0; i < cut; i++){
                TarotCard card = _deck.Dequeue();
                _deck.Add(card);
            }
        }

        private void ShuffleRiffle(){
            int cut = Utilities.RNG.Next(_deck.Count);
            int count = _deck.Count;
            List<TarotCard> left = _deck.GetRange(0, cut);
            List<TarotCard> right = _deck.GetRange(cut, count - cut);
            _deck.Clear();

            while(left.Count > 0 && right.Count > 0){
                _deck.Add(left.Dequeue());
                _deck.Add(right.Dequeue());
            }

            if(left.Count == 0){
                _deck.AddRange(right);
            } 
            else if(right.Count == 0){
                _deck.AddRange(left);
            }
        }
    }
}