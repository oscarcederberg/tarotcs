namespace tarot.Subscription{
    class ActionLogger : AbstractLogger<bool>{
        private string _onSuccess;
        private string _onFailure;
        public ActionLogger(string onSuccess, string onFailure, Log log = null) : base(log){
            this._onSuccess = onSuccess;
            this._onFailure = onFailure;
        }

        override public void Notify(bool value){
            LogEntry entry = new LogEntry(value ? _onSuccess : _onFailure);
            _logs.ForEach(x => x.Add(entry));
        }
    }
}
