using System;
using System.IO;
using CommandLine;

namespace tarot{
    class Program{
        static string userPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString();
        static string configPath = Path.Combine(userPath,@"tarot/");
        static string filePath = Path.Combine(configPath,@"current_deck.json");
        static string defaultFilePath = @"Data/default_cards.json";

        static int Main(string[] args){
            Directory.CreateDirectory(configPath);
            TarotDeck deck = new TarotDeck();

            if(File.Exists(filePath)){
                deck.DeserializeDeck(filePath);
            }else{
                deck.DeserializeDeck(defaultFilePath);
                File.WriteAllText(filePath, deck.SerializeDeck());
            }

            Type[] types = {typeof(ShuffleOptions), typeof(GetOptions), typeof(ResetOptions)};
            return Parser.Default.ParseArguments(args, types)
                .MapResult(
                (GetOptions options) => Get(options, deck),
                (ShuffleOptions options) => Shuffle(options, deck),
                (ResetOptions options) => Reset(options, deck),
                errors => 1);
        }

        private static int Get(GetOptions options, TarotDeck deck){
            options.Amount = Math.Max(1, options.Amount);
                for (int i = 0; i < options.Amount; i++){
                    Console.WriteLine(deck.RequeueCard().ToString());
                }
                File.WriteAllText(filePath, deck.SerializeDeck());
                return 0;
        }

        private static int Shuffle(ShuffleOptions options, TarotDeck deck){
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
                File.WriteAllText(filePath, deck.SerializeDeck());
                return 0;
        }

        private static int Reset(ResetOptions options, TarotDeck deck){
            deck.DeserializeDeck(defaultFilePath);
            if(!options.Quiet) Console.WriteLine("The deck has been reset to default.");
            File.WriteAllText(filePath, deck.SerializeDeck());
            return 0;
        }
    }   
}