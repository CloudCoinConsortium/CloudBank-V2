using Newtonsoft.Json;

    public class FoundersStack
    {
        [JsonProperty("cloudcoin")]
        public FoundersCloudCoin[] cc { get; set; }
    }
