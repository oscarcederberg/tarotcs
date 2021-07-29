using System;
using System.IO;
using System.Collections.Generic;
using CommandLine;

namespace tarot{
    class Program{
        static string userPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString();
        static string configPath = Path.Combine(userPath,@"tarot/");
        static string filePath = Path.Combine(configPath,@"current_deck.json");
        static string defaultCardsFilePath = @"Data/default_cards.json";
        static string defaultSpreadsFilePath = @"Data/default_spreads.json";

        static int Main(string[] args){
            Directory.CreateDirectory(configPath);
            TarotDeck deck = new TarotDeck();
            Dictionary<string, string[]> spreads_uninitialized = new Dictionary<string, string[]>();

            if(File.Exists(filePath)){
                deck.DeserializeDeck(filePath);
            }else{
                deck.DeserializeDeck(defaultCardsFilePath);
                File.WriteAllText(filePath, deck.SerializeDeck());
            }
            spreads_uninitialized = Utilities.Deserialize<Dictionary<string, string[]>>(defaultSpreadsFilePath);

            Type[] types = {typeof(ShuffleOptions), typeof(GetOptions), typeof(ResetOptions), typeof(SpreadOptions)};
            return Parser.Default.ParseArguments(args, types)
                .MapResult(
                (GetOptions options) => Get(options, deck),
                (ShuffleOptions options) => Shuffle(options, deck),
                (ResetOptions options) => Reset(options, deck),
                (SpreadOptions options) => Spread(options, spreads_uninitialized, deck),
                errors => 1);
        }

        private static int Get(GetOptions options, TarotDeck deck){
            Console.WriteLine(options.Amount);
            options.Amount = Math.Max(1, options.Amount);
            for (int i = 0; i < options.Amount; i++){
                Console.WriteLine(deck.RequeueCard().ToString());
            }
            SaveDeck(deck);
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
                SaveDeck(deck);
                return 0;
        }

        private static int Reset(ResetOptions options, TarotDeck deck){
            deck.DeserializeDeck(defaultCardsFilePath);
            if(!options.Quiet) Console.WriteLine("The deck has been reset to default.");
            SaveDeck(deck);
            return 0;
        }

        private static int Spread(SpreadOptions options, Dictionary<string, string[]> spreads_unitit, TarotDeck deck){
            List<string> spread_names = new List<string>(spreads_unitit.Keys);

            if(options.ListAll){      
                spread_names.Sort();
                for (int i = 0; i < spread_names.Count; i++){
                    string name = spread_names[i];
                    Console.WriteLine($"{i+1}. {name}:");
                    string[] positions = spreads_unitit[name];
                    for (int j = 0; j < positions.Length; j++){
                        Console.WriteLine($"\t{j+1}) {positions[j]}");
                    }
                }
            }else if(options.Name != default){
                if(spread_names.Contains(options.Name)){
                    if(options.List){
                        Console.WriteLine($"{options.Name}:");
                        string[] positions = spreads_unitit[options.Name];
                        for (int i = 0; i < positions.Length; i++){
                            Console.WriteLine($"\t{i+1}) {positions[i]}");
                        }
                    }else{
                        TarotSpread spread = new TarotSpread(spreads_unitit[options.Name], deck);
                        spread.PrintSpread();
                    }
                }else{
                    Console.WriteLine("Spread does not exist.");
                    return 1;
                }
            }else{
                return 1;
            }
            SaveDeck(deck);
            return 0;
        }

        static void SaveDeck(TarotDeck deck){
            File.WriteAllText(filePath, deck.SerializeDeck());
        }
    }   
}