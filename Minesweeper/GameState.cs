
namespace Minesweeper
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public Board board { get; }
        public bool isBlown { get; private set; }

    }
}
