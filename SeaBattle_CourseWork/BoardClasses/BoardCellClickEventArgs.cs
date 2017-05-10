using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle_CourseWork
{
    public class BoardCellClickEventArgs : EventArgs
    {
        private readonly int _x;
        private readonly int _y;

        public BoardCellClickEventArgs(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int Y
        {
            get { return _y; }
        }

        public int X
        {
            get { return _x; }
        }
    }
}
