using System;
using Subscription;

namespace MenuCLI{
    public class FuncEntry<T> : MenuEntry<T>{
        private Func<T> _func;

        public FuncEntry(string text, Func<T> func, ISubscriber<T> subscriber = null) : base(text, subscriber){
            this._func = func;
        }

        public override string GetText(){
            return _text;
        }

        public override void Select(){
            T value = _func.Invoke();
            NotifySubscribers(value);
        }
    }
}
