using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarot
{
    public interface IObserver<T>
    {
        public void Notify(T value);
    }
}
