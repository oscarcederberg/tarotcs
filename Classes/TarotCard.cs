namespace tarot{
    public class TarotCard : ICard{
        public readonly string Name;
        public readonly string Keywords;

        public TarotCard(string name, string keywords){
            this.Name = name;
            this.Keywords = keywords;
        }

        public string GetName(){
            return Name;
        }

        public string GetKeywords(){
            return Keywords;
        }
    }
}