using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta
{
    public class FCodeEncoder
    {
        /// <summary>エンコードに使用する文字</summary>
        private const string ENCODE_CHAR = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ[]";

        private static readonly IDictionary<PuyoType, int> PUYO_TYPE_CONV = new Dictionary<PuyoType, int>()
        {
            { PuyoType.AKA     , 0 },
            { PuyoType.MIDORI  , 1 },
            { PuyoType.AO      , 2 },
            { PuyoType.KI      , 3 },
            { PuyoType.MURASAKI, 4 },
        };

        private static readonly IDictionary<Direction, int> DIR_CONV = new Dictionary<Direction, int>()
        {
            { Direction.UP   , 0 },
            { Direction.RIGHT, 1 },
            { Direction.DOWN , 2 },
            { Direction.LEFT , 3 },
        };

        public string Encode(List<PairPuyo> steps)
        {
            List<int> stepValues = new List<int>();
            int oneData = 0;
            foreach (PairPuyo step in steps) {
                if (step.IsOjama)
                {
                    oneData = (7 << 9);
                    oneData |= ((step.OjamaRow << 6) | BinaryToDecimal(step.OjamaBit));
                }
                else
                {
                    // 1手のデータを 0～124 (7bit)の数値で表す(ただし多ツモ非対応のため、実質0～24)
                    oneData = PUYO_TYPE_CONV[step[0]] * 5 + PUYO_TYPE_CONV[step[1]];
                    
                    // 上位5bitで軸ぷよの位置と方向を表す
                    oneData |= (((step.Pos << 2) + DIR_CONV[step.Dir]) << 7);
                }

                stepValues.Add(oneData);
            }

            return "_" + ConvertValueToFcode(stepValues);
        }

        private int BinaryToDecimal(BitArray bits)
        {
            int value = 0;
            for (int i = bits.Length - 1; i >= 0; i--)
            {
                value <<= 1;
                value |= bits[i] ? 1 : 0;
            }

            return value;
        }

        private string ConvertValueToFcode(List<int> stepValues)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int stepValue in stepValues)
            {
                int v1 = (stepValue) & 0x3f;
                int v2 = (stepValue >> 6) & 0x3f;
                sb.Append(ENCODE_CHAR[v1]);
                sb.Append(ENCODE_CHAR[v2]);
            }

            return sb.ToString();
        }
    }
}
