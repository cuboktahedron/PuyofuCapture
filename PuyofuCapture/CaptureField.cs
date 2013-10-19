using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta
{
    public class CaptureField
    {
        public const int X_MAX = 6;
        public const int Y_MAX = 12;
        public const int UNIT = 32;

        private PuyoType[,] Types { get; set; }
        private ColorPairPuyo[] Nexts { get; set; }

        public PuyoType GetPuyoType(int x, int y)
        {
            return Types[(Y_MAX - 1) - y, x];
        }

        public void SetPuyoType(int x, int y, PuyoType type)
        {
            Types[(Y_MAX - 1) - y, x] = type;
        }

        public ColorPairPuyo GetNext(int no)
        {
            return Nexts[no];
        }

        public void SetNext(int no, ColorPairPuyo pp)
        {
            Nexts[no] = pp;
        }

        public CaptureField()
        {
            Types = new PuyoType[Y_MAX, X_MAX];
            Nexts = new ColorPairPuyo[1];
        }

        public Rectangle GetRect(int x, int y)
        {
            return new Rectangle()
            {
                X = x * UNIT,
                Y = ((Y_MAX - 1) - y) * UNIT,
                Width = UNIT,
                Height = UNIT,
            };
        }

        public Rectangle GetNextRect(int no, int y)
        {
            return new Rectangle()
            {
                X = 0,
                Y = (1 - y) * UNIT,
                Width = UNIT,
                Height = UNIT,
            };
        }
    }
}
