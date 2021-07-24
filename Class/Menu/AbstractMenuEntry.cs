using System.Collections.Generic;
using tarot.Subscription;

namespace tarot.Menu{
    public abstract class AbstractMenuEntry<T> : IMenuEntry, ISubscribable<T>{
        protected string _text;
        protected readonly List<ISubscriber<T>> _observers;

        protected AbstractMenuEntry(string text, ISubscriber<T> observer = null){
            this._text = text;
            this._observers = new List<ISubscriber<T>>();
            if (observer is not null) _observers.Add(observer);
        }

        public virtual string ToString(){
            return _text;
        }

        public abstract void Select();

        public virtual void Subscribe(ISubscriber<T> observer){
            _observers.Add(observer);
        }

        public virtual void NotifySubscribers(T value){
            _observers.ForEach(x => x.Notify(value));
        }
    }
}
