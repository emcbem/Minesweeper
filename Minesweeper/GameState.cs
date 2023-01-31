using System;
using System.Linq.Expressions;

namespace Minesweeper
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public Board board { get; }
        public bool isBlown { get; private set; }

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
            board = new Board(rows, cols);
        }

        public void BLOWN()
        {
            isBlown = true;
        }

    }
}
