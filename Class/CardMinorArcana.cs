using System;
using System.Collections.Generic;

namespace tarot
{
    public class CardMinorArcana : ICard
    {
        public readonly int Value;
        public readonly string Suit;
        private readonly string _name;
        public CardMinorArcana(int value, string suit){
            this.Value = (value % 14) + 1;
            this.Suit = suit;
            this._name = Utilities.ToMinorArcanaValueNotation(this.Value) + " of " + this.Suit;
        }

        public override string ToString()
        {
            return this._name;
        }
    }
}