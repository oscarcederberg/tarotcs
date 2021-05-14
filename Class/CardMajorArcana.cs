using System;
using System.Collections.Generic;

namespace tarot
{
    public class CardMajorArcana : ICard
    {
        public readonly int Value;
        private readonly string _name;
        public CardMajorArcana(int value, string name){
            this.Value = value % 22;
            this._name = Utilities.ToRoman(this.Value) + " - " + name;
        }

        public override string ToString()
        {
            return this._name;
        }
    }
}