using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public abstract class Player
    {

        public readonly Dictionary<Point, ShotResult> PastShots;
        private bool _canShoot;

        public String Name { get; set; }

        public event EventHandler<ShootingEventArgs> Shooting;
        public event EventHandler<ShootingEventArgs> Shot;
        public event EventHandler MyTurn;

        protected Player(string name)
        {
            Name = name;
            PastShots = new Dictionary<Point, ShotResult>();
        }

        public virtual void Shoot()
        {
            _canShoot = true;
            var handler = MyTurn;
            if (handler != null)
                handler(this, new EventArgs());
        }

        public virtual void Reset()
        {
            PastShots.Clear();
            _canShoot = false;
        }

        public void ShotTargetChosen(int x, int y)
        {
            if (!_canShoot)
                return;

            _canShoot = false;

            var shooting = Shooting;
            if (shooting == null)
                return;
            var eventArgs = new ShootingEventArgs(x, y);
            shooting(this, eventArgs);
            AddShotResult(x, y, eventArgs.Result);

            var shot = Shot;
            if (shot != null)
                shot(this, eventArgs);
        }

        protected virtual void AddShotResult(int x, int y, ShotResult result)
        {
            PastShots[new Point(x, y)] = result;
        }

    }
}
