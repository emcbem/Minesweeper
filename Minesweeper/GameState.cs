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

        public int numFlagged { get; private set; }
        public int numEmpty { get; private set; }
        

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
            board = new Board(rows, cols);
            numFlagged = 0;
            numEmpty = rows * cols;

            //CHANGE THE WAY MINES ARE MADE. mAKE THE RANDOM GENERATOR GET TWO NUMBERS AND USE THEM AS THE COORDINATES OF THE BOMB.
            //SET MAX BOMBS BEFORE BEING MADE
        }

        public void BLOWN()
        {
            isBlown = true;
        }

    }
}
