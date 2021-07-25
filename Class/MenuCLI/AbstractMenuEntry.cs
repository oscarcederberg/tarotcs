using System.Collections.Generic;
using Subscription;

namespace MenuCLI{
    public abstract class AbstractMenuEntry<T> : IMenuEntry, ISubscribable<T>{
        protected string _text;
        protected readonly List<ISubscriber<T>> _subscribers;

        protected AbstractMenuEntry(string text, ISubscriber<T> subscriber = null){
            this._text = text;
            this._subscribers = new List<ISubscriber<T>>();
            if (subscriber is not null) _subscribers.Add(subscriber);
        }

        public virtual string GetText(){
            return _text;
        }

        public abstract void Select();

        public virtual void Subscribe(ISubscriber<T> subscriber){
            _subscribers.Add(subscriber);
        }

        public virtual void NotifySubscribers(T value){
            _subscribers.ForEach(x => x.Notify(value));
        }
    }
}
