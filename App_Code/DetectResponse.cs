using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


    public class DetectResponse
    {
        [JsonProperty("server")]
        public string server;
        [JsonProperty("status")]
        public string status;
        [JsonProperty("message")]
        public string Message;
        [JsonProperty("time")]
        public string time;
        [JsonProperty("version")]
        public string version;

    }
