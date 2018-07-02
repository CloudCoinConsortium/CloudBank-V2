using System;
using System.Collections.Generic;
using System.Text;


    public class ProgressChangedEventArgs : EventArgs
    {
        public double MinorProgress;
        public double MajorProgress;
        public String MinorProgressMessage;
        public String MajorProgressMessage;
        public ProgressChangedEventArgs()
        {

        }
    }

