using System;
using System.Collections.Generic;

namespace tarot{
    class Program{
        static void Main(string[] args){
            Deck deck = new Deck();
            deck.AddToDeck(Utilities.DeserializeMajorArcana(@"Data/base_majorarcana.json"));
            deck.AddToDeck(Utilities.DeserializeMinorArcana(@"Data/base_minorarcana.json"));
            MainUI ui = new MainUI(deck);
            ui.Show();
        }       
    }   
}
