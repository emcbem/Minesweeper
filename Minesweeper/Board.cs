
using System;

namespace Minesweeper
{
    public class Board
    {
        private int[,] cells;
        private int[,] numAdjacent;

        Board(int x, int y)
        {
            x += 2;
            y += 2;
            cells = new int[x, y];
            Random random= new Random(x * y + ((int)DateTime.Now.Millisecond));
            for(int i = 1; i < x - 1; i++)
            {
                for(int j = 1; j < y - 1; j++)
                {
                    cells[i, j] = random.Next(0, 1);
                }
            }

            numAdjacent = new int[x, y];
            for(int i = 1; i < x - 1; i++)
            {
                for (int j = 1; j < y - 1; j++)
                {
                    numAdjacent[i, j] = cells[i + 1, j] + cells[i + 1, j + 1] + cells[i, j + 1] + cells[i - 1, j + 1] + cells[i - 1, j] + cells[i-1, j-1] + cells[i, j-1] + cells[i + 1, j - 1];
                }
            }
        }
    }
}
