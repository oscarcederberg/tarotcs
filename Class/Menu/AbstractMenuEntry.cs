using System.Collections.Generic;
using tarot.Observer;

namespace tarot.Menu{
    public abstract class AbstractMenuEntry<T> : IMenuEntry, IObservable<T>{
        protected string _text;
        protected readonly List<IObserver<T>> _observers;

        protected AbstractMenuEntry(string text, IObserver<T> observer = null){
            this._text = text;
            this._observers = new List<IObserver<T>>();
            if (observer is not null) _observers.Add(observer);
        }

        public virtual string ToString(){
            return _text;
        }

        public abstract void Select();

        public virtual void AddObserver(IObserver<T> observer){
            _observers.Add(observer);
        }

        public virtual void NotifyObservers(T value){
            _observers.ForEach(x => x.Notify(value));
        }
    }
}
