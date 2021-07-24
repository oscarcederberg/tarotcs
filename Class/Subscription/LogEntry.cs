using System;

namespace tarot.Subscription{
    public class LogEntry{
        public DateTime Time;
        public string Message;

        public LogEntry(string message){
            this.Time = DateTime.Now;
            this.Message = message;
        }

        override public string ToString(){
            return Time.ToLongTimeString() + " - " + Message;
        }
    }
}
