using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Grumpy_Cat.UserAccount;

namespace Grumpy_Cat.Commands.BsCommands
{
    public class BsCommands : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = {/*brammys*/308707063993860116 };
        private Random rnd;

        [Command("AllRolesToString")]
        public async Task AllRolesToString()
        {
            string msg = "";
            var embed = new EmbedBuilder();

            var test = Context.Guild.Roles.ToArray();
            for (int i = 0; i < test.Length; i++)
            {
                msg += test[i].ToString() + ",\n";
            }
            embed.WithDescription(msg);
            await Context.Channel.SendMessageAsync("", false, embed);

        }

        [Command("timetest")]
        public async Task timeTest()
        {
            DateTime dt = DateTime.Now;

            await Context.Channel.SendMessageAsync(String.Format("{0:m ss}", dt));
        }

        [Command("DM")]
        public async Task dmUser(SocketGuildUser user)
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

            embed.AddField($"Welcome", msg1);
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
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : User {user} joined");
        }

        [Command("dmLeave")]
        public async Task LeaveDM(SocketGuildUser user)
        {
            var embed = new EmbedBuilder();

            embed.WithDescription("We are sad to hear that you left our discord server ;(");
            embed.AddField("Regret your decision?",
            "Use this link to get back into the discord!\nhttps://discord.gg/uepwQwB");
            await user.SendMessageAsync("", false, embed);
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : {user} left {Context.Guild.Name}");
        }

        [Command("TimeDifTest")]
        public async Task TimeDifTest()
        {
            var account = UserAccounts.GetAccount(Context.User);

            TimeSpan timeDif = DateTime.Now.Subtract(account.TimeConnected);
            await Context.Channel.SendMessageAsync($"{timeDif.TotalMinutes}");
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: ?TimeDifTest");

        }

        [Command("settimedif")]
        public async Task settimedif()
        {
            var account = UserAccounts.GetAccount(Context.User);

            account.TimeConnected = DateTime.Now;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync("time set");
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: ?settimedif");

        }

        [Command("SetGame")]
        [Summary("Sets a 'Game' for the bot")]
        public async Task setgame([Remainder] string game)
        {
            var YourEmoji = new Emoji("🤔");
            await Context.Message.AddReactionAsync(YourEmoji);
            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);
            if (admins.Contains(GuildUser.Id))
            {
                var user = Context.User.Username;
                await (Context.Client as DiscordSocketClient).SetGameAsync(game);
                await Context.Channel.SendMessageAsync($"{user} changed playing to *{game}*");
                Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} used ?setgame ");
            }
            else
            {
                //error message
                await Context.Channel.SendMessageAsync(":no_entry: You shall not use this command :no_entry: ");
                Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} tried to use ?setgame ");
            }
        }
    }
}
