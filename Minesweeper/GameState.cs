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

        public int numMines { get; private set; }

        public int numFlagged { get; private set; }
        public int numEmpty { get; private set; }

        public bool isFinished { get { if (numFlagged + numEmpty == numMines) return true; else return false; } }
        

        public GameState(int rows, int cols, int mines)
        {
            Rows = rows;
            Columns = cols;
            board = new Board(rows, cols, mines);
            numMines = mines;
            numFlagged = 0;
            numEmpty = rows * cols;
        }

        public void BLOWN()
        {
            isBlown = true;
        }

    }
}
