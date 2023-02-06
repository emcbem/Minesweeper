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
        public bool isMine { get; private set; }
        public bool isFlagged { get; private set; }

        public bool isShown { get; private set; }
     
        public Cell(bool isMine)
        {
            this.isMine = isMine;
        }

        public Cell(bool isMine, bool isShown)
        {
            this.isMine = isMine;
            this.isShown = isShown;
        }
        public void Flag()
        {
            if (isFlagged) isFlagged = false;
            else isFlagged = true;
            return;
        }
        public bool click()
        {
            if (isShown || isFlagged)
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

        public void Shown()
        {
            isShown = true;
        }

        public void MakeMine()
        {
            isMine = true;
        }
    }
}
