using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;


    public class Utils
    {
        public static CloudCoin[] LoadJson(string filename)
        {
            try
            {
                using (StreamReader r = (File.OpenText(filename)))
                {
                    string json = r.ReadToEnd();
                    Stack coins = JsonConvert.DeserializeObject<Stack>(json);
                    return coins.cc;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static StringBuilder CoinsToCSV(IEnumerable<CloudCoin> coins)
        {
            var csv = new StringBuilder();


            var headerLine = string.Format("sn,denomination,nn,");
            string headeranstring = "";
            for (int i = 0; i < Config.NodeCount; i++)
            {
                headeranstring += "an" + (i + 1) + ",";
            }

            // Write the Header Record
            csv.AppendLine(headerLine + headeranstring);

            // Write the Coin Serial Numbers
            foreach (var coin in coins)
            {
                csv.AppendLine(coin.GetCSV());
            }
            return csv;
        }
        public static string WriteObjectToString()
        {
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            return "";
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /**
        * Method ordinalIndexOf used to parse cloudcoins. Finds the nth number of a character within a string
        *
        * @param str The string to search in
        * @param substr What to count in the string
        * @param n The nth number
        * @return The index of the nth number
        */
        public static int ordinalIndexOf(string str, string substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
            {
                pos = str.IndexOf(substr, (pos + 1));
            }
            return pos;
        }//end ordinal Index of


        public static async Task<String> GetHtmlFromURL(String urlAddress)
        {
            
            string data = "";
            try
            {
                using (var cli = new HttpClient())
                {
                    HttpResponseMessage response = await cli.GetAsync(urlAddress);
                    if (response.IsSuccessStatusCode)
                        data = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(data);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return data;
        }//end get HTML

    }

