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

        // Epic - View latest free game
        [SlashCommand("free-games", "Use this to see the current free game available on epic games.")]
        public async Task EpicGames()
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = "FREE THIS WEEK"
            };
            embed
            .WithFooter($"Requested by {Context.User.Username}")
            .WithTimestamp(DateTimeOffset.Now);
            string jsonString = new WebClient().DownloadString("https://store-site-backend-static.ak.epicgames.com/freeGamesPromotions");
            JToken token = JToken.Parse(jsonString);
            var elements = token.SelectToken("data").SelectToken("Catalog").SelectToken("searchStore").SelectToken("elements");
            var imageURL = "";
            string title = "Title";
            string description = "Description";
            string pageURL = "https://www.epicgames.com/store/en-US/p/";
            var gameLink = "";
            var appLink = "";
            elements.Take(1);
            foreach (var element in elements)
            {
                var isPromotion = element.SelectToken("promotions");
                var images = element.SelectToken("keyImages");
                if (isPromotion.HasValues)
                {
                    var offers = isPromotion.SelectToken("promotionalOffers");
                    if (offers.HasValues)
                    {
                        var offerss = offers[0].SelectToken("promotionalOffers");
                        if (offerss.HasValues)
                        {
                            title = (string)element.SelectToken("title");
                            description = (string)element.SelectToken("description");
                            gameLink = (string)element.SelectToken("productSlug");
                            appLink = "https://www.epicfreegames.net/redirect?slug=" + gameLink;
                            gameLink = pageURL + gameLink;
                            if (images.HasValues)
                            {
                                var imagess = images[0].SelectToken("url");
                                imageURL = imagess.ToString();
                            }
                        }
                    }
                }
            }
            embed.AddField(title, description, false).WithImageUrl(imageURL).WithUrl(gameLink);
            var builder = new ComponentBuilder()
            .WithButton("View in Browser", null, ButtonStyle.Link, null, gameLink);
            builder
             .WithButton("View in Launcher", null, ButtonStyle.Link, null, appLink);
            await RespondAsync(embed: embed.Build(), components: builder.Build());
        }

    }
}