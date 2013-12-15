/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/license/LICENSE-MIT.txt
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Cubokta.Common.game;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class FCodeEncoderTest
    {
        private FCodeEncoder encoder = new FCodeEncoder();

        [TestMethod]
        public void 譜情報からFコードが生成されること()
        {
            Assert.AreEqual("_", encoder.Encode(new List<PairPuyo>()));
            Assert.AreEqual("_58hu4AbGlX", encoder.Encode(new List<PairPuyo>()
            {
                CreateColorPuyo(PuyoType.AKA     , PuyoType.MIDORI, Direction4.UP   , 1),
                CreateColorPuyo(PuyoType.AO      , PuyoType.KI    , Direction4.LEFT , 3),
                CreateColorPuyo(PuyoType.MURASAKI, PuyoType.AKA   , Direction4.DOWN , 4),
                CreateColorPuyo(PuyoType.MIDORI  , PuyoType.AO    , Direction4.RIGHT, 5),
                new OjamaPairPuyo() {
                        OjamaRow = 3,
                        OjamaBit = new BitArray(new bool[] { true, false, true, false, true, false })
                },
            }));
        }

        private ColorPairPuyo CreateColorPuyo(PuyoType satellite, PuyoType pivot, Direction4 dir, int pos)
        {
            return new ColorPairPuyo()
            {
                Pivot = pivot,
                Satellite = satellite,
                Dir = dir,
                Pos = pos,
            };
        }
    }
}
