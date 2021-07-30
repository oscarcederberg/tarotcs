using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tarot{
    public static class Utilities{
        public static Random RNG = new Random();

        private static List<(int, string)> romanNumerals = new List<(int, string)>(){ 
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"), (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I") 
        };

        public static T Deserialize<T>(string filePath){
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public static string Serialize<T>(T obj){
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static string ToRoman(int number){
            number = Math.Max(number, 0);

            if(number == 0){
                return "0";
            }

            String roman = "";
            foreach((int value, string symbol) in romanNumerals){
                while(number >= value){
                    roman += symbol;
                    number -= value;
                }
            }
            return roman;
        }
    }

    static class ListExtension{
        public static T Dequeue<T>(this List<T> list){
            T r = list[0];
            list.RemoveAt(0);
            return r;
        }
    }

    static class StringExtension{
        public static string Multiply(this string text, int multiplier){
            return String.Concat(Enumerable.Repeat(text, multiplier));
        }
    }

    static class DictionaryExtension{
        public static void AddRange<T,U>(this Dictionary<T,U> target, Dictionary<T,U> source)
        {
            if(target is null) throw new ArgumentNullException(nameof(target));
            if(source is null) throw new ArgumentNullException(nameof(source));
            foreach(T key in source.Keys){
                U value = source[key];
                if(target.ContainsKey(key)){
                    target[key] = value;
                }else{
                    target.Add(key, value);
                }
            }
        }
    }
}