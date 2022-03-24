using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DiscordNetBot.Modules
{
    public class Slash1HelpModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private CommandHandler _handler;

        public Slash1HelpModule(CommandHandler handler)
        {
            _handler = handler;
        }

        // Help - View all available slash commands
        [SlashCommand("help", "Use this if you need help.")]
        public async Task HelpMe()
        {
            string prefix = "/";
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            foreach (var module in Commands.Modules)
            {
                string name = module.Name;

                if (name == "Slash1HelpModule")
                {
                    name = "💜 HELPFUL COMMANDS \n ---------------------------------------------";
                }
                else if (name == "Slash2FunModule")
                {
                    name = "\n \n \n 💜 FUN COMMANDS \n ---------------------------------------------";
                }
                string description = null;
                foreach (var cmd in module.SlashCommands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context, null);
                    if (result.IsSuccess)
                        description += $"{prefix}{cmd.Name}\n {cmd.Description} \n \n ";
                }
                if (!string.IsNullOrWhiteSpace(description))
                {
                    embed.AddField(x =>
                    {
                        x.Name = name;
                        x.Value = description;
                        x.IsInline = true;
                    });
                }
            }
            await RespondAsync(embed: embed.Build());
        }

        // Server - View server info
        [SlashCommand("ping", "Use this to see server info.")]
        public async Task Server()
        {
            var guildName = Context.Guild.Name;
            var userCount = Context.Guild.Users.Count.ToString();
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218)
            };
            embed
            .AddField("💜 MEMBERS \n----------------------------", $" *{userCount} user(s)*", true)
            .AddField("💜 ROLES \n----------------------------", $"*{Context.Guild.Roles.Count} role(s)*", true)
            .AddField("💜 CHANNELS \n----------------------------", $"*{Context.Guild.TextChannels.Count} text channel(s) \n " + Context.Guild.VoiceChannels.Count + "voice channel(s)*", true)
            .WithThumbnailUrl(Context.Guild.IconUrl)
            .WithFooter(x =>
            {
                x.Text = $"Requested By {Context.User.Username}";
                x.IconUrl = Context.User.GetAvatarUrl();
            })
            .WithTimestamp(DateTimeOffset.Now);

            await RespondAsync(embed: embed.Build());
        }

        // User - View user info
        [SlashCommand("user", "Use this to view user info.")]
        public async Task UserInfoAsync(SocketGuildUser user = null)
        {
            user = (SocketGuildUser)(user ?? Context.User);
            if (user.IsBot)
            {
                await ReplyAsync("Bots are not people :D");
                return;
            }
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                ThumbnailUrl = user.GetAvatarUrl()
            };
            embed
            .AddField("Member ID", user.Id, true)
            .AddField("Status", user.Status, true)
            .AddField("Joined", user.JoinedAt.Value.ToString("dd MMMM, yyyy"), true)
            .AddField("Account Created", user.CreatedAt.ToString("dd MMMM, yyyy"), true)
            .AddField("Roles", user.Roles.Count - 1, true)
            .WithTimestamp(DateTimeOffset.Now)
            .WithAuthor(x =>
            {
                x.Name = user.Username;
                x.IconUrl = user.GetAvatarUrl();
            })
            .WithFooter(x =>
            {
                x.Text = $"Requested By {Context.User.Username}";
                x.IconUrl = Context.User.GetAvatarUrl();
            });

            await RespondAsync(embed: embed.Build());
        }

        // Send to admin - send message in admin channel
        //
        // !! IMPORTANT - Set the channelID in order to send a message to an admin channel
        //
        [SlashCommand("send-to-admin", "Sends an anonymous message to the moderators, which will not show on the server.")]
        public async Task Send([MaxLength(1250)] string message)
        {
            var userInfo = Context.User;
            var embed = new EmbedBuilder()
            {
                Title = "SUPPORT REQUEST",
                Color = new Color(114, 137, 218)
            };
            var client = Context.Client;
            ulong channelID = 0000000000000; // CHANNEL ID HERE 
            var channel = client.GetChannel(channelID) as SocketTextChannel;
            embed.AddField("Message: ", message, false);
            embed.WithFooter(x =>
            {
                x.Text = userInfo.Username;
                x.IconUrl = userInfo.GetAvatarUrl();
            });
            await channel.SendMessageAsync(embed: embed.Build());
            await RespondAsync("Thanks! Your message has been posted to the moderators.");
        }

        // Invite - display bot invite link
        //
        // !! IMPORTANT - Set the 'BOT_ID' to your own bot in order to send an invite link
        //
        [SlashCommand("invite-link", "Use this to get my invite link!")]
        public async Task InviteLink()
        {
            var embed = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Title = "Invite this bot to a server",
                Description = " "
            };
            embed
            .AddField("URL: ",
            "[https://discord.com/oauth2/authorize?client_id=BOT_ID&permissions=515396521024&scope=bot%20applications.commands](https://discord.com/oauth2/authorize?client_id=BOT_ID&permissions=515396521024&scope=bot%20applications.commands)")
            .WithImageUrl("https://media1.tenor.com/images/8be041fe538a0f292bb85885768341a7/tenor.gif?itemid=5261112")
            .WithUrl("https://discord.com/oauth2/authorize?client_id=BOT_ID&permissions=515396521024&scope=bot%20applications.commands")
            .WithFooter(x =>
            {
                x.Text = $"Requested By {Context.User.Username}";
                x.IconUrl = Context.User.GetAvatarUrl();
            })
            .WithThumbnailUrl(Context.Guild.IconUrl)
            .WithCurrentTimestamp();

            await RespondAsync(embed: embed.Build());
        }
    }
}