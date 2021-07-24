using System;
using System.Collections.Generic;

namespace tarot{
    public enum ShuffleType{
        FisherYates,
        Overhand,
        Riffle
    }

    public class Deck{
        private List<ICard> _deck;

        public Deck(){
            this._deck = new List<ICard>();
        }

        public void AddToDeck(IEnumerable<ICard> collection){
            this._deck.AddRange(collection);
        }

        public ICard RequeueCard() {
            ICard card = _deck.Dequeue();
            _deck.Add(card);
            return card;
        }

        public void PrintDeck(){
            foreach(ICard card in this._deck){
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
                ICard card = _deck[k];
                _deck[k] = _deck[i];
                _deck[i] = card;
            }
        }

        private void ShuffleOverhand(){
            int cut = Utilities.RNG.Next(_deck.Count);
            for (int i = 0; i < cut; i++){
                ICard card = _deck.Dequeue();
                _deck.Add(card);
            }
        }

        private void ShuffleRiffle(){
            int cut = Utilities.RNG.Next(_deck.Count);
            int count = _deck.Count;
            List<ICard> left = _deck.GetRange(0, cut);
            List<ICard> right = _deck.GetRange(cut, count - cut);
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