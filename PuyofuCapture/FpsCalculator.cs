using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    public class FpsCalculator
    {
        private const int FREQUENCY = 1000;
        private float fps;
        private int prevMills = DateTime.Now.Millisecond;

        public void Refresh()
        {
            int curMills = DateTime.Now.Millisecond;
            if (curMills - prevMills > 0)
            {
                fps = FREQUENCY / (curMills - prevMills);
            }
            prevMills = curMills;
        }

        public int GetFpsInt()
        {
            return (int)fps;
        }
    }
}
