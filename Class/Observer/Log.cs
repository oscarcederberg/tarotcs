using System;
using System.Collections.Generic;
using System.Text;

namespace tarot
{
    public class Log
    {
        private List<LogEntry> _entries;

        public Log()
        {
            _entries = new List<LogEntry>();
        }

        public void Add(LogEntry entry)
        {
            _entries.Add(entry);
        }

        public LogEntry Get(int i) 
        {
            return _entries[i];
        }

        public int Count()
        {
            return _entries.Count;
        }
    }
}
