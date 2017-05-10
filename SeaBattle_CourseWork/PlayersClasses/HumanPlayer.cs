using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public class HumanPlayer : Player
    {
        private readonly Board _board;

        public HumanPlayer(string name, Board board) : base(name)
        {
            _board = board;
            _board.OnClick += OnBoardClick;
        }

        private void OnBoardClick(object sender, BoardCellClickEventArgs e)
        {
            if (PastShots.ContainsKey(new Point(e.X, e.Y)))
                return;

            ShotTargetChosen(e.X, e.Y);
        }
    }
}
