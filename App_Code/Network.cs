using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


    public class Network
    {
        [JsonProperty]
        public int nn { get; set; }
        [JsonProperty]
        public RAIDANode[] raida { get; set; }
    }
    public class RAIDANode
    {
        [JsonProperty]
        public int raida_index { get; set; }
        [JsonProperty]
        public bool failsEcho { get; set; }
        [JsonProperty]
        public bool failsDetect { get; set; }
        [JsonProperty]
        public bool failsFix { get; set; }
        [JsonProperty]
        public bool failsTicket { get; set; }
        [JsonProperty]
        public string location { get; set; }
        [JsonProperty]
        public NodeURL[] urls { get; set; }
    }
    public class RAIDADirectory
    {
        [JsonProperty]
        public Network[] networks { get; set; }
    }
    public class directory
    {
        [JsonProperty]
        public Network[] networks { get; set; }
    }
    public class NodeURL
    {
        [JsonProperty]
        public string url { get; set; }
        [JsonProperty]
        public int? port { get; set; }
        [JsonProperty]
        public int? milliseconds { get; set; }

    }

