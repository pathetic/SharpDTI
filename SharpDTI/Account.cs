using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpDTI
{
    class Account
    {
        static int c = 1;
        static int f = 1;
        static int og = 1;
        static int allg = 0;
        public static void Connections(string token)
        {
            var request = new HttpRequest();
            request.AddHeader("Authorization", token);
            request.AddHeader("Content-Type", "application/json");
            var connRes = request.Get("https://discordapp.com/api/v6/users/@me/connections");
            string connString = connRes.ToString();
            dynamic connData = JsonConvert.DeserializeObject(connString);
            foreach (dynamic x in connData)
            {
                if(c<10)    Console.WriteLine($"Connection {c}:        {x.type} -> {x.name}");
                else        Console.WriteLine($"Connection {c}:       {x.type} -> {x.name}");
                c++;
            }
        }

        public static void Friends(string token)
        {
            var request = new HttpRequest();
            request.AddHeader("Authorization", token);
            request.AddHeader("Content-Type", "application/json");
            var friendsRes = request.Get("https://discord.com/api/v8/users/@me/relationships");
            string friendsString = friendsRes.ToString();
            dynamic friendsData = JsonConvert.DeserializeObject(friendsString);
            //List<Acc> friendsObj = JsonConvert.DeserializeObject<List<Acc>>(friendsString);
            foreach (dynamic x in friendsData)
            {
                f++;
            }
            Console.WriteLine($"Total Friends:       {f}");
        }


        public class User
        {
            [JsonProperty("id")]
            public string id { get; set; }

            [JsonProperty("username")]
            public string username { get; set; }

            [JsonProperty("avatar")]
            public string avatar { get; set; }

            [JsonProperty("discriminator")]
            public string discriminator { get; set; }

            [JsonProperty("public_flags")]
            public int public_flags { get; set; }
        }

        public class Acc
        {
            [JsonProperty("id")]
            public string id { get; set; }

            [JsonProperty("type")]
            public int type { get; set; }

            [JsonProperty("nickname")]
            public string nickname { get; set; }

            [JsonProperty("user")]
            public List<User> user { get; set; }
        }

        public static void OwnedGuilds(string token)
        {
            var request = new HttpRequest();
            request.AddHeader("Authorization", token);
            request.AddHeader("Content-Type", "application/json");
            string oguildResponse = request.Get("https://discordapp.com/api/v6/users/@me/guilds").ToString();
            //Console.WriteLine(getResponse);
            var data = JsonConvert.DeserializeObject<List<UserPermission>>(oguildResponse);
            foreach (var item in data)
            {
                allg++;
                if (item.Owner)
                {
                    var guildrequest = new HttpRequest();
                    guildrequest.AddHeader("Authorization", token);
                    guildrequest.AddHeader("Content-Type", "application/json");
                    string getGuild = guildrequest.Get($"https://discordapp.com/api/v6/guilds/{item.Id}?with_counts=true").ToString();
                    dynamic guilddata = JsonConvert.DeserializeObject(getGuild);
                    if(og<10) Console.WriteLine($"Owned Guild {og}:       {item.Name} -> Members: {guilddata.approximate_member_count}");
                    else      Console.WriteLine($"Owned Guild {og}:      {item.Name} -> Members: {guilddata.approximate_member_count}");
                    og++;
                }
            }
            Console.WriteLine($"Total Guilds:        {allg}");
        }

        public class UserPermission
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("owner")]
            public bool Owner { get; set; }

            [JsonProperty("permissions")]
            public long Permissions { get; set; }

            [JsonProperty("features")]
            public List<string> Features { get; set; }

            [JsonProperty("permissions_new")]
            public long PermissionsNew { get; set; }
        }

    }
}
