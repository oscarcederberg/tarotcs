using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tarot
{
    public static class Utilities
    {
        public static Random RNG = new Random();

        private static List<(int, string)> romanNumerals = new List<(int, string)>() 
        { 
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"), (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I") 
        };

        public static List<CardMajorArcana> DeserializeMajorArcana(string file)
        {
            return JsonConvert.DeserializeObject<List<CardMajorArcana>>(File.ReadAllText(file));
        }

        public static List<CardMinorArcana> DeserializeMinorArcana(string file)
        {
            return JsonConvert.DeserializeObject<List<CardMinorArcana>>(File.ReadAllText(file));
        }

        public static string ToMinorArcanaValueNotation(int number)
        {
            number = Math.Max(Math.Min(number, 14), 1);
            switch (number)
            {
                case 1:
                    return "Ace";
                case 11:
                    return "Page";
                case 12:
                    return "Knight";
                case 13:
                    return "Queen";
                case 14:
                    return "King";
                default:
                    return ToRoman(number);
            }
        }

        public static string ToRoman(int number)
        {
            number = Math.Max(number, 0);

            if(number == 0){
                return "0";
            }

            String roman = "";
            foreach((int value, string symbol) in romanNumerals)
            {
                while(number >= value) 
                {
                    roman += symbol;
                    number -= value;
                }
            }
            return roman;
        }
    }

    static class ListExtension
    {
        public static T Dequeue<T>(this List<T> list)
        {
            T r = list[0];
            list.RemoveAt(0);
            return r;
        }
    }

    static class StringExtension
    {
        public static string Multiply(this string text, int multiplier)
        {
            return String.Concat(Enumerable.Repeat(text, multiplier));
        }
    }
}