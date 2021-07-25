using CommandLine;

namespace tarot{
    [Verb("get", HelpText = "Retrieve cards from the deck.")]
            public class GetOptions{
                [Value(0, MetaName = "amount", MetaValue = "int", Default = 1, HelpText = "Amount of cards to retrieve.")]
                public uint Amount{get; set;}
            }

    [Verb("shuffle", HelpText = "Shuffle the deck")]
    public class ShuffleOptions{
        [Value(0, MetaName = "type", MetaValue = "string", Default = "riffle", HelpText = "What shuffle to perform.")]
        public string Type{get; set;}
        [Value(1, MetaName = "amount", MetaValue = "int", Default = 1, HelpText = "Number of shuffles to perform.")]
        public int Amount{get; set;}
        [Option('q',"quiet", HelpText = "Suppress stdout.")]
        public bool Quiet{get; set;}
    }
}