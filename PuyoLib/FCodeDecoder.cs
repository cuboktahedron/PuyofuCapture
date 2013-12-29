/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using Cubokta.Common.game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cubokta.Puyo.Common
{
    /// <summary>
    /// Fコードから譜情報を復元するデコーダ
    /// 
    /// Fコードとは対戦ぷよパーク！！(http://www.puyop.com/)のぷよぷよ連鎖シミュレータで使用されている
    /// URLパラメータとなるコードのこと。
    /// </summary>
    public class FCodeDecoder
    {
        /// <summary>デコードに使用する文字</summary>
        private const string DECODE_CHAR = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ[]";

        /// <summary>値⇒ぷよ種別の変換テーブル</summary>
        private static readonly IDictionary<int, PuyoType> PUYO_TYPE_CONV = new Dictionary<int, PuyoType>()
        {
            { 0, PuyoType.AKA      },
            { 1, PuyoType.MIDORI   },
            { 2, PuyoType.AO       },
            { 3, PuyoType.KI       },
            { 4, PuyoType.MURASAKI },
        };

        /// <summary>値⇒方向の変換テーブル</summary>
        private static readonly IDictionary<int, Direction4> DIR_CONV = new Dictionary<int, Direction4>()
        {
            { 0, Direction4.UP    },
            { 1, Direction4.RIGHT },
            { 2, Direction4.DOWN  },
            { 3, Direction4.LEFT  },
        };

        /// <summary>
        /// Fコードからぷよ譜情報を復元する
        /// </summary>
        /// <param name="fcode">Fコード</param>
        /// <returns>ぷよ譜情報</returns>
        public List<PairPuyo> Decode(string fcode)
        {
            if (fcode == "")
            {
                return new List<PairPuyo>();
            }

            // 先頭の'_'を取り除く
            fcode = fcode.Substring(1);
            List<int> stepValues = new List<int>();

            while (fcode.Count() > 0)
            {
                string oneFcode = fcode.Substring(0, fcode.Count() >= 2 ? 2 : 1);
                stepValues.Add(ConvertFcodeToValue(oneFcode));
                fcode = fcode.Substring(oneFcode.Length);
            }

            return ConvertValuesToSteps(stepValues);
        }

        /// <summary>
        /// Fコードを値に変換する
        /// </summary>
        /// <param name="step">Fコードの１ツモ分のコード(2文字)</param>
        /// <returns>Fコードに対応する値</returns>
        private int ConvertFcodeToValue(string step)
        {
            if (step.Count() == 1)
            {
                step += "0";
            }

            char c1 = step[0];
            char c2 = step[1];

            int value = DECODE_CHAR.IndexOf(c2);
            value <<= 6;
            value += DECODE_CHAR.IndexOf(c1);

            return value;
        }

        /// <summary>
        /// Fコード値を譜情報に変換する
        /// </summary>
        /// <param name="stepValues">Fコード値リスト</param>
        /// <returns>譜情報</returns>
        private List<PairPuyo> ConvertValuesToSteps(List<int> stepValues)
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            foreach (int value in stepValues)
            {
                int v1 = value >> 9;
                if (v1 == 7)
                {
                    // お邪魔ぷよ
                    OjamaPairPuyo ojama = new OjamaPairPuyo();
                    ojama.OjamaRow = (value >> 6) & 0x7;
                    ojama.OjamaBit = DecimalToBinary(value & 0x3f);

                    steps.Add(ojama);
                }
                else
                {
                    // 色ぷよ
                    ColorPairPuyo pp = new ColorPairPuyo();

                    int v2 = value & 0x3f;
                    int p2 = v2 % 5;
                    int p1 = (v2 - p2) / 5;
                    pp.Pivot = PUYO_TYPE_CONV[p1];
                    pp.Satellite = PUYO_TYPE_CONV[p2];
                    pp.Dir = DIR_CONV[((value >> 7) & 0x3)];
                    pp.Pos = v1;

                    steps.Add(pp);
                }

            }

            return steps;
        }

        /// <summary>
        /// Fコード値を2進数情報に変換する
        /// 2進数の各ビットはお邪魔ぷよの端数を表している。
        /// </summary>
        /// <param name="value">Fコード値</param>
        /// <returns>2進数情報</returns>
        private BitArray DecimalToBinary(int value)
        {
            BitArray bits = new BitArray(FieldConst.FIELD_X);
            for (int i = 0; i < bits.Length - 1; i++)
            {
                bits[i] = ((value & 1) == 1);
                value >>= 1;
            }

            return bits;
        }
    }
}
