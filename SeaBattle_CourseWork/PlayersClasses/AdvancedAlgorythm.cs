using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public class AdvancedAlgorythm : IShootingAlgorythm
    {
        private readonly Random _random;
        private readonly List<Point> _currentTarget;
        private Player _player;

        private static AdvancedAlgorythm _instance;

        protected AdvancedAlgorythm(Player player)
        {
            _player = player;
            _random = new Random(DateTime.Now.Millisecond);
            _currentTarget = new List<Point>();
        }

        public static AdvancedAlgorythm Instance(Player player)
        {
            if (_instance == null)
                _instance = new AdvancedAlgorythm(player);
            return _instance;
        }

        public void OnTimer(object sender, EventArgs e)
        {
            if (_currentTarget.Count == 0)
            {
                ShootRandom();
                return;
            }

            TryDownShip();
        }

        public void AddShotResult(int x, int y, ShotResult result)
        {
            if (result == ShotResult.ShipDrowned)
            {
                _currentTarget.Add(new Point(x, y));
                ShipDrowned();
                return;
            }

            if (result == ShotResult.ShipHit)
            {
                _currentTarget.Add(new Point(x, y));
            }
        }

        public void Reset()
        {
            _currentTarget.Clear();
        }

        private void ShootRandom()
        {
            int x;
            int y;
            do
            {
                x = _random.Next(0, 10);
                y = _random.Next(0, 10);
            } while (_player.PastShots.ContainsKey(new Point(x, y)));

            _player.ShotTargetChosen(x, y);
        }

        private bool IsValidShot(Point p)
        {
            return !_player.PastShots.ContainsKey(p) && new Rect(0, 0, 10, 10).Contains(p);
        }

        private Point GetRandomNeighbour(Point p)
        {
            int x;
            int y;
            do
            {
                x = _random.Next(-1, 2);
                y = _random.Next(-1, 2);
            } while (Math.Abs(x + y) != 1);

            return new Point(p.X + x, p.Y + y);
        }

        private void TryDownShip()
        {
            Point lastHit;
            Point prevHit;
            Point nextShot;

            if (_currentTarget.Count == 1)
            {
                lastHit = _currentTarget[0];

                do
                {
                    nextShot = GetRandomNeighbour(lastHit);
                } while (!IsValidShot(nextShot));
            }
            else
            {
                lastHit = _currentTarget[_currentTarget.Count - 1];
                prevHit = _currentTarget[_currentTarget.Count - 2];

                var x = lastHit.X - prevHit.X;
                var y = lastHit.Y - prevHit.Y;

                nextShot = new Point(lastHit.X + x, lastHit.Y + y);

                if (!IsValidShot(nextShot))
                {
                    x = _currentTarget[0].X - _currentTarget[1].X;
                    y = _currentTarget[0].Y - _currentTarget[1].Y;

                    nextShot = new Point(_currentTarget[0].X + x, _currentTarget[0].Y + y);

                    /*if (!IsValidShot(nextShot))
                        throw new Exception("logic failed");*/
                }
            }
            _player.ShotTargetChosen(nextShot.X, nextShot.Y);
        }

        private void ShipDrowned()
        {
            foreach (var p in _currentTarget)
            {
                _player.PastShots[new Point(p.X - 1, p.Y - 1)] = ShotResult.ShipDrowned;
                _player.PastShots[new Point(p.X - 1, p.Y)] = ShotResult.ShipDrowned;
                _player.PastShots[new Point(p.X - 1, p.Y + 1)] = ShotResult.ShipDrowned;

                _player.PastShots[new Point(p.X, p.Y - 1)] = ShotResult.ShipDrowned;
                _player.PastShots[new Point(p.X, p.Y)] = ShotResult.ShipDrowned;
                _player.PastShots[new Point(p.X, p.Y + 1)] = ShotResult.ShipDrowned;

                _player.PastShots[new Point(p.X + 1, p.Y - 1)] = ShotResult.ShipDrowned;
                _player.PastShots[new Point(p.X + 1, p.Y)] = ShotResult.ShipDrowned;
                _player.PastShots[new Point(p.X + 1, p.Y + 1)] = ShotResult.ShipDrowned;
            }

            _currentTarget.Clear();
        }

    }
}
