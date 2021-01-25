using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Leaf.xNet;
using Newtonsoft.Json;

namespace SharpDTI
{
    class Billing
    { 

        public static dynamic y = null;

        public static void Info(string token)
        {
            var request = new HttpRequest();
            request.AddHeader("Authorization", token);
            request.AddHeader("Content-Type", "application/json");
            var billRes = request.Get("https://discordapp.com/api/v6/users/@me/billing/payment-sources");
            string billString = billRes.ToString();
            dynamic billData = JsonConvert.DeserializeObject(billString);
            foreach (dynamic x in billData)
            {
                y = x.billing_address;
                //Console.WriteLine(y);
                Console.WriteLine($"Name:                {y.name}");
                Console.WriteLine($"Address 1:           {y.line_1}");
                if(y.line_2 != null) Console.WriteLine($"Address 1:           {y.line_2}");
                Console.WriteLine($"City:                {y.city}");
                Console.WriteLine($"Postal Code:         {y.postal_code}");
                Console.WriteLine($"State:               {y.state}");
                Console.WriteLine($"Country:             {y.country}");
            }
        }

        public static void Payment(string token)
        {
            var request = new HttpRequest();
            request.AddHeader("Authorization", token);
            request.AddHeader("Content-Type", "application/json");
            var billRes = request.Get("https://discordapp.com/api/v6/users/@me/billing/payment-sources");
            string billString = billRes.ToString();
            dynamic billData = JsonConvert.DeserializeObject(billString);
            foreach (dynamic x in billData)
            {
                string value = "";
                if (x.invalid == "False") value = "True";
                else if (x.invalid == "True") value = "False";
                if (x.type == 1)
                {
                    Console.WriteLine($"Payment Type:        Credit Card");
                    Console.WriteLine($"Valid:               {value}");
                    Console.WriteLine($"Brand:               {x.brand}");
                    Console.WriteLine($"CC Num:              ************{x.last_4}");
                    Console.WriteLine($"Exp Date:            0{x.expires_month}/{x.expires_year}");
                    Console.WriteLine($"Default Method:      {x["default"]}");
                }
                else if (x.type == 2)
                {
                    Console.WriteLine($"Payment Type:        PayPal");
                    Console.WriteLine($"Valid:               {value}");
                    Console.WriteLine($"PayPal Email:        {x.email}");
                    Console.WriteLine($"Default Method:      {x["default"]}");
                }
            }
        }
    }
}
