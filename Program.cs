﻿using System;
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

        static TarotDeck deck = new TarotDeck();
        static TarotSpreads spreads = new TarotSpreads();

        static int Main(string[] args){
            Type[] types = {
                typeof(ShuffleOptions), typeof(GetOptions), typeof(ResetOptions), typeof(SpreadOptions), 
                typeof(ViewOptions), typeof(MoveOptions), typeof(SwapOptions)
             };

            HandleFiles();

            return Parser.Default.ParseArguments(args, types).MapResult(
                (GetOptions options) => Get(options),
                (ShuffleOptions options) => Shuffle(options),
                (ResetOptions options) => Reset(options),
                (SpreadOptions options) => Spread(options),
                (ViewOptions options) => View(options),
                (MoveOptions options) => Move(options),
                (SwapOptions options) => Swap(options),
                errors => 1
            );
        }

        private static void HandleFiles(){
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

        private static void SaveDeck(){
            File.WriteAllText(currentDeckFilePath, deck.SerializeDeck());
        }

        private static int Get(GetOptions options){
            options.Amount = Math.Max(1, options.Amount);
            Random random = new Random();
            Array orientations = Enum.GetValues(typeof(Orientation));

            for (int i = 0; i < options.Amount; i++){
                TarotCard card = deck.RequeueCard();
                if(options.RandomizeOrientation){
                    Orientation orientation = (Orientation)orientations.GetValue(random.Next(orientations.Length));
                    if(orientation == Orientation.Upright){
                        Console.WriteLine(card.GetName());
                    }else{
                        Console.WriteLine($"Reverse {card.GetName()}");
                    }
                    if(options.Keywords) Console.WriteLine($"\tKeywords: {card.GetKeywords(orientation)}");
                }else{
                    Console.WriteLine(card.GetName());
                    if(options.Keywords) Console.WriteLine($"\tKeywords: {card.GetKeywords()}");
                }
            }
            SaveDeck();
            return 0;
        }

        private static int Shuffle(ShuffleOptions options){
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
                SaveDeck();
                return 0;
        }

        private static int Reset(ResetOptions options){
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
            SaveDeck();
            return 0;
        }

        private static int Spread(SpreadOptions options){
            List<string> names = new List<string>(spreads.Keys);
            string name = names.Find(n => n.ToLower() == options.Name.ToLower());
            
            if(name is not null){
                TarotSpread spread = spreads[name];
                spread.EnqueueCards(deck);
                spread.PrintSpread();
                SaveDeck();
                return 0;
            }else{
                Console.WriteLine("Spread does not exist.");
                return 1;
            }
        } 

        private static int View(ViewOptions options){
            if(options.Card){
                if(options.Name != default){
                    TarotCard card = deck.Cards.Find(c => c.Name.ToLower() == options.Name.ToLower());

                    if(card is not null){
                        Console.WriteLine($"{card.GetName()}: \n\tUpright: {card.GetKeywords(Orientation.Upright)}\n\tReverse: {card.GetKeywords(Orientation.Reverse)}");
                    }else{
                        Console.WriteLine("That card does not exist.");
                        return 1;
                    }
                }else{
                    List<TarotCard> cards_in_order = Utilities.Deserialize<List<TarotCard>>(defaultCardsFilePath);
                    try{
                        cards_in_order.AddRange(Utilities.Deserialize<List<TarotCard>>(userCardsFilePath));
                    }catch (System.Exception){}
                    
                    for (int i = 0; i < cards_in_order.Count; i++){
                        TarotCard card = cards_in_order[i];
                        Console.WriteLine($"{card.GetName()}: \n\tUpright: {card.GetKeywords(Orientation.Upright)}\n\tReverse: {card.GetKeywords(Orientation.Reverse)}");
                    }
                }
            }else if(options.Spread){
                List<string> names = new List<string>(spreads.Keys);
                names.Sort();

                if(options.Name != default){
                    string name = names.Find(n => n.ToLower() == options.Name.ToLower());

                    if(name is not null){
                    TarotSpread spread = spreads[name];

                    Console.WriteLine($"{name}:");
                    for (int i = 0; i < spread.Length(); i++){
                        Console.WriteLine($"\t{i+1}. {spread.Positions[i]}");
                    }
                    }else{
                        Console.WriteLine("That spread does not exist.");
                        return 1;
                    }
                }else{
                    foreach(string name in names){
                        TarotSpread spread = spreads[name];

                        Console.WriteLine($"{name}:");
                        for (int i = 0; i < spread.Length(); i++){
                            Console.WriteLine($"\t{i+1}. {spread.Positions[i]}");
                        }
                    }
                }
            }
            return 0;
        }

        private static int Move(MoveOptions options){
            if(options.OldIndex > deck.Cards.Count -1 || options.OldIndex < 0 ||
                options.NewIndex > deck.Cards.Count -1 || options.NewIndex < 0){
                Console.WriteLine($"Positional index is outside of the deck range [0,{deck.Cards.Count -1}].");
                return 1;
            }else{
                deck.MoveCard(options.OldIndex, options.NewIndex);
                return 0;
            }
        }

        private static int Swap(SwapOptions options){
            if(options.FirstIndex > deck.Cards.Count -1 || options.FirstIndex < 0 ||
                options.SecondIndex > deck.Cards.Count -1 || options.SecondIndex < 0){
                Console.WriteLine($"Positional index is outside of the deck range [0,{deck.Cards.Count -1}].");
                return 1;
            }else{
                deck.SwapCards(options.FirstIndex, options.SecondIndex);
                return 0;
            }
        }
    }   
}