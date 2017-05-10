using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public abstract class BoardCellState
    {
        protected string text;
        protected Color color;

        public string Text { get { return text; } }
        public Color Color { get { return color; } }
    }

    public class NormalState : BoardCellState
    {
        public NormalState()
        {
            text = string.Empty;
            color = Color.FromArgb(222, 222, 222);
        }
    }

    public class MissedShotState : BoardCellState
    {
        public MissedShotState()
        {
            text = ((char)0x3D).ToString();
            color = Color.FromArgb(222, 222, 222);
        }
    }

    public class ShipState : BoardCellState
    {
        public ShipState()
        {
            text = string.Empty;
            color = Color.FromArgb(65, 133, 243);
        }
    }

    public class ShotShipState : BoardCellState
    {
        public ShotShipState()
        {
            text = ((char)0x72).ToString();
            color = Color.FromArgb(65, 133, 243);
        }
    }

    public class ShipDragState : BoardCellState
    {
        public ShipDragState()
        {
            text = string.Empty;
            color = Color.Green;
        }
    }

    public class ShipDragInvalidState : BoardCellState
    {
        public ShipDragInvalidState()
        {
            text = string.Empty;
            color = Color.Red;
        }
    }

    public class ShipDrownedState : BoardCellState
    {
        public ShipDrownedState()
        {
            text = ((char)0x72).ToString();
            color = Color.Red;
        }
    }
}
