namespace tarot{
    public class TarotCard : ICard{
        private readonly string _name;
        public TarotCard(int id, string name){
            this._name = name;
        }

        public override string ToString(){
            return this._name;
        }
    }
}