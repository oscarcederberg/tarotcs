namespace tarot{
    public interface IUserInterface{
        public bool IsActive();
        public void Show();
        public IUserInterface Switch(IUserInterface ui);
    }
}