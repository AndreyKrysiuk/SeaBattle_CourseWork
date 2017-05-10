using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public class Board : Control
    {
        private const int CellSize = 25;

        private static readonly Rect BoardRegion = new Rect(0, 0, 10, 10);

        private readonly BoardCell[,] _cells;
        private readonly List<Ship> _ships;
        private DraggableShip _draggedShip;
        private readonly Random _random;
        private readonly bool _drawShips;

        public ShipSetBuilder ShipSetBuilder { get; set; }

        public BoardMode Mode { get; set;}

        public new event EventHandler<BoardCellClickEventArgs> OnClick;

        public Board() : this(true) { }

        public Board(bool drawShips)
        {
            _drawShips = drawShips;
            _cells = new BoardCell[10, 10];
            _ships = new List<Ship>();
            _random = new Random(DateTime.Now.Millisecond);
            Mode = BoardMode.Design;
            Margin = Padding.Empty;

            CreateBoard();
        }

        private void CreateHeaders()
        {
            for(var i = 0; i < BoardRegion.Width; i++)
            {
                var offset = CellSize * i + CellSize;
                var columnHeader = FormElementsCreator.CreateHeaderCell(offset, 0, ((char)(i + 65)).ToString(), CellSize);
                var rowHeader = FormElementsCreator.CreateHeaderCell(0, offset, (i + 1).ToString(), CellSize);

                Controls.Add(columnHeader);
                Controls.Add(rowHeader);
            }
        }

        private void CreateBoard()
        {
            SuspendLayout();

            var boardSize = new Size(CellSize * BoardRegion.Width + CellSize, CellSize * BoardRegion.Height + CellSize);
            base.MinimumSize = boardSize;
            base.MaximumSize = boardSize;

            CreateHeaders();

            var points = BoardRegion.GetPoints();

            foreach(var point in points)
            {
                BoardCell cell = new BoardCell(point.X, point.Y)
                {
                    Top = point.X * CellSize + CellSize,
                    Left = point.Y * CellSize + CellSize,
                    Width = CellSize,
                    Height = CellSize,
                };
                cell.CellStateChanged(1);

                _cells[point.X, point.Y] = cell;
                cell.MouseDown += OnCellMouseDown;
                cell.DragEnter += OnCellDragEnter;
                cell.DragLeave += OnCellDragLeave;
                cell.DragDrop += OnCellDragDrop;
                cell.QueryContinueDrag += OnCellQueryContinueDrag;
                cell.Click += OnCellClick;
                Controls.Add(cell);           
            }

            ResumeLayout();
        }

        private void OnCellClick(Object sender, EventArgs e)
        {
            if (Mode != BoardMode.Game)
                return;

            var handler = OnClick;
            if (handler == null)
                return;
            BoardCell cell = (BoardCell)(sender);
            var eventArgs = new BoardCellClickEventArgs(cell.X, cell.Y);
            handler(this, eventArgs);
        }

        private Ship GetShipAt(int x, int y)
        {
            return _ships.FirstOrDefault(ship => ship.IsLocatedAt(x, y));
        }

        private void OnCellMouseDown(object sender, MouseEventArgs e)
        {
            if (Mode == BoardMode.Game || !_drawShips)
                return;

            var cell = (BoardCell)sender;
            var ship = GetShipAt(cell.X, cell.Y);

            if (ship == null)
                return;

            _draggedShip = DraggableShip.From(ship);
            cell.DoDragDrop(ship, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void OnCellQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            var shouldRotate = ((e.KeyState & 8) == 8);
            var isRotated = _draggedShip.IsOrientationModified;

            if ((shouldRotate && isRotated) || (!shouldRotate && !isRotated))
                return;

            var rect = _draggedShip.GetShipRegion();
            RedrawRegion(rect);

            _draggedShip.Rotate();
            _draggedShip.IsOrientationModified = !isRotated;

            int code = CanPlaceShip(_draggedShip, _draggedShip.X, _draggedShip.Y) ? 5 : 6;
            DrawShip(_draggedShip, code);
        }

        private void OnCellDragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(_draggedShip.Source.GetType()))
            {
                var cell = (BoardCell)sender;
                _draggedShip.MoveTo(cell.X, cell.Y);

                var canPlaceShip = CanPlaceShip(_draggedShip, cell.X, cell.Y);
                int code = canPlaceShip ? 5 : 6;

                DrawShip(_draggedShip, code);

                e.Effect = canPlaceShip ? DragDropEffects.Move : DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnCellDragLeave(object sender, EventArgs e)
        {
            var rect = _draggedShip.GetShipRegion();
            RedrawRegion(rect);
        }

        private void OnCellDragDrop(object sender, DragEventArgs e)
        {
            var cell = (BoardCell)sender;
            if(e.Data.GetDataPresent(_draggedShip.Source.GetType()))
            {
                if (!CanPlaceShip(_draggedShip, cell.X, cell.Y))
                    return;

                var ship = _draggedShip.Source;
                _ships.Remove(ship);

                var rect = ship.GetShipRegion();
                RedrawRegion(rect);

                ship.Orientation = _draggedShip.Orientation;

                AddShip(ship, cell.X, cell.Y);
                _draggedShip = null;
            } 
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        private void AddShip(Ship ship, int x, int y)
        {
            ship.MoveTo(x, y);

            _ships.Add(ship);
            DrawShip(ship, 3);
        }

        private bool CanPlaceShip(Ship ship, int x, int y)
        {
            var shipRegion = ship.GetShipRegion();

            shipRegion.MoveTo(x, y);

            if (!BoardRegion.Contains(shipRegion))
                return false;

            shipRegion.Inflate(1, 1);

            foreach(var s in _ships)
            {
                if(ship is DraggableShip && s == ((DraggableShip)ship).Source)
                {
                    continue;
                }

                if (s.GetShipRegion().IntersectsWith(shipRegion))
                {
                    return false;
                }
            }
            return true;
        }

        private void RedrawRegion(Rect region)
        {
            SuspendLayout();

            var points = region.GetPoints();
            foreach(var point in points)
            {
                if(!BoardRegion.Contains(point))
                {
                    continue;
                }

                var ship = GetShipAt(point.X, point.Y);
                _cells[point.X, point.Y].CellStateChanged(ship == null ? 1 : 3);
            }

            ResumeLayout();
        }

        public void ShowShips()
        {
            foreach(var ship in _ships)
            {
                var shipPoints = ship.GetShipRegion().GetPoints();

                foreach(var point in shipPoints)
                {
                    var cell = _cells[point.X, point.Y];
                    if (!(cell.CurrentState is NormalState))
                        continue;
                    cell.CellStateChanged(3);
                }
            }
        }

        private void DrawShip(Ship ship, int code)
        {
            DrawShip(ship, code, false);
        }

        private void DrawShip(Ship ship, int code, bool force)
        {
            if (!_drawShips && !force)
                return;

            var points = ship.GetShipRegion().GetPoints();

            foreach(var point in points)
            {
                if(BoardRegion.Contains(point))
                {
                    _cells[point.X, point.Y].CellStateChanged(code);
                }
            }
        }

        public void ClearBoard()
        {
            SuspendLayout();

            _ships.Clear();

            var points = BoardRegion.GetPoints();
            foreach(var point in points)
            {
                _cells[point.X, point.Y].CellStateChanged(1);
            }

            ResumeLayout();
        }

        public void AddRandomShips()
        {
            SuspendLayout();
            ClearBoard();

            var ships = GetNewShips();

            foreach(var ship in ships)
            {
                var shipAdded = false;

                while (!shipAdded)
                {
                    var x = _random.Next(10);
                    var y = _random.Next(10);

                    if (!CanPlaceShip(ship, x, y))
                        continue;

                    AddShip(ship, x, y);
                    shipAdded = true;
                }
            }

            ResumeLayout();
        }

        private IList<Ship> GetNewShips()
        {
            ShipSetGenerator shipGen = new ShipSetGenerator();
            var ships = shipGen.GenerateShipSet(ShipSetBuilder).shipSet;
            return ships;
        }

        public ShotResult OponentShotAt(int x, int y)
        {
            var ship = GetShipAt(x, y);

            if (ship == null)
            {
                _cells[x, y].CellStateChanged(2);
                return ShotResult.Missed;
            }
            _cells[x, y].CellStateChanged(4);
            ship.HitCount++;

            if (ship.IsDrowned)
            {     
                var region = ship.GetShipRegion().GetPoints();
                foreach(var r in region)
                {
                    if(r.X > 0)
                    {
                        if (r.Y > 0)
                            _cells[r.X - 1, r.Y - 1].CellStateChanged(2);

                        _cells[r.X - 1, r.Y].CellStateChanged(2);

                        if (r.Y < 9)
                            _cells[r.X - 1, r.Y + 1].CellStateChanged(2);
                    }


                    if (r.Y > 0)
                        _cells[r.X, r.Y - 1].CellStateChanged(2);

                        _cells[r.X, r.Y].CellStateChanged(2);

                    if(r.Y < 9)
                        _cells[r.X, r.Y + 1].CellStateChanged(2);

                    if (r.X < 9)
                    {
                        if (r.Y > 0)
                            _cells[r.X + 1, r.Y - 1].CellStateChanged(2);

                        _cells[r.X + 1, r.Y].CellStateChanged(2);

                        if (r.Y < 9)
                            _cells[r.X + 1, r.Y + 1].CellStateChanged(2);
                    }
                        
                    
                }
                DrawShip(ship, 7, true);

            }
                

            return ship.IsDrowned ? ShotResult.ShipDrowned : ShotResult.ShipHit;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Font = Parent.Font;
        }
    }
}
