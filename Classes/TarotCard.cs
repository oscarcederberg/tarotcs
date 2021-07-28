namespace tarot{
    public class TarotCard : ICard{
        public readonly string Name;
        public TarotCard(string name){
            this.Name = name;
        }

        public override string ToString(){
            return Name;
        }
    }
}