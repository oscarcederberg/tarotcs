namespace tarot{
    class ValueLogger<T> : AbstractLogger<T>{
        private string _onRetrieval;
        public ValueLogger(string _onRetrieval, Log log = null) : base(log){
            this._onRetrieval = _onRetrieval;
        }

        override public void Notify(T value){
            LogEntry entry = new LogEntry(_onRetrieval + value.ToString());
            _logs.ForEach(x => x.Add(entry));
        }
    }
}
