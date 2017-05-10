using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SeaBattle_CourseWork
{
    public class BoardCell : Label
    {
        private static readonly Color DefaultBorderColor = Color.FromArgb(214, 214, 214);

        public int X { get; private set; }
        public int Y { get; private set; }
        /*public enum BoardCellState
   {
       Normal,
       MissedShot,
       Ship,
       ShotShip,
       ShipDrag,
       ShipDragInvalid,
       ShowDrowned
   }*/
        private BoardCellState _currentState;

        private NormalState _normalState;
        private MissedShotState _missedShotState;
        private ShipState _shipState;
        private ShotShipState _shotShipState;
        private ShipDragState _shipDragState;
        private ShipDragInvalidState _shipDragInvalidState;
        private ShipDrownedState _shipDrownedState;

        public BoardCell(int x, int y)
        {
            X = x;
            Y = y;

            _normalState = new NormalState();
            _missedShotState = new MissedShotState();
            _shipState = new ShipState();
            _shotShipState = new ShotShipState();
            _shipDragState = new ShipDragState();
            _shipDragInvalidState = new ShipDragInvalidState();
            _shipDrownedState = new ShipDrownedState();

            base.AutoSize = false;
            base.TextAlign = ContentAlignment.MiddleCenter;
            base.Font = new Font("Webdings", 10);
            base.AllowDrop = true;
        }

        public BoardCellState CurrentState
        {
            get
            {
                return _currentState;
            }

       /*     set
            {
                _state = value;
                CellStateChanged(1);
            }*/
        }

        public void CellStateChanged(int code)
        {
            SuspendLayout();
            switch(code)
            {
                case 1:
                    _currentState = _normalState;
                    SetNewCellState();
                    break;
                case 2:
                    _currentState = _missedShotState;
                    SetNewCellState();
                    break;
                case 3:
                    _currentState = _shipState;
                    SetNewCellState();
                    break;
                case 4:
                    _currentState = _shotShipState;
                    SetNewCellState();
                    break;
                case 5:
                    _currentState = _shipDragState;
                    SetNewCellState();
                    break;
                case 6:
                    _currentState = _shipDragInvalidState;
                    SetNewCellState();
                    break;
                case 7:
                    _currentState = _shipDrownedState;
                    SetNewCellState();
                    break;       
            }
            SetNewCellState();
            Invalidate();
            ResumeLayout();
        }

        private void SetNewCellState()
        {
            Text = _currentState.Text;
            BackColor = _currentState.Color;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var pen = new Pen(DefaultBorderColor))
            {
                pen.Alignment = PenAlignment.Inset;
                pen.DashStyle = DashStyle.Solid;

                var rect = ClientRectangle;
                rect.Height -= 1;
                rect.Width -= 1;

                e.Graphics.DrawRectangle(pen, rect);
            }
        }

    }
}
