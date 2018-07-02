using System;
using System.Collections.Generic;
using System.Text;


    public class DetectEventArgs :EventArgs
    {
        private CloudCoin detectedCoin;

        public DetectEventArgs(CloudCoin coin)
        {
            this.detectedCoin = coin;

        }

        public CloudCoin DetectedCoin {
            get { return detectedCoin; }
        }


    }

