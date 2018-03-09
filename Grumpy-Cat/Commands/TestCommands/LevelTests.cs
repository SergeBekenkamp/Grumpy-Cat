using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Grumpy_Cat.UserAccount;

namespace Grumpy_Cat.Commands.TestCommands
{
    public class LevelTests : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = {/*brammys*/308707063993860116 };
        private Random rnd;

        [Command("addXp")]
        public async Task addXp(uint xp)
        {

            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);

            if (admins.Contains(GuildUser.Id))
            {
                var embed = new EmbedBuilder();
                rnd = new Random();
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                UserLeveling.AddXpAndCheckLevel(Context.User, Context.Guild, xp);

                await Context.Channel.SendMessageAsync($"Added {xp} xp", false, embed);
                Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: ?AddXp");
            }
        }

        [Command("allUsers")]
        public async Task allUsers()
        {
            UserAccount.UserAccount useraccount = new UserAccount.UserAccount();
            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);

            if (admins.Contains(GuildUser.Id))
            {
                Console.WriteLine(Datastorage.LoadUserAccounts("Data/UserData/accounts.json"));
                foreach(object User in useraccount.ToString())
                {
                    var Id = useraccount.ID;
                    var Level = useraccount.Level;
                    var TimeConnected = useraccount.TimeConnected;
                    Console.WriteLine($"{Id}, {Level}, {TimeConnected}");
                }
            }
        }
    }
}
