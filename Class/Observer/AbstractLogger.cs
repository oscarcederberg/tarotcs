using System.Collections.Generic;

namespace tarot.Observer{
    abstract class AbstractLogger<T> : ILogger<T>{
        protected List<Log> _logs;
        protected AbstractLogger(Log log = null){
            _logs = new List<Log>();
            if (log is not null) _logs.Add(log);
        }

        abstract public void Notify(T value);

        virtual public void AddLog(Log log){
            _logs.Add(log);
        }
    }
}
