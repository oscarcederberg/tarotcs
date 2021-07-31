using System;
using System.IO;
using System.Collections.Generic;
using CommandLine;

using TarotSpreads = System.Collections.Generic.Dictionary<string, tarot.TarotSpread>;

namespace tarot{
    class Program{
        static string localApplicationDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"tarot/"
        );
        static string currentDeckFilePath = Path.Combine(localApplicationDataPath,@"current_deck.json");
        static string currentSpreadsFilePath = Path.Combine(localApplicationDataPath,@"current_spreads.json");

        static string configurationDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".config/tarot/"
        );
        static string userCardsFilePath = Path.Combine(configurationDataPath, "cards.json");
        static string userSpreadsFilePath = Path.Combine(configurationDataPath, "spreads.json");

        static string defaultCardsFilePath = @"Data/default_cards.json";
        static string defaultSpreadsFilePath = @"Data/default_spreads.json";
        static string templateCardsFilePath = @"Data/template_cards.json";
        static string templateSpreadsFilePath = @"Data/template_spreads.json";

        static int Main(string[] args){
            TarotDeck deck = new TarotDeck();
            TarotSpreads spreads = new TarotSpreads();
            Type[] types = {typeof(ShuffleOptions), typeof(GetOptions), typeof(ResetOptions), typeof(SpreadOptions)};

            HandleFiles(deck, spreads);

            return Parser.Default.ParseArguments(args, types).MapResult(
                (GetOptions options) => Get(options, deck),
                (ShuffleOptions options) => Shuffle(options, deck),
                (ResetOptions options) => Reset(options, deck, spreads),
                (SpreadOptions options) => Spread(options, deck, spreads),
                errors => 1
            );
        }

        private static void HandleFiles(TarotDeck deck, TarotSpreads spreads){
            Directory.CreateDirectory(localApplicationDataPath);
            Directory.CreateDirectory(configurationDataPath);

            if(File.Exists(currentDeckFilePath)){
                deck.DeserializeDeck(currentDeckFilePath);
            }else{
                deck.DeserializeDeck(defaultCardsFilePath);
                File.WriteAllText(currentDeckFilePath, deck.SerializeDeck());
            }

            if(File.Exists(currentSpreadsFilePath)){
                spreads.AddRange(Utilities.Deserialize<TarotSpreads>(currentSpreadsFilePath));
            }else{
                spreads.AddRange(spreads = Utilities.Deserialize<TarotSpreads>(defaultSpreadsFilePath));
                File.WriteAllText(currentSpreadsFilePath, Utilities.Serialize(spreads));
            }

            if(File.Exists(userCardsFilePath)){
                try{
                    deck.DeserializeDeck(userCardsFilePath);
                }catch (System.Exception){};
            }else{
                File.WriteAllText(userCardsFilePath, File.ReadAllText(templateCardsFilePath));
            }

            if(File.Exists(userSpreadsFilePath)){
                try{
                    spreads.AddRange(Utilities.Deserialize<TarotSpreads>(userSpreadsFilePath));
                }catch (System.Exception){};
            }else{
                File.WriteAllText(userSpreadsFilePath, File.ReadAllText(templateSpreadsFilePath));
            }
        }

        static void SaveDeck(TarotDeck deck){
            File.WriteAllText(currentDeckFilePath, deck.SerializeDeck());
        }

        private static int Get(GetOptions options, TarotDeck deck){
            options.Amount = Math.Max(1, options.Amount);
            for (int i = 0; i < options.Amount; i++){
                TarotCard card = deck.RequeueCard();
                Console.WriteLine(card.GetName());
                if(options.Keywords) Console.WriteLine($"\tKeywords: {card.GetKeywords()}");
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

        private static int Reset(ResetOptions options, TarotDeck deck, TarotSpreads spreads){
            deck = new TarotDeck();
            spreads = new TarotSpreads();

            if(options.Deck != default){
                try{
                    deck.DeserializeDeck(options.Deck);
                }catch(System.Exception){
                    if(!options.Quiet) Console.WriteLine($"Failed to load deck from '{options.Deck}' (Malformed file/path?).");
                    deck.DeserializeDeck(defaultCardsFilePath);
                }
            }else{
                deck.DeserializeDeck(defaultCardsFilePath);
            }
            
            if(options.Spreads != default){
                try{
                    spreads.AddRange(Utilities.Deserialize<TarotSpreads>(options.Spreads));
                }catch(System.Exception){
                    if(!options.Quiet) Console.WriteLine($"Failed to load spreads from '{options.Spreads}' (Malformed file/path?).");
                    spreads.AddRange(Utilities.Deserialize<TarotSpreads>(defaultSpreadsFilePath));
                }
            }else{
                spreads.AddRange(Utilities.Deserialize<TarotSpreads>(defaultSpreadsFilePath));
            }

            if(!options.Quiet) Console.WriteLine("The deck and spreads have been reset to default.");
            SaveDeck(deck);
            return 0;
        }

        private static int Spread(SpreadOptions options, TarotDeck deck, TarotSpreads spreads){
            List<string> names = new List<string>(spreads.Keys);

            if(options.ListAll){
                names.Sort();
                for (int i = 0; i < names.Count; i++){
                    string name = names[i];
                    Console.WriteLine($"{i+1}. {name}:");
                    TarotSpread spread = spreads[name];
                    for (int j = 0; j < spread.Length(); j++){
                        Console.WriteLine($"\t{j+1}) {spread.Positions[j]}");
                    }
                }
            }else if(options.Name != default){
                if(names.Contains(options.Name)){
                    TarotSpread spread = spreads[options.Name];
                    if(options.List){
                        Console.WriteLine($"{options.Name}:"); 
                        for (int j = 0; j < spread.Length(); j++){
                            Console.WriteLine($"\t{j+1}) {spread.Positions[j]}");
                        }
                    }else{
                        spread.AddCards(deck);
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
    }   
}