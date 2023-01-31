using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;

namespace Minesweeper
{
    public class Cell
    {
        public bool isMine { get; }
        public bool isFlagged { get; private set; }
     
        public Cell(bool isMine)
        {
            this.isMine = isMine;
        }
        public void Flag()
        {
            if (isFlagged) isFlagged = false;
            else isFlagged = true;
            return;
        }
        public bool click()
        {
            if(isFlagged)
            {
                //True means that the user is not dead
                return true;
            }
            if(isMine)
            {
                return false;
            }
            return true;
        }
    }
}
