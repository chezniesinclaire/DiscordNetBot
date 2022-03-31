using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordNetBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _comserv;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;
        private static System.Timers.Timer Timer { get; set; }

        public CommandHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services, CommandService comserv)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _comserv = comserv;
        }

        public async Task InitializeAsync()
        {
            // Process the InteractionCreated payloads to execute Interactions commands
            _client.InteractionCreated += HandleInteraction;
         
            // Process the command execution results 
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
            _client.MessageReceived += HandleCommandAsync;

            // Process every 55 mins
            Timer = new(3300000);
            Timer.Elapsed += async (_, _) =>
            {
                if (_client.ConnectionState == ConnectionState.Connected)
                {
                    // If Thursday at 6pm, send announcement about free epic game
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && DateTime.Now.Hour == 18)
                    {
                        await SendAnnouncement();
                    }
                }
            };
            Timer.AutoReset = true;
            Timer.Enabled = true;

            // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _comserv.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), _services);
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e) => throw new NotImplementedException();
        private async Task SendAnnouncement()
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = "FREE THIS WEEK"
            };
            embed
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
            ulong channel = 123456789012345678;
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
            try
            {
                await ((ITextChannel)_client.GetChannel(channel)).SendMessageAsync(embed: embed.Build());
            }
            catch { }
        }
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                // If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
        private async Task HandleCommandAsync(SocketMessage MessageParam)
        {
            var message = MessageParam as SocketUserMessage;
            if (message == null) return;

            int ArgPos = 0;

            // If there's no prefix or the message is from a bot then nothing happens
            if (!(message.HasCharPrefix('!', ref ArgPos) || message.HasMentionPrefix(_client.CurrentUser, ref ArgPos)) || message.Author.IsBot) return;

            var context = new SocketCommandContext(_client, message);

            await _comserv.ExecuteAsync(
                context: context,
                argPos: ArgPos,
                services: null
                );
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        

    }
}
