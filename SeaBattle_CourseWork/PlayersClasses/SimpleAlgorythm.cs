using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public class SimpleAlgorythm : IShootingAlgorythm
    {
        private readonly Random _random;
        private readonly List<Point> _currentTarget;
        private Player _player;

        private static SimpleAlgorythm _instance;

        protected SimpleAlgorythm(Player player)
        {
            _player = player;
            _random = new Random(DateTime.Now.Millisecond);
            _currentTarget = new List<Point>();
        }

        public static SimpleAlgorythm Instance(Player player)
        {
            if (_instance == null)
                _instance = new SimpleAlgorythm(player);
            return _instance;
        }

        public void OnTimer(object sender, EventArgs e)
        {
            ShootRandom();
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
