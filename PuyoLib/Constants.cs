using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo.Common
{
    public class FieldConst {
        public const int OJAMA_ROW_MAX = 5;
        public const int FIELD_X = 6;
        public const int FIELD_Y = 12;
    }

    public enum PuyoType {
        NONE = 0,
        AKA,
        MIDORI,
        AO,
        KI,
        MURASAKI,
        OJAMA,
    }

    public enum HandType {
        NONE = 0,
        TWO = 20,
        OJAMA = 99,
    }

    public enum Direction
    {
        NONE = -1,
        UP,
        LEFT,
        DOWN,
        RIGHT,
    }
}
