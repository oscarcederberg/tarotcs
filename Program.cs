using System;
using System.IO;
using CommandLine;

namespace tarot{
    class Program{
        static int Main(string[] args){
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string configPath = Path.Combine(userPath,@".config/tarot/");
            Directory.CreateDirectory(configPath);

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
                options.Amount = Math.Max(1, options.Amount);
                switch(options.Type.ToLower()){
                    case "riffle":
                        for (int i = 0; i < options.Amount; i++){
                            deck.ShuffleDeck(ShuffleType.Riffle);
                        }
                        break;
                    case "overhand":
                        for (int i = 0; i < options.Amount; i++){
                            deck.ShuffleDeck(ShuffleType.Overhand);
                        }
                        break;
                    case "fisheryates" or "perfect":
                        for (int i = 0; i < options.Amount; i++){
                            deck.ShuffleDeck(ShuffleType.FisherYates);
                        }
                        break;
                    default:
                        if (!options.Quiet){
                            Console.WriteLine("Unkown shuffletype: {0}.", options.Type.ToLower());
                        }
                        return 1;
                }
                if (!options.Quiet){
                    if(options.Amount > 1){
                        Console.WriteLine("Performed {0} {1} shuffles.", options.Amount, options.Type.ToLower());
                    }else{
                        Console.WriteLine("Performed {0} shuffle.", options.Type.ToLower());
                    }
                }
                return 0;
            },
            errors => 1);
        }
    }   
}
