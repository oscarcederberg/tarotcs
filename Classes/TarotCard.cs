namespace tarot{
    public enum Orientation{
        Upright,
        Reverse
    }

    public class TarotCard{
        public readonly string Name;
        public readonly string KeywordsUpright;
        public readonly string KeywordsReverse;

        public TarotCard(string name, string keywordsUpright, string keywordsReverse){
            this.Name = name;
            this.KeywordsUpright = keywordsUpright;
            this.KeywordsReverse = keywordsReverse;
        }

        public string GetName(){
            return Name;
        }

        public string GetKeywords(Orientation orientation = Orientation.Upright){
            if(orientation is Orientation.Upright){
                return KeywordsUpright;
            }else{
                return KeywordsReverse;
            }    
        }
    }
}