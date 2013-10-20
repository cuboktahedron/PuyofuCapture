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
        private IDictionary<PuyoType, int[,,]> colorSamples = new Dictionary<PuyoType, int[,,]>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PuyoTypeDetector()
        {
            colorSamples[PuyoType.AKA] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.MIDORI] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.AO] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.KI] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            colorSamples[PuyoType.MURASAKI] = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
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

            int[,,] pattern = DetectPattern(ba, rect);
            return GetPuyoType(pattern);

            //rect = new Rectangle()
            //{
            //    X = rect.X + CaptureField.UNIT / 4,
            //    Y = rect.Y + CaptureField.UNIT / 4,
            //    Width = rect.Width / 2,
            //    Height = rect.Height / 2,
            //};

            //for (int y = rect.Top; y < rect.Top + rect.Height; y++)
            //{
            //    for (int x = rect.Left; x < rect.Left + rect.Width; x++)
            //    {
            //        Color c = ba.GetPixel(x, y);
            //        r = (int)c.R;
            //        g = (int)c.G;
            //        b = (int)c.B;
            //        Color bc = Color.FromArgb(r, g, b);
            //        int diffValueOfRed = DetectColorDiff(bc, baseColors[PuyoType.AKA]);
            //        int diffValueOfGreen = DetectColorDiff(bc, baseColors[PuyoType.MIDORI]);
            //        int diffValueOfBlue = DetectColorDiff(bc, baseColors[PuyoType.AO]);
            //        int diffValueOfYellow = DetectColorDiff(bc, baseColors[PuyoType.KI]);
            //        int diffValueOfPurple = DetectColorDiff(bc, baseColors[PuyoType.MURASAKI]);

            //        int[] diffs = new int[]
            //        {
            //            diffValueOfRed,
            //            diffValueOfGreen,
            //            diffValueOfBlue,
            //            diffValueOfYellow,
            //            diffValueOfPurple,
            //        };

            //        PuyoType type = GetPuyoType(diffs);
            //        if (type != PuyoType.NONE)
            //        {
            //            typeCounts[type]++;
            //        }
            //    }
            //}

            //if (typeCounts.Max(pair => pair.Value) < 32)
            //{
            //    return PuyoType.NONE;
            //}
            //PuyoType p = (from n in typeCounts
            //              where n.Value == (typeCounts.Max(pair => pair.Value))
            //              select n.Key).First();
            //return p;
        }

        /// <summary>同じぷよタイプであると見なす類似値の閾値</summary>
        const int SIMILARITY_THRESHOLD = int.MaxValue;
        private PuyoType GetPuyoType(int[,,] pattern)
        {
            PuyoType similarType = PuyoType.NONE;
            int minSimilarityValue = int.MaxValue;
            for (int typeIndex = (int)PuyoType.AKA; typeIndex <= (int)PuyoType.MURASAKI; typeIndex++)
            {
                PuyoType type = (PuyoType)typeIndex;
                int similarityValue = GetSimilarityValue(type, pattern);
                Debug.Write(type + ":" + similarityValue + " ");
                if (minSimilarityValue > similarityValue) {
                    minSimilarityValue = similarityValue;
                    similarType = type;
                }
            }
            Debug.WriteLine("【" + similarType + ":" + minSimilarityValue + "】");

            if (minSimilarityValue >= SIMILARITY_THRESHOLD)
            {
                return PuyoType.NONE;
            }
            else
            {
                return similarType;
            }
        }

        private int GetSimilarityValue(PuyoType puyoType, int[,,] pattern)
        {
            // サンプルと実測値との距離を計算
            // 各色分布の差を2乗した総和値を距離とする。（TODO: 重みづけをして近い色なら距離が短くなるようなこともした方がよいかも）
            int similarityValue = 0;
            int[,,] sample = colorSamples[puyoType];
            for (int ri = 0; ri < COLOR_DIVISION_NUM; ri++)
            {
                for (int gi = 0; gi < COLOR_DIVISION_NUM; gi++)
                {
                    for (int bi = 0; bi < COLOR_DIVISION_NUM; bi++)
                    {
                        int diff = sample[ri, gi, bi] - pattern[ri, gi, bi];
                        similarityValue += (diff * diff);
                    }
                }
            }

            return similarityValue;
        }

        private int[,,] DetectPattern(RapidBitmapAccessor ba, Rectangle rect)
        {
            int[,,] patterns = new int[COLOR_DIVISION_NUM, COLOR_DIVISION_NUM, COLOR_DIVISION_NUM];
            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                for (int y = rect.Y; y < CaptureField.UNIT + rect.Height; y++)
                {
                    // RGBから色番号を決定
                    Color c = ba.GetPixel(x, y);
                    int ri = c.R / (256 / COLOR_DIVISION_NUM);
                    int gi = c.G / (256 / COLOR_DIVISION_NUM);
                    int bi = c.B / (256 / COLOR_DIVISION_NUM);

                    patterns[ri, gi, bi]++;
                }
            }

            return patterns;
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
            colorSamples[puyoType] = DetectPattern(ba, new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = CaptureField.UNIT,
                Height = CaptureField.UNIT
            });

#if DEBUG
            Debug.WriteLine("サンプルデータセット(ぷよ種別 = " + puyoType + ")");
            for (int ri = 0; ri < COLOR_DIVISION_NUM; ri++)
            {
                for (int gi = 0; gi < COLOR_DIVISION_NUM; gi++)
                {
                    for (int bi = 0; bi < COLOR_DIVISION_NUM; bi++)
                    {
                        int r = ri * (256 / COLOR_DIVISION_NUM);
                        int g = gi * (256 / COLOR_DIVISION_NUM);
                        int b = bi * (256 / COLOR_DIVISION_NUM);
                        Debug.WriteLine(colorSamples[puyoType][ri, gi, bi] + ": RGB(" + r + ", " + g + ", " + b + ")");
                    }
                }
            }
            Debug.Flush();
#endif
        }
    }
}
