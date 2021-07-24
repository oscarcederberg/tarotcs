using System;
using tarot.Observer;

namespace tarot.Menu{
    public class MenuEntryFunc<T> : AbstractMenuEntry<T>{
        private Func<T> _func;

        public MenuEntryFunc(string text, Func<T> func, Observer.IObserver<T> observer = null) : base(text, observer){
            this._func = func;
        }

        public override string ToString(){
            return _text;
        }

        public override void Select(){
            T value = _func.Invoke();
            NotifyObservers(value);
        }
    }
}
