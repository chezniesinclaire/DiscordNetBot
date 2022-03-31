# DiscordNetBot
A Discord Bot with basic slash commands and restful APIs using .NET Core 3.1 and Discord.NET 3.3.2.

Includes basic slash commands that provide:
* Slash command help / info, server and user info, invite link info
* The ability to send a help message to a specific channel (e.g. can be used to send a help message to an admin channel)
* The ability to post an automatic message to a specific channel (set to check every 55 mins - if day is Thursday and at 6pm UK time, it will send an automatic response advising the user of which Epic Game is available to download this week)
* Other nonsense / fun commands that get data from APIs / json responses (kanye.rest and epic games), as well as an example Chuck Norris quote that gets set data from a class

# Initial Setup:
* After you've created your bot in the Discord Developer Portal:
  * Add your BOT TOKEN and CHANNEL ID into the appsettings.json file


  * Replace 'BOT_ID' with your BOT ID in the following link to invite the bot to your server: 
  https://discord.com/oauth2/authorize?client_id=BOT_ID&permissions=515396521024&scope=bot%20applications.commands
  
  * 'Slash1HelpModule.cs' - there are some commands in here ("send-to-admin" and "invite-link") that will require your CHANNEL ID or BOT ID in order to work.
  * 'CommandHandler.cs' - there is a method here called 'SendAnnouncement' that is scheduled to post an automatic response to a channel on a specific day at a specific hour. To get this to work, add your CHANNEL ID into the 'channel' ulong variable. Also ensure that conditional days of week / times are set correctly within the 'InitializeAsync' task. 
 


