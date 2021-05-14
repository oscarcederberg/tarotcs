using System;
using System.Collections.Generic;
using System.Text;

namespace tarot
{
    public interface IMenuEntry
    {
        public string ToString();
        public void Select();
    }
}
