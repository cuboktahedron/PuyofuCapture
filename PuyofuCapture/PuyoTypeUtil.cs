using Cubokta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta
{
    public class PuyoTypeUtil
    {
        public static bool IsColor(PuyoType type)
        {
            return type.CompareTo(PuyoType.AKA) >= 0 && type.CompareTo(PuyoType.MURASAKI) <= 0;
        }

        public static bool IsOjama(PuyoType type)
        {
            return type == PuyoType.OJAMA;
        }
    }
}
