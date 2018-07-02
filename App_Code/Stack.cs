using Newtonsoft.Json;
using System.Collections.Generic;


    public class Stack
    {
        public Stack()
        {

        }
        public Stack(CloudCoin coin)
        {
            cc = new CloudCoin[1];
            cc[0] = coin;
        }
        public Stack(List<CloudCoin> coins)
        {
            cc = coins.ToArray();
        }
        public Stack(CloudCoin[] coins)
        {
            cc = coins;
        }
        [JsonProperty("cloudcoin")]
        public CloudCoin[] cc { get; set; }
    }
