
namespace Cubokta.Puyo.Common
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
