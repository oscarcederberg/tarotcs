using System.Collections.Generic;

namespace tarot.Subscription.Logging{
    public class Log{
        private List<LogEntry> _entries;

        public Log(){
            _entries = new List<LogEntry>();
        }

        public void Add(LogEntry entry){
            _entries.Add(entry);
        }

        public LogEntry Get(int i) {
            return _entries[i];
        }

        public int Count(){
            return _entries.Count;
        }
    }
}
