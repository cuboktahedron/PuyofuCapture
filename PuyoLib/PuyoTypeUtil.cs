
namespace Cubokta.Puyo.Common
{
    /// <summary>
    /// ぷよ種別ユーティリティ
    /// </summary>
    public class PuyoTypeUtil
    {
        /// <summary>
        /// 色ぷよかどうかを判定する
        /// </summary>
        /// <param name="type">ぷよ種別</param>
        /// <returns>色ぷよかどうか</returns>
        public static bool IsColor(PuyoType type)
        {
            return type.CompareTo(PuyoType.AKA) >= 0 && type.CompareTo(PuyoType.MURASAKI) <= 0;
        }

        /// <summary>
        /// お邪魔ぷよかどうかを判定する
        /// </summary>
        /// <param name="type">ぷよ種別</param>
        /// <returns>お邪魔ぷよかどうか</returns>
        public static bool IsOjama(PuyoType type)
        {
            return type == PuyoType.OJAMA;
        }
    }
}
