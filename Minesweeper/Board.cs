using System;

namespace Minesweeper
{
    public class Board
    {
        private Cell[,] cells;
        private int[,] numAdjacent;
        public Board(int x, int y)
        {
            x += 2;
            y += 2;
            cells = new Cell[x, y];
            Random random= new Random(x * y + ((int)DateTime.Now.Millisecond) * ((int)DateTime.Now.Minute));
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    if(i == 0 || i == x - 1 || j == 0 || j == y -1) { cells[i, j] = new Cell(false, true); continue; }
                    else cells[i, j] = random.Next(0, 10) == 4 ? new Cell(true) : new Cell(false);

                }
            }

            numAdjacent = new int[x, y];
            for(int i = 1; i < x - 1; i++)
            {
                for (int j = 1; j < y - 1; j++)
                {
                    if (cells[i + 1, j].isMine) numAdjacent[i, j]++;
                    if (cells[i + 1, j + 1].isMine) numAdjacent[i, j]++; 
                    if (cells[i, j + 1].isMine) numAdjacent[i, j]++; 
                    if (cells[i - 1, j + 1].isMine) numAdjacent[i, j]++;
                    if (cells[i - 1, j].isMine) numAdjacent[i, j]++;
                    if (cells[i-1, j-1].isMine) numAdjacent[i, j]++;
                    if (cells[i, j-1].isMine) numAdjacent[i, j]++;
                    if (cells[i + 1, j - 1].isMine) numAdjacent[i, j]++;
                }
            }
        }

        public bool checkIfBomb(int x, int y)
        {
            return cells[x + 1, y + 1].isMine;
        }

        public int getNumAdj(int x, int y)
        {
            return numAdjacent[x + 1, y + 1];
        }
        
        public bool checkIfShown(int x, int y)
        {
            return cells[x + 1, y + 1].isShown;
        }

        public void makeShown(int x, int y)
        {
            cells[x + 1, y + 1].Shown();
        }

        public bool checkIfFlagged(int x, int y)
        {
            return cells[x + 1, y + 1].isFlagged;
        }

        public void flag(int x, int y)
        {
            cells[x + 1, y + 1].Flag();
        }
    }
}
