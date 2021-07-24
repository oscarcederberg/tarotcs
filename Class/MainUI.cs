using System;

namespace tarot{
    public class MainUI : IUserInterface{
        private readonly int _width = 60;
        private readonly int _height = 30;
        private readonly int _visibleLogs = 10;
        private readonly ListMenu _menu;
        private readonly Log _log;    
        private readonly Deck _deck;
        private Boolean _active;

        public MainUI(Deck deck) {
            _deck = deck;
            _log = new Log();

            MenuEntryFunc<bool> menuEntry1 = new MenuEntryFunc<bool>("Overhand Shuffle", () =>{
                    deck.ShuffleDeck(ShuffleType.Overhand);
                    return true; 
                }, new ActionLogger("Performed Overhand Shuffle",
                "Failed to perform Overhand Shuffle", _log));

            MenuEntryFunc<bool> menuEntry2 = new MenuEntryFunc<bool>("Riffle Shuffle", () => {
                    deck.ShuffleDeck(ShuffleType.Riffle);
                    return true;
                }, new ActionLogger("Performed Riffle Shuffle",
                "Failed to perform Riffle Shuffle", _log));

            MenuEntryFunc<ICard> menuEntry3 = new MenuEntryFunc<ICard>("Take Card", () => 
                deck.RequeueCard(), 
                new ValueLogger<ICard>("You retrieved: ", _log));

            MenuEntryFunc<bool> menuEntry4 = new MenuEntryFunc<bool>("Exit", () =>{
                    Environment.Exit(0);
                    return true;
                });

            IMenuEntry[] menuEntries = { menuEntry1, menuEntry2, menuEntry3, menuEntry4 };
            _menu = new ListMenu(menuEntries, "=> ");
        }

        public bool IsActive(){
            return _active;
        }

        public void Show(){
            Init();
            _active = true;

            while (_active){
                Clear();
                _menu.Draw(1, 1);
                DrawLog(1, _height - 3 - _visibleLogs);
                Console.SetCursorPosition(1, _height - 2);
                Console.Write("> ");

                while(!Console.KeyAvailable){
                    System.Threading.Thread.Sleep(100);
                }

                CheckKey(Console.ReadKey());  
            }
        }

        public IUserInterface Switch(IUserInterface ui){
            _active = false;
            ui.Show();
            return this;
        }

        private void CheckKey(ConsoleKeyInfo keyInfo){
            switch (keyInfo.Key){
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    _menu.Up();
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    _menu.Down();
                    break;
                case ConsoleKey.Enter:
                    _menu.Select();
                    break;
                default:
                    break;
            }
        }

        private void Clear(){
            Console.Clear();
        }

        private void Init(){
            #pragma warning disable CA1416 // Validate platform compatibility
            Console.SetWindowSize(_width, _height);
            Console.SetBufferSize(_width, _height);
            #pragma warning restore CA1416 // Validate platform compatibility
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

        public void DrawBox(){
            throw new NotImplementedException();
        }

        private void DrawLog(int x, int y){
            Console.SetCursorPosition(x, y);
            Console.Write("Log:");
            for (int i = 0; i < _visibleLogs; i++){
                if(_log.Count() - i > 0) {
                    Console.SetCursorPosition(x, y + _visibleLogs - i);
                    Console.Write(_log.Get(_log.Count() - 1 - i));
                }
            }
        }

        private class Box{
            public Point Point1;
            public Point Point2;

            public Box(Point p1, Point p2){
                this.Point1 = p1;
                this.Point2 = p2;
            }

            public int GetWidth(){
                return Point2.X - Point1.X;
            }

            public int GetHeight(){
                return Point2.Y - Point1.Y;
            }

            public void Draw(){
                Console.SetCursorPosition(Point1.X, Point1.Y);
                Console.Write("┌");
                Console.SetCursorPosition(Point2.X, Point1.Y);
                Console.Write("┐");
                Console.SetCursorPosition(Point1.X, Point2.Y);
                Console.Write("└");
                Console.SetCursorPosition(Point2.X, Point2.Y);
                Console.Write("┘");
                
                for (int i = 1; i < GetWidth() - 1; i++){
                    Console.SetCursorPosition(Point1.X + i, Point1.Y);
                    Console.Write("─");
                    Console.SetCursorPosition(Point1.X + i, Point2.Y);
                    Console.Write("─");
                }

                for (int i = 1; i < GetHeight() - 1; i++){
                    Console.SetCursorPosition(Point1.X, Point1.Y + i);
                    Console.Write("│");
                    Console.SetCursorPosition(Point2.X, Point1.Y + i);
                    Console.Write("│");
                }
            }
        }

        private class Point{
            public int X;
            public int Y;

            public Point(int x, int y){
                this.X = x;
                this.Y = y;
            }
        }
    }
}
