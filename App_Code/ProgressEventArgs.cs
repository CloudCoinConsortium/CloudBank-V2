using System;


    public class ProgressEventArgs : EventArgs
    {
        public string Status { get; private set; }
        public int percentage { get; private set; }
        public ProgressEventArgs(string status, int percentage = 0)
        {
            Status = status;
            this.percentage = percentage;
        }


    }
