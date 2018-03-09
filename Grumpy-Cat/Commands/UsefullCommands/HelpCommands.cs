using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Grumpy_Cat.UserAccount;

namespace Grumpy_Cat.Commands.UsefullCommands
{
    public class HelpCommands : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;

        [Command("GameHelp")]
        public async Task GameRoleHelp()
        {
            var embed = new EmbedBuilder();
            string GameNames = "";
            rnd = new Random();
            string HelpMsg1 = Utilities.GetFormattedAlert("GameRoleHelp1");
            string HelpMsg2 = Utilities.GetFormattedAlert("GameRoleHelp2");
            string HelpMsg3 = Utilities.GetFormattedAlert("GameRoleHelp3");
            string RemoveGame = Utilities.GetFormattedAlert("RemoveGame");

            embed.WithTitle($"Info for {Context.User.Username}");
            embed.WithDescription(HelpMsg1 + HelpMsg2);
            embed.WithThumbnailUrl("https://i.gyazo.com/f29f36719c4aa76076aa8d764d35b96d.jpg");
            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
            embed.WithCurrentTimestamp();
            for (int i = 0; i < Datastorage.GetPairsCount(); i++)
            {
                string GameName = Datastorage.GetFormattedAlert($"Role{i}{Context.Guild.Id}");
                GameNames += GameName + "\n";
            }
            embed.AddField("Available games:", GameNames);
            embed.AddField("My game is not in the list.", HelpMsg3);
            embed.AddField("How do i remove a game that i added?", RemoveGame);
            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $"Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User}");
        }

        [Command("help")]
        public async Task help()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Info for " + Context.User.Username);
            embed.WithDescription(":arrow_double_down:       :arrow_double_down: ");
            embed.AddField("Commands:", "`?level\n?gameHelp`");

            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: ?Help");

        }
    }
}
