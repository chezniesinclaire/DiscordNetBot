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
        [SlashCommand("kanye", "Use this for a daily Kanye West quote.")]
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
        [SlashCommand("free-games", "Use this to see the current free epic game.")]
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
            string title = "hello";
            string description = "hello";
            var gameLink = "";
            var appLink = "";
            var pageSlug = "";

            elements.Take(1);
            foreach (var element in elements)
            {
                var isPromotion = element.SelectToken("promotions");
                var images = element.SelectToken("keyImages");
                var offerMappings = element.SelectToken("offerMappings");
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
                            if (offerMappings.HasValues)
                            {
                                var offerMappingss = offerMappings[0].SelectToken("pageSlug");
                                pageSlug = offerMappingss.ToString();
                            }
                            if (images.HasValues)
                            {
                                var imagess = images[0].SelectToken("url");
                                imageURL = imagess.ToString();
                            }
                        }
                    }
                }
            }
            appLink = "https://www.epicfreegames.net/redirect?slug=" + pageSlug;
            gameLink = "https://www.epicgames.com/store/en-US/p/" + pageSlug;
            embed.AddField(title, description, false).WithImageUrl(imageURL).WithUrl(gameLink);
            var builder = new ComponentBuilder().WithButton("View in Browser", null, ButtonStyle.Link, null, gameLink);
            builder.WithButton("View in Launcher", null, ButtonStyle.Link, null, appLink);
            await RespondAsync(embed: embed.Build(), components: builder.Build());
        }

        // Epic - View upcoming free game
        [SlashCommand("upcoming-games", "Use this to see the upcoming free epic games.")]
        public async Task NextEpicGames()
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = "UPCOMING FREE GAMES"
            };
            embed
            .WithFooter(x =>
            {
                x.Text = $"Requested By {Context.User.Username}";
                x.IconUrl = Context.User.GetAvatarUrl();
            })
            .WithTimestamp(DateTimeOffset.Now);

            string jsonString = new WebClient().DownloadString("https://store-site-backend-static.ak.epicgames.com/freeGamesPromotions");
            JToken token = JToken.Parse(jsonString);
            var elements = token.SelectToken("data").SelectToken("Catalog").SelectToken("searchStore").SelectToken("elements");
            var imageURL = "";
            string title = "Title";
            string description = "Description";
            var gameLink = "";
            var appLink = "";
            var pageSlug = "";

            elements.Take(1);
            foreach (var element in elements)
            {
                var isPromotion = element.SelectToken("promotions");
                var images = element.SelectToken("keyImages");
                var offerMappings = element.SelectToken("offerMappings");

                if (isPromotion.HasValues)
                {
                    var offers = isPromotion.SelectToken("upcomingPromotionalOffers");
                    if (offers.HasValues)
                    {
                        var offerss = offers[0].SelectToken("promotionalOffers");
                        if (offerss.HasValues)
                        {
                            title = (string)element.SelectToken("title");
                            description = (string)element.SelectToken("description");
                            if (offerMappings.HasValues)
                            {
                                var offerMappingss = offerMappings[0].SelectToken("pageSlug");
                                pageSlug = offerMappingss.ToString();
                            }
                            if (images.HasValues)
                            {
                                var imagess = images[0].SelectToken("url");
                                imageURL = imagess.ToString();
                            }
                        }
                    }
                }
            }
            appLink = "https://www.epicfreegames.net/redirect?slug=" + pageSlug;
            gameLink = "https://www.epicgames.com/store/en-US/p/" + pageSlug;
            embed.AddField(title, description, false).WithImageUrl(imageURL).WithUrl(gameLink);
            var builder = new ComponentBuilder().WithButton("View in Browser", null, ButtonStyle.Link, null, gameLink);
            builder.WithButton("View in Launcher", null, ButtonStyle.Link, null, appLink);
            await RespondAsync(embed: embed.Build(), components: builder.Build());
        }

    }
}