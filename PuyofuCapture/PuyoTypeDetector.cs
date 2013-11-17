﻿using Cubokta.Common;
using Cubokta.Puyo.Common;
using log4net;
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
        private static readonly ILog LOGGER =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>色識別用にRGBの各要素を分類する際に必要となるビット数</summary>
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

        public PuyoType Detect(RapidBitmapAccessor ba, Rectangle rect)
        {
            int[,,] pattern = DetectPattern(ba, rect);
            return GetPuyoType(pattern);
        }

        /// <summary>同じぷよタイプであると見なす類似値の閾値</summary>
        public int SimilarityThreshold { get; set; }

        /// <summary>同じぷよタイプであると見なす類似値の閾値</summary>
        private PuyoType GetPuyoType(int[,,] pattern)
        {
            PuyoType similarType = PuyoType.NONE;
            int minSimilarityValue = int.MaxValue;
            for (int typeIndex = (int)PuyoType.AKA; typeIndex <= (int)PuyoType.MURASAKI; typeIndex++)
            {
                PuyoType type = (PuyoType)typeIndex;
                int similarityValue = GetSimilarityValue(type, pattern);
//                LOGGER.Debug(type + ":" + similarityValue + " ");
                if (minSimilarityValue > similarityValue) {
                    minSimilarityValue = similarityValue;
                    similarType = type;
                }
            }

            if (minSimilarityValue >= SimilarityThreshold)
            {
//                LOGGER.Debug("【" + PuyoType.NONE + ":" + minSimilarityValue + "】"); 
                return PuyoType.NONE;
            }
            else
            {
//                LOGGER.Debug("【" + similarType + ":" + minSimilarityValue + "】"); 
                return similarType;
            }
        }
        
        private static readonly int[,] MASK = new int[CaptureField.UNIT, CaptureField.UNIT] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

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
            for (int x = rect.X; x < rect.X + rect.Width; x = x + 2)
            {
                for (int y = rect.Y; y < rect.Y + rect.Height; y = y + 2)
                {
                    if (MASK[x - rect.X, y - rect.Y] != 1)
                    {
                        // マスクされている個所は処理しない
                        continue;
                    }

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
        }
    }
}
