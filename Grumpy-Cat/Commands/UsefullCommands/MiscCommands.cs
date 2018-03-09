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
    public class MiscCommands : ModuleBase<SocketCommandContext>
    {
        public ulong[] admins = { /*Brammys*/308707063993860116 };
        private Random rnd;

        [Command("Game")]
        public async Task GameRole1(string word1)
        {

            string GameNames = "";
            var user = Context.Guild.GetUser(Context.User.Id);
            var roleName = word1;
            var role = user.Guild.Roles.Where(has => has.Name.ToUpper() == roleName.ToUpper());
            var test = Context.Guild.Roles.ToArray();
            for (int i = 0; i < Datastorage.GetPairsCount(); i++)
            {
                string GameName = Datastorage.GetFormattedAlert($"Role{i}{Context.Guild.Id}");
                GameNames += GameName + "\n";
            }

            if (GameNames.Contains(roleName.ToUpper()))
            {
                await user.AddRolesAsync(role);
                await Context.Channel.SendMessageAsync($"Game: {roleName} added to your account :thumbsup: ");
                Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $"Role: {roleName.ToUpper()} added to {user} || Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($" :no_entry: Game: {roleName} is not added yet use `?gamehelp` for the list of games :no_entry:");
            }
        }

        [Command("AddGame")]
        public async Task AddGameRole(string RoleName)
        {
            var GuildUser = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);
            if (admins.Contains(GuildUser.Id))
            {
                var user = Context.Guild.GetUser(Context.User.Id);
                var embed = new EmbedBuilder();
                rnd = new Random();
                embed.WithDescription($"Game: `{RoleName}` added");
                embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                await Context.Guild.CreateRoleAsync(RoleName.Replace("|", " "), null, Color.Default, false, null);
                await Context.Channel.SendMessageAsync("", false, embed);

                //Adding game to data
                Datastorage.AddPairToStorage("Role" + Datastorage.GetPairsCount() + Context.Guild.Id, RoleName.ToUpper());
            }
            else
            {
                //error message
                await Context.Channel.SendMessageAsync(":no_entry: You shall not use this command :no_entry: ");
                Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} tried to use ?AddGame ");
            }
        }

        [Command("RemoveGame")]
        public async Task RemoveGame(string word1)
        {
            var roleName = word1;
            var user = Context.Guild.GetUser(Context.User.Id);
            var role = user.Guild.Roles.Where(has => has.Name.ToUpper() == roleName.ToUpper());
            string GameNames = "";
            for (int i = 0; i < Datastorage.GetPairsCount(); i++)
            {
                string GameName = Datastorage.GetFormattedAlert($"Role{i}{Context.Guild.Id}");
                GameNames += GameName + "\n";
            }

            if (GameNames.Contains(roleName.ToUpper()))
            {
                await user.RemoveRolesAsync(role);
                await Context.Channel.SendMessageAsync($"Game: {roleName} removed from your account :thumbsup:");
                Console.WriteLine(String.Format("{0:G}") + $"Role: {roleName.ToUpper()} removed from {user} || Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User}");
            }
            else
            {
                await Context.Channel.SendMessageAsync($" :no_entry: Game: {roleName} doesn't exist. Use `?gamehelp` for the list of games :no_entry:");
            }
        }

        [Command("level")]
        public async Task ShowLevel()
        {
            rnd = new Random();
            var embed = new EmbedBuilder();
            var account = UserAccounts.GetAccount(Context.User);

            embed.WithTitle("Info for " + Context.User.Username);
            embed.WithDescription(":arrow_double_down:       :arrow_double_down: ");
            embed.AddInlineField("Level", account.Level);
            embed.AddInlineField("XP", account.XP);
            embed.AddInlineField("Total time conntected (In minutes)", account.TotalTimeConntected);
            embed.WithColor(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            await Context.Channel.SendMessageAsync("", false, embed);
            Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" : Server: {Context.Guild} || Channel: {Context.Channel} || User: {Context.User} || Used: ?level");
        }
    }
}
