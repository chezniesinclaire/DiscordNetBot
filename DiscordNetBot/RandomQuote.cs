using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordNetBot
{
    class RandomQuotes
    {
        private static List<string> _quotes = new List<string>
        {
            "Chuck Norris doesn’t read books. He stares them down until he gets the information he wants.",
            "If you spell Chuck Norris Nads in Scrabble, you win. Forever.",
            "Chuck Norris breathes air … five times a day.",
            "If Chuck Norris were to travel to an alternate dimension in which there was another Chuck Norris and they both fought, they would both win.",
            "The dinosaurs looked at Chuck Norris the wrong way once. You know what happened to them.",
            "Chuck Norris does not sleep. He waits.",
            "Chuck Norris counted to infinity… twice.",
        };

        public static string RandomQuote()
        {
            return _quotes.Shuffle();
        }
    }
}
