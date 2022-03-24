using Discord;
using Discord.Interactions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiscordNetBot.Modules
{
    public class Slash2FunModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;

        public Slash2FunModule(CommandHandler handler)
        {
            _handler = handler;
        }

        // Kanye.rest - quote of the day
        [SlashCommand("kanye", "Use this for a Kanye quote.")]
        public async Task Kanye()
        {
            var imageURL = "https://thumbs.dreamstime.com/b/kanye-west-83039299.jpg";
            string title = "hello";
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = "KANYE SAYS...",
                ThumbnailUrl = imageURL
            };
            string jsonString = new WebClient().DownloadString("https://api.kanye.rest");
            JToken token = JToken.Parse(jsonString);
            title = (string)token.SelectToken("quote");
            embed.AddField(title, "- Kanye", false);
            await RespondAsync(embed: embed.Build());
        }

        // Chuck Norris facts
        [SlashCommand("chuck-norris", "Use this to find out facts about Chuck Norris.")]
        [DefaultPermission(true)]
        public async Task ChuckFacts()
        {
            var rnd = RandomQuotes.RandomQuote();
            await RespondAsync("Random Chuck Norris fact: " + rnd);
        }

    }
}