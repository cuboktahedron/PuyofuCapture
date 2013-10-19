using Cubokta.Common;
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
    class PuyoTypeDetector
    {
        /// <summary>色識別用にRGBの各要素を分類する際に必要となるビット数</summary>
        /// <remarks>3、4、5、6のいずれかを指定する。</remarks>
        private const int COLOR_ELEMENT_BIT = 3;

        /// <summary>色の分類数(0～255を何分割するか)</summary>
        private const int COLOR_DIVISION_NUM = 2 << (COLOR_ELEMENT_BIT - 1);


        /// <summary>色識別用サンプルデータ</summary>
        private IDictionary<PuyoType, List<int>> colorSamples = new Dictionary<PuyoType, List<int>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PuyoTypeDetector()
        {
            colorSamples[PuyoType.AKA] = new List<int>(new int[256 / COLOR_DIVISION_NUM * 3]);
            colorSamples[PuyoType.MIDORI] = new List<int>(new int[256 / COLOR_DIVISION_NUM * 3]);
            colorSamples[PuyoType.AO] = new List<int>(new int[256 / COLOR_DIVISION_NUM * 3]);
            colorSamples[PuyoType.KI] = new List<int>(new int[256 / COLOR_DIVISION_NUM * 3]);
            colorSamples[PuyoType.MURASAKI] = new List<int>(new int[256 / COLOR_DIVISION_NUM * 3]);
        }

        // TODO: いずれ消す
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

        /// <summary>
        /// ぷよタイプ識別用のサンプルデータを更新する。
        /// </summary>
        /// <param name="puyoType">ぷよタイプ</param>
        /// <param name="ba">サンプルデータ画像アクセサ</param>
        /// <remarks>サンプルデータ画像のサイズは1セルのサイズとする。</remarks>
        public void UpdateSample(PuyoType puyoType, RapidBitmapAccessor ba)
        {
            colorSamples[puyoType] = new List<int>(new int[256 / COLOR_DIVISION_NUM * 3]);
            for (int x = 0; x < CaptureField.UNIT; x++)
            {
                for (int y = 0; y < CaptureField.UNIT; y++)
                {
                    // RGBから色番号を決定
                    Color c = ba.GetPixel(x, y);
                    int colorNo = c.R / (256 / COLOR_DIVISION_NUM);
                    colorNo += COLOR_DIVISION_NUM;

                    colorNo += (c.G / (256 / COLOR_DIVISION_NUM));
                    colorNo += COLOR_DIVISION_NUM;

                    colorNo += c.B / (256 / COLOR_DIVISION_NUM);

                    colorSamples[puyoType][colorNo]++;
                }
            }

#if DEBUG
            Debug.WriteLine("サンプルデータセット(ぷよ種別 = " + puyoType + ")");
            for (int i = 0, len = colorSamples[puyoType].Count; i < len; i++)
            {
                Debug.WriteLine(colorSamples[puyoType][i]);
            }
            Debug.Flush();
#endif
        }
    }
}
