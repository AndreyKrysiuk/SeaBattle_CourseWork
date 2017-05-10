using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle_CourseWork
{
    public interface IShootingAlgorythm
    {
        void OnTimer(object sender, EventArgs e);
        void AddShotResult(int x, int y, ShotResult result);
        void Reset();
    }
}