using Cubokta.Common;
using Cubokta.Puyo.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    class PuyoTypeDetector
    {
        private IDictionary<PuyoType, Color> baseColors = new Dictionary<PuyoType, Color>()
        {
            { PuyoType.AKA, Color.FromArgb(0xaf, 0x74, 0x85) },
            { PuyoType.MIDORI, Color.FromArgb(0x53, 0x9f, 0x37) },
            { PuyoType.AO, Color.FromArgb(0x11, 0x36, 0x7a) },
            { PuyoType.KI, Color.FromArgb(0x95, 0x5c, 0x00) },
            { PuyoType.MURASAKI, Color.FromArgb(0x65, 0x05, 0x6c) },
        };

        public IDictionary<PuyoType, Color> BaseColors
        {
            get
            {
                return baseColors;
            }
            set
            {
                baseColors = value;
            }
        }
        
        public PuyoType Detect(RapidBitmapAccessor ba, Rectangle rect)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            IDictionary<PuyoType, int> typeCounts = new Dictionary<PuyoType, int> {
                { PuyoType.AKA, 0 },
                { PuyoType.MIDORI, 0 },
                { PuyoType.AO, 0 },
                { PuyoType.KI, 0 },
                { PuyoType.MURASAKI, 0 },
            };

            rect = new Rectangle()
            {
                X = rect.X + CaptureField.UNIT / 4,
                Y = rect.Y + CaptureField.UNIT / 4,
                Width = rect.Width / 2,
                Height = rect.Height / 2,
            };

            for (int y = rect.Top; y < rect.Top + rect.Height; y++)
            {
                for (int x = rect.Left; x < rect.Left + rect.Width; x++)
                {
                    Color c = ba.GetPixel(x, y);
                    r = (int)c.R;
                    g = (int)c.G;
                    b = (int)c.B;
                    Color bc = Color.FromArgb(r, g, b);
                    int diffValueOfRed = DetectColorDiff(bc, baseColors[PuyoType.AKA]);
                    int diffValueOfGreen = DetectColorDiff(bc, baseColors[PuyoType.MIDORI]);
                    int diffValueOfBlue = DetectColorDiff(bc, baseColors[PuyoType.AO]);
                    int diffValueOfYellow = DetectColorDiff(bc, baseColors[PuyoType.KI]);
                    int diffValueOfPurple = DetectColorDiff(bc, baseColors[PuyoType.MURASAKI]);

                    int[] diffs = new int[]
                    {
                        diffValueOfRed,
                        diffValueOfGreen,
                        diffValueOfBlue,
                        diffValueOfYellow,
                        diffValueOfPurple,
                    };

                    PuyoType type = GetPuyoType(diffs);
                    if (type != PuyoType.NONE)
                    {
                        typeCounts[type]++;
                    }
                }
            }

            if (typeCounts.Max(pair => pair.Value) < 32)
            {
                return PuyoType.NONE;
            }
            PuyoType p = (from n in typeCounts
                          where n.Value == (typeCounts.Max(pair => pair.Value))
                          select n.Key).First();
            return p;
        }

        private const int COLOR_THRESHOLD = 32 * 32 * 3;
        private PuyoType GetPuyoType(int[] diffs)
        {
            PuyoType[] TYPES = new PuyoType[]
            {
                PuyoType.AKA,
                PuyoType.MIDORI,
                PuyoType.AO,
                PuyoType.KI,
                PuyoType.MURASAKI,
            };

            int minIndex = 0;
            int minValue = int.MaxValue;
            for (int i = 0; i < diffs.Length; i++)
            {
                if (diffs[i] < minValue)
                {
                    minIndex = i;
                    minValue = diffs[i];
                }
            }

            if (minValue > COLOR_THRESHOLD)
            {
                return PuyoType.NONE;
            }
            else
            {
                return TYPES[minIndex];
            }
        }

        private int DetectColorDiff(Color c1, Color c2)
        {
            int diffRed = (c1.R - c2.R);
            int diffGreen = (c1.G - c2.G);
            int diffBlue = (c1.B - c2.B);

            return diffRed * diffRed + diffGreen * diffGreen + diffBlue * diffBlue;
        }
    }
}
