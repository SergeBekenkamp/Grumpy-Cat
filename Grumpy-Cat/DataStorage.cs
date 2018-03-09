using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grumpy_Cat
{
    public class Datastorage
    {
        private static Dictionary<string, string> pairs = new Dictionary<string, string>();


        public static void AddPairToStorage(string key, string value)
        {
            pairs.Add(key, value);
            SaveData();
        }

        public static int GetPairsCount()
        {
            return pairs.Count;
        }
        static Datastorage()
        {
            // load data
            if (!ValidateStorageFile("Data/BotData/GameRoles.json")) return;
            string json = File.ReadAllText("data/BotData/GameRoles.json");
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveData()
        {
            // Save data
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText("data/BotData/gameRoles.json", json);
        }

        private static bool ValidateStorageFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            return true;
        }
        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (pairs.ContainsKey(key))
            {
                return String.Format(pairs[key], parameter);
            }
            return "";
        }

        private static object _lock = new object();
        public static void SaveUserAccounts(IEnumerable<UserAccount.UserAccount> accounts, string filePath)
        {
            lock (_lock)
            {
                string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static IEnumerable<UserAccount.UserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount.UserAccount>>(json);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
