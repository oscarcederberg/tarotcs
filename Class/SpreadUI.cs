using System;

namespace tarot{
    class SpreadUI : IUserInterface{
        private Deck deck;
        private Boolean _active;

        public SpreadUI(Deck deck){
            this.deck = deck;
        }

        public void Show(){
            throw new NotImplementedException();
        }

        public IUserInterface Switch(IUserInterface ui){
            throw new NotImplementedException();
        }
        public bool IsActive(){
            return _active;
        }
    }
}
