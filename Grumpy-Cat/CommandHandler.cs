using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Grumpy_Cat.UserAccount;

namespace Grumpy_Cat
{
    public class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
            _client.Connected += SetGame;
            _client.UserJoined += userJoined;
            _client.UserLeft += userLeft;
            _client.MessageReceived += userSendMessage;
            _client.UserVoiceStateUpdated += UserJoinedOrLeftChannel;
        }

        public async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    if (result.ToString() == "BadArgCount: The input text has too many parameters.")
                    {
                        await context.Channel.SendMessageAsync(":no_entry_sign: The input has to many parameters");
                    }
                    else if (result.ToString() == "BadArgCount: The input text has too few parameters.")
                    {
                        await context.Channel.SendMessageAsync(":no_entry_sign: The input text has too few parameters.");
                    }
                    else
                    {
                        await context.Channel.SendMessageAsync(result.ToString());
                    }
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }

        public async Task SetGame()
        {
            await (_client as DiscordSocketClient).SetGameAsync("?help || ?gameHelp");
        }

        public async Task userJoined(SocketGuildUser user)
        {
            //dming welcome message to joined user
            var embed = new EmbedBuilder();
            string msg1 = Utilities.GetFormattedAlert("WelcomMsg1");
            string msg11 = Utilities.GetFormattedAlert("?Game GameName");
            string msg2 = Utilities.GetFormattedAlert("#looking-for-game");
            string msg3 = Utilities.GetFormattedAlert("#newphonewhodis");
            string msg4 = Utilities.GetFormattedAlert("#general-chat");
            string msg5 = Utilities.GetFormattedAlert("#rainbow6siege,#overwatch,and #skill-over-luck");
            string msg6 = Utilities.GetFormattedAlert("#cute-animals");
            string msg7 = Utilities.GetFormattedAlert("#the-salt-is-real");
            string msg8 = Utilities.GetFormattedAlert("#suggestions");
            string msg9 = Utilities.GetFormattedAlert("Women also have their own space and voicechannel");
            string msg10 = Utilities.GetFormattedAlert("EndMsg");

            embed.AddField("Welcome", msg1);
            embed.AddField("?Game GameName", msg11);
            embed.AddField("#looking-for-game", msg2);
            embed.AddField("#newphonewhodis", msg3);
            embed.AddField("#general-chat", msg4);
            embed.AddField("#rainbow6siege , #overwatch , and #skill-over-luck", msg5);
            embed.AddField("#cute-animals", msg6);
            embed.AddField("#the-salt-is-real", msg7);
            embed.AddField("#suggestions", msg8);
            embed.AddField("Women also have their own space and voicechannel.", msg9);
            embed.AddField("Anyway this should cover it", msg10);
            embed.WithThumbnailUrl("https://i.gyazo.com/f29f36719c4aa76076aa8d764d35b96d.jpg%22");
            embed.WithColor(255, 50, 255);
            embed.WithCurrentTimestamp();

            await user.SendMessageAsync("", false, embed);
            Console.WriteLine($"JOIN: User: {user} joined || Date {DateTime.Now}");
        }

        public async Task userLeft(SocketGuildUser user)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription("We are sad to hear that you left our discord server ;(");
            embed.AddField("Regret your decision?", "Use this link to get back into the discord!\nhttps://discord.gg/uepwQwB");
            await user.SendMessageAsync("", false, embed);
            Console.WriteLine($"LEFT: User: {user} joined || Date {DateTime.Now}");
        }

        public async Task userSendMessage(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new SocketCommandContext(_client, msg);
            //adding xp for sending message 
            UserLeveling.AddXpAndCheckLevel(context.User, context.Guild, 3);
        }

        public async Task UserJoinedOrLeftChannel(SocketUser user, SocketVoiceState voiceState1, SocketVoiceState voiceState2)
        {
            var account = UserAccounts.GetAccount(user);
            //adding xp for joining/leaving voice channel

            string voiceState1String = voiceState1.ToString();
            TimeSpan timeDif = DateTime.Now.Subtract(account.TimeConnected);
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : {user} connected to {voiceState2} from {voiceState1}");
            if (voiceState1String != "Unknown" && voiceState1String != "AFK")
            {
                int i = 0;
                double timeDiffMinutes = timeDif.TotalMinutes;
                while (timeDiffMinutes >= 3)
                {
                    UserLeveling.AddXp(user, 1); //adding XP
                    timeDiffMinutes -= 3;
                    i++;
                }
                UserLeveling.TotalTimeConntected(user); //adding total minutes to account
                if (timeDif.TotalMinutes > 3) Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : {user} gained {i * 1} XP by staying in {voiceState1} for {timeDif.TotalMinutes}");
            }
            UserLeveling.LastActivity(user); //setting new LastActivity
        }
    }
}
