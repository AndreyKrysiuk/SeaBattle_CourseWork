using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle_CourseWork
{
    public class ComputerPlayer : Player
    {
        private Random _random;
        private Timer _timer;
        private IShootingAlgorythm _algo;

        public ComputerPlayer(string name) : base(name)
        {
            _algo = SimpleAlgorythm.Instance(this);
            _random = new Random(DateTime.Now.Millisecond);
            _timer = new Timer { Enabled = false };
            _timer.Tick += _algo.OnTimer;
        }

        public override void Shoot()
        {
            base.Shoot();
            _timer.Interval = _random.Next(100, 1000);
            _timer.Start();
        }

        protected override void AddShotResult(int x, int y, ShotResult result)
        {
            base.AddShotResult(x, y, result);
            _algo.AddShotResult(x, y, result);
        }

        public override void Reset()
        {
            base.Reset();
            _algo.Reset();
        }

        public void SetEasyLevel()
        {
            _timer.Tick -= _algo.OnTimer;
            _algo = SimpleAlgorythm.Instance(this);
            _timer.Tick += _algo.OnTimer;
        }

        public void SetNormLevel()
        {
            _timer.Tick -= _algo.OnTimer;
            _algo = AdvancedAlgorythm.Instance(this);
            _timer.Tick += _algo.OnTimer;
        }

    }    
}
