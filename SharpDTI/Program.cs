using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Leaf.xNet;
using Newtonsoft.Json;

namespace SharpDTI
{
    class Program
    {
        static string resp = "";
        static bool worked = false;
        static bool nitro = false;
        static void Main(string[] args)
        {
            Console.Title = "SharpDTI";
            Logo();
            Title("\nInput Token: ");
            string token = Console.ReadLine();
            resp = BasicInfo(token);
            CheckStatus();
            CheckNitro(token);

            Title("\nBilling Information:\n");
            Billing.Info(token);

            Title("\nPayment Method(s):\n");
            Billing.Payment(token);

            Title("\nAdvanced Information:\n");
            Account.Connections(token);
            Account.Friends(token);
            Account.OwnedGuilds(token);

            Title("\nChecked token succesfuly!\n");
            Console.ReadKey();

        }

        static string BasicInfo(string token)
        {
            string returnValue = null;
            try
            {
                var request = new HttpRequest();
                request.AddHeader("Authorization", token);
                request.AddHeader("Content-Type", "application/json");
                var basicInfoRes = request.Get("https://discordapp.com/api/v6/users/@me");
                string basicInfoString = basicInfoRes.ToString();
                dynamic basicData = JsonConvert.DeserializeObject(basicInfoString);
                double unix = (((long)Convert.ToDouble(basicData.id) >> 22) + 1420070400000) / 1000;

                    Title("\nUser information:\n");
                    Console.WriteLine($"Username:            {basicData.username}#{basicData.discriminator}");
                    Console.WriteLine($"ID:                  {basicData.id}");
                    Console.WriteLine($"Avatar:              https://cdn.discordapp.com/avatars/{basicData.id}/{basicData.avatar}.gif");
                    Console.WriteLine($"Email:               {basicData.email}");
                    Console.WriteLine($"Phone:               {basicData.phone}");
                    Console.WriteLine($"2FA/MFA:             {basicData.mfa_enabled}");
                    Console.WriteLine($"Flags:               {basicData.public_flags}");
                    Console.WriteLine($"Locale:              {basicData.locale}");
                    Console.WriteLine($"Verified email:      {basicData.verified}");
                    Console.WriteLine($"Creation date:       {UnixTimeStampToDateTime(unix)}");

                returnValue = "";
                worked = true;
            }
            catch (HttpException ex)
            {
                worked = false;
                returnValue = $"HTTP Error: {ex.Message}";
            }
            return returnValue;
        }

        public static void CheckNitro (string token)
        {
            var request = new HttpRequest();
            request.AddHeader("Authorization", token);
            request.AddHeader("Content-Type", "application/json");
            var nitroRes = request.Get("https://discordapp.com/api/v6/users/@me/billing/subscriptions");
            string nitroString = nitroRes.ToString();
            dynamic nitroData = JsonConvert.DeserializeObject(nitroString);
            if (nitroString != "[]") nitro = true;
            Console.WriteLine($"Nitro status:        {nitro}");
            if (nitro)
            {
                Console.WriteLine($"Expiration date:     {nitroData[0].current_period_end}");
            }
        }

        public static void CheckStatus()
        {
            if (!worked)
            {
                Console.WriteLine(resp);
                Console.WriteLine("Press any key to close!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static void Title(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Logo()
        {
            Console.WriteLine("▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ █▀▀█ ▒█▀▀▄ ▀▀█▀▀ ▀█▀ ");
            Console.WriteLine("░▀▀▀▄▄ █▀▀█ █▄▄█ █▄▄▀ █░░█ ▒█░▒█ ░▒█░░ ▒█░ ");
            Console.WriteLine("▒█▄▄▄█ ▀░░▀ ▀░░▀ ▀░▀▀ █▀▀▀ ▒█▄▄▀ ░▒█░░ ▄█▄ ");
        }
    }
}
