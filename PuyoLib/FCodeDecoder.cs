using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo.Common
{
    public class FCodeDecoder
    {
        /// <summary>デコードに使用する文字</summary>
        private const string DECODE_CHAR = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ[]";

        private static readonly IDictionary<int, PuyoType> PUYO_TYPE_CONV = new Dictionary<int, PuyoType>()
        {
            { 0, PuyoType.AKA      },
            { 1, PuyoType.MIDORI   },
            { 2, PuyoType.AO       },
            { 3, PuyoType.KI       },
            { 4, PuyoType.MURASAKI },
        };

        private static readonly IDictionary<int, Direction> DIR_CONV = new Dictionary<int, Direction>()
        {
            { 0, Direction.UP    },
            { 1, Direction.RIGHT },
            { 2, Direction.DOWN  },
            { 3, Direction.LEFT  },
        };

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
