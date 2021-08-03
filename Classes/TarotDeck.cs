using System;
using System.Collections.Generic;

namespace tarot{
    public enum ShuffleType{
        FisherYates,
        Overhand,
        Riffle
    }

    public class TarotDeck{
        public readonly List<TarotCard> Cards;

        public TarotDeck(){
            this.Cards = new List<TarotCard>();
        }

        public void DeserializeDeck(string filePath){
            Cards.AddRange(Utilities.Deserialize<List<TarotCard>>(filePath));
        }

        public string SerializeDeck(){
            return Utilities.Serialize<List<TarotCard>>(Cards);
        }

        public TarotCard RequeueCard() {
            TarotCard card = Cards.Dequeue();
            Cards.Add(card);
            return card;
        }

        public void PrintDeck(){
            foreach(TarotCard card in this.Cards){
                Console.WriteLine(card.GetName());
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

        public void SwapCards(int firstIndex, int secondIndex){
            TarotCard temp = Cards[firstIndex];
            Cards[firstIndex] = Cards[secondIndex];
            Cards[secondIndex] = temp;
        }

        public void MoveCard(int oldIndex, int newIndex){
            TarotCard card = Cards[oldIndex];
            Cards.RemoveAt(oldIndex);
            if(newIndex > oldIndex) newIndex--; 
            Cards.Insert(newIndex, card);
        }

        private void ShuffleFisherYates(){  
            for (int i = Cards.Count - 1; i >= 0; i--){
                int k = Utilities.RNG.Next(i + 1);
                TarotCard card = Cards[k];
                Cards[k] = Cards[i];
                Cards[i] = card;
            }
        }

        private void ShuffleOverhand(){
            int cut = Utilities.RNG.Next(Cards.Count);
            for (int i = 0; i < cut; i++){
                TarotCard card = Cards.Dequeue();
                Cards.Add(card);
            }
        }

        private void ShuffleRiffle(){
            int cut = Utilities.RNG.Next(Cards.Count);
            int count = Cards.Count;
            List<TarotCard> left = Cards.GetRange(0, cut);
            List<TarotCard> right = Cards.GetRange(cut, count - cut);
            Cards.Clear();

            while(left.Count > 0 && right.Count > 0){
                Cards.Add(left.Dequeue());
                Cards.Add(right.Dequeue());
            }

            if(left.Count == 0){
                Cards.AddRange(right);
            } 
            else if(right.Count == 0){
                Cards.AddRange(left);
            }
        }
    }
}