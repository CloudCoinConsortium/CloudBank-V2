using System;
using System.Collections.Generic;
using System.Text;


    public class MultiDetectRequest
    {
        public int[] nn ;
        public int[] sn;
        public String[][] an = new String[Config.NodeCount][];
        public String[][] pan = new String[Config.NodeCount][];
        public int[] d;
        public int timeout;
    }

