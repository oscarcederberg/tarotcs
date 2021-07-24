using System;
using tarot.Subscription;

namespace tarot.Menu{
    public class MenuEntryFunc<T> : AbstractMenuEntry<T>{
        private Func<T> _func;

        public MenuEntryFunc(string text, Func<T> func, ISubscriber<T> observer = null) : base(text, observer){
            this._func = func;
        }

        public override string ToString(){
            return _text;
        }

        public override void Select(){
            T value = _func.Invoke();
            NotifySubscribers(value);
        }
    }
}
