/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
namespace Cubokta.Puyo.Common
{
    /// <summary>
    /// フィールド関係の定数
    /// </summary>
    public class FieldConst {
        public const int OJAMA_ROW_MAX = 5;
        public const int FIELD_X = 6;
        public const int FIELD_Y = 12;
    }

    /// <summary>
    /// ぷよ種別
    /// </summary>
    public enum PuyoType {
        NONE = 0,
        AKA,
        MIDORI,
        AO,
        KI,
        MURASAKI,
        OJAMA,
    }
}
