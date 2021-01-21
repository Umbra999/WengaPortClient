using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VRC;
using WengaPort.Modules;

namespace WengaPort.Extensions.APIExtension
{
    class ApiBot
    {
        public static void BotFriendSpam(Player player)
        {

            ApiBot ApiBot = new ApiBot();
            ApiBot.FriendSpam(player);
        }

        public async Task FriendSpam(Player p)
        {
            string[] tokens = File.ReadAllLines("WengaPort\\Photon\\AuthTokens.txt");
            if (tokens.Length == 0)
            {
                Logger.WengaLogger("No Tokens found");
            }  
            else
            {
                Logger.WengaLogger($"Adding {p.DisplayName()} with {tokens.Length} API Bots");
                for (int i = 0, l = tokens.Length; i < l; i++)
                {
                    var token = tokens[i].Trim();
                    var handler = new HttpClientHandler
                    {
                        UseCookies = false
                    };
                    using var httpClient = new HttpClient(handler);
                    using var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://api.vrchat.cloud/api/1/user/{p.UserID()}/friendRequest?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26");
                    request.Headers.TryAddWithoutValidation("Cookie", "auth=" + token);
                    var response = await httpClient.SendAsync(request);

                    string content = await response.Content.ReadAsStringAsync();
                    if (!content.Contains("Users are already Friends") && content.Contains("error") && !content.Contains("This user has already been sent a friend request"))
                    {
                        Logger.WengaLogger(content);
                    }
                    await Task.Delay(3500);
                }
                Logger.WengaLogger($"Friendspam done");
            }
        }
    }
}
