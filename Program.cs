using System;
using CommandLine;

namespace tarot{
    class Program{
        [Verb("get", HelpText = "Retrieve cards from the deck.")]
        public class GetOptions{
            [Value(0, MetaName = "amount", MetaValue = "int", Default = 1, HelpText = "Amount of cards to retrieve.")]
            public uint Amount{get; set;}
        }

        [Verb("shuffle", HelpText = "Shuffle the deck")]
        public class ShuffleOptions{
            [Value(0, MetaName = "type", MetaValue = "string", Default = "riffle", HelpText = "What shuffle to perform.")]
            public string Type{get; set;}
        }

        static int Main(string[] args){
            Type[] types = {typeof(ShuffleOptions), typeof(GetOptions)};

            Deck deck = new Deck();
            deck.AddToDeck(Utilities.DeserializeMajorArcana(@"Data/base_majorarcana.json"));
            deck.AddToDeck(Utilities.DeserializeMinorArcana(@"Data/base_minorarcana.json"));
            
            return Parser.Default.ParseArguments(args, types)
            .MapResult(
            (GetOptions options) => {
                options.Amount = Math.Max(1, options.Amount);
                for (int i = 0; i < options.Amount; i++){
                    Console.WriteLine(deck.RequeueCard().ToString());
                }
                return 0;
            },
            (ShuffleOptions options) => {
                switch(options.Type.ToLower()){
                    case "riffle":
                        deck.ShuffleDeck(ShuffleType.Riffle);
                        break;
                    case "overhand":
                        deck.ShuffleDeck(ShuffleType.Overhand);
                        break;
                    case "fisheryates" or "perfect":
                        deck.ShuffleDeck(ShuffleType.FisherYates);
                        break;
                    default:
                        Console.WriteLine("Unkown shuffletype: {0}", options.Type.ToLower());
                        return 1;
                }
                Console.WriteLine("Performed {0} shuffle", options.Type.ToLower());
                return 0;
            },
            errors => 1);
        }
    }   
}
