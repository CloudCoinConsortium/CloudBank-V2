using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


    public class EchoResponse
    {
        public int ReadyCount = 0;
        public int NotReadyCount = 0;
        public int NetworkNumber = 0;

        public NodeEchoResponse[] responses = new NodeEchoResponse[Config.NodeCount];

    }

