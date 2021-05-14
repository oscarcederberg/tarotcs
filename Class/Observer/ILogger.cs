using System;
using System.Collections.Generic;
using System.Text;

namespace tarot
{
    public interface ILogger<T> : IObserver<T>
    {
        public void AddLog(Log log);
    }
}
