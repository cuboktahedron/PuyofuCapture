/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using System.Collections.Generic;
namespace Cubokta.Puyo.Common
{
    /// <summary>
    /// ぷよ種別ユーティリティ
    /// </summary>
    public class PuyoTypeUtil
    {
        /// <summary>
        /// ぷよ種別名への変換テーブル
        /// </summary>
        private static readonly IDictionary<PuyoType, string> TYPE2CHAR = new Dictionary<PuyoType, string>()
        {
            { PuyoType.NONE, "空白" },
            { PuyoType.AKA, "赤ぷよ" },
            { PuyoType.MIDORI, "緑ぷよ" },
            { PuyoType.AO, "青ぷよ" },
            { PuyoType.KI, "黄ぷよ" },
            { PuyoType.MURASAKI, "紫ぷよ" },
        };
        
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

        /// <summary>
        /// ぷよ種別名を返却する
        /// </summary>
        /// <param name="type">ぷよ種別</param>
        /// <returns>ぷよ種別の名前</returns>
        public static string GetTypeName(PuyoType type)
        {
            return TYPE2CHAR[type];
        }
    
    }
}
