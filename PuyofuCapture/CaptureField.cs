using Cubokta.Puyo.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    public class CaptureField
    {
        public const int X_MAX = FieldConst.FIELD_X;
        public const int Y_MAX = FieldConst.FIELD_Y;
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

        public ColorPairPuyo GetStepFromDiff(CaptureField f2, ColorPairPuyo pp)
        {
            CaptureField f1 = this;
            bool foundPivot = false;
            bool foundSatellite = false;
            ColorPairPuyo p2 = new ColorPairPuyo();
            Point pivotPt = new Point(-1, -1);
            Point satellitePt = new Point(-1, -1);
            for (int x = 0; x < X_MAX; x++)
            {
                for (int y = 0; y < Y_MAX; y++)
                {
                    PuyoType pt1 = f1.GetPuyoType(x, y);
                    PuyoType pt2 = f2.GetPuyoType(x, y);

                    if (pt1 != pt2)
                    {
                        if (!foundPivot && pp.Pivot == pt2)
                        {
                            p2.Pos = x;
                            p2.Pivot = pt2;
                            foundPivot = true;
                            pivotPt.X = x;
                            pivotPt.Y = y;
                        }
                        else if (pp.Satellite == pt2)
                        {
                            p2.Satellite = f2.GetPuyoType(x, y);
                            foundSatellite = true;
                            satellitePt.X = x;
                            satellitePt.Y = y;
                        }
                        else
                        {
                            return null;
                        }

                        if (foundPivot && foundSatellite)
                        {
                            if (pivotPt.X == satellitePt.X && pivotPt.Y < satellitePt.Y)
                            {
                                p2.Dir = Direction.UP;
                            }
                            else if (pivotPt.X == satellitePt.X)
                            {
                                p2.Dir = Direction.DOWN;
                            }
                            else if (pivotPt.X < satellitePt.X)
                            {
                                p2.Dir = Direction.RIGHT;
                            }
                            else
                            {
                                p2.Dir = Direction.LEFT;
                            }

                            p2.Pos++;
                            return p2;
                        }
                    }
                }
            }

            return null;
        }

        private static readonly IDictionary<PuyoType, string> TYPE2KANJI = new Dictionary<PuyoType, string>()
        {
            { PuyoType.NONE, "□" },
            { PuyoType.AKA, "赤" },
            { PuyoType.MIDORI, "緑" },
            { PuyoType.AO, "青" },
            { PuyoType.KI, "黄" },
            { PuyoType.MURASAKI, "紫" },
        };

        public void Drop(ColorPairPuyo pp)
        {
            int pos = pp.Pos - 1;
            if (pp.Dir == Direction.UP)
            {
                DropOne(pp.Pivot, pos);
                DropOne(pp.Satellite, pos);
            }
            else if (pp.Dir == Direction.DOWN)
            {
                DropOne(pp.Satellite, pos);
                DropOne(pp.Pivot, pos);
            }
            else if (pp.Dir == Direction.LEFT)
            {
                DropOne(pp.Pivot, pos);
                DropOne(pp.Satellite, pos - 1);
            }
            else
            {
                DropOne(pp.Pivot, pos);
                DropOne(pp.Satellite, pos + 1);
            }
        }

        private void DropOne(PuyoType type, int pos)
        {
            for (int y = 0; y < Y_MAX; y++)
            {
                if (GetPuyoType(pos, y) == PuyoType.NONE)
                {
                    SetPuyoType(pos, y, type);
                    return;
                }
            }
        }

        public void Correct()
        {
            // 浮いているぷよは除外する
            for (int y = 1; y < Y_MAX; y++)
            {
                for (int x = 0; x < X_MAX; x++)
                {
                    if (GetPuyoType(x, y - 1) == PuyoType.NONE)
                    {
                        SetPuyoType(x, y, PuyoType.NONE);
                    }
                }
            }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Y_MAX; y++)
            {
                for (int x = 0; x < X_MAX; x++)
                {
                    sb.Append(TYPE2KANJI[Types[y, x]]);
                }

                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
