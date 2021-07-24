namespace tarot{
    public interface IMenu{
        public void Up();
        public void Down();
        public void Left();
        public void Right();
        public void Select();
        public void Draw(int x, int y);
    }
}