using CommandLine;

namespace tarot{
    [Verb("get", HelpText = "Retrieve cards from the deck.")]
    public class GetOptions{
        [Value(0, MetaName = "amount", MetaValue = "uint", Required = false, Default = 1u, HelpText = "Amount of cards to retrieve.")]
        public uint Amount{get; set;}

        [Option('r',"randomize-orientation", Default = false, HelpText = "Randomize orientation of retrieved cards.")]
        public bool RandomizeOrientation{get; set;}

        [Option('k',"keywords", HelpText = "Print out keywords related to the retrieved cards.")]
        public bool Keywords{get; set;}
    }

    [Verb("shuffle", HelpText = "Shuffle the deck.")]
    public class ShuffleOptions{
        [Value(0, MetaName = "type", MetaValue = "string", Required = false, Default = "riffle", HelpText = "What shuffle to perform.")]
        public string Type{get; set;}

        [Value(1, MetaName = "amount", MetaValue = "uint", Required = false, Default = 1u, HelpText = "Number of shuffles to perform.")]
        public uint Amount{get; set;}

        [Option('q',"quiet", HelpText = "Suppress stdout.")]
        public bool Quiet{get; set;}
    }

    [Verb("reset", HelpText = "Reset deck to default.")]
    public class ResetOptions{
        [Option('d',"load-deck", HelpText = "Filepath to custom deck-file to load.")]
        public string Deck{get; set;}

        [Option('s',"load-spreads", HelpText = "Filepath to custom spreads-file to load.")]
        public string Spreads{get; set;}

        [Option('q',"quiet", HelpText = "Suppress stdout.")]
        public bool Quiet{get; set;}
    }

    [Verb("spread", HelpText = "Perform a tarot spread.")]
    public class SpreadOptions{
        [Value(0, MetaName = "name", MetaValue = "string", Required = true, HelpText = "Name of spread to perform.")]
        public string Name{get; set;}
    }

    [Verb("view", HelpText = "View all available cards or spreads.")]
    public class ViewOptions{
        [Option('c',"card",  SetName = "cards", MetaValue = "string", Required = true, HelpText = "List all, or view a specific card.")]
        public bool Card{get; set;}

        [Option('s',"spread",  SetName = "spread", MetaValue = "string", Required = true, HelpText = "List all, or view a specific spread.")]
        public bool Spread{get; set;}

        [Value(0, MetaName = "name", MetaValue = "string", Required = false, HelpText = "Name of card or spread to view. Leave empty for all.")]
        public string Name{get; set;}
    }

    [Verb("move", HelpText = "Move a card to a new position in the deck.")]
    public class MoveOptions{
        [Value(0, MetaName = "card index", MetaValue = "int", Required = true, HelpText = "Position of card to move.")]
        public int OldIndex{get; set;}

        [Value(0, MetaName = "new index", MetaValue = "int", Required = true, HelpText = "Position of where to move the card to.")]
        public int NewIndex{get; set;}
    }

    [Verb("swap", HelpText = "Swap the positions of two cards in the deck")]
    public class SwapOptions{
        [Value(0, MetaName = "first card index", MetaValue = "int", Required = true, HelpText = "Position of first card to swap.")]
        public int FirstIndex{get; set;}

        [Value(0, MetaName = "second card index", MetaValue = "int", Required = true, HelpText = "Position of second card to swap.")]
        public int SecondIndex{get; set;}
    }
}