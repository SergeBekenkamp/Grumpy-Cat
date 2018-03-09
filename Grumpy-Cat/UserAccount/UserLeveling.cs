using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Discord;
using Discord.WebSocket;
using Grumpy_Cat.UserAccount;

namespace Grumpy_Cat.UserAccount
{
    public class UserLeveling
    {
        public static async void AddXpAndCheckLevel(SocketUser user, SocketGuild guild, uint xp)
        {
            var account = UserAccounts.GetAccount(user);
            string userString = user.ToString();
            if (userString != "Mord#1715" || userString != "Grumpy cat#6522")
            {
                if (account.XP >= 100)
                {
                    while (account.XP >= 100)
                    {

                        account.XP -= 100;
                        account.Level += 1;
                    }
                    try
                    {
                        await guild.DefaultChannel.SendMessageAsync($":tada: {user.Mention} is now level {account.Level} use: `?level` to see more :tada:");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(String.Format("{0:G}", DateTime.Now) + $" *********** couldn't send a message to the default channel ***********");
                    }
                }
                AddXp(user, xp);
            }
        }

        public static async void AddXp(SocketUser user, uint xp)
        {
            var account = UserAccounts.GetAccount(user);
            account.XP += xp;
            UserAccounts.SaveAccounts();
        }

        public static void LastActivity(SocketUser user)
        {
            var account = UserAccounts.GetAccount(user);
            account.TimeConnected = DateTime.Now;
            UserAccounts.SaveAccounts();
        }

        public static void TotalTimeConntected(SocketUser user)
        {
            var account = UserAccounts.GetAccount(user);
            TimeSpan timeDif = DateTime.Now.Subtract(account.TimeConnected);
            account.TotalTimeConntected += ulong.Parse(Math.Round(timeDif.TotalMinutes, 0, MidpointRounding.ToEven).ToString());
            UserAccounts.SaveAccounts();
        }
    }
}
