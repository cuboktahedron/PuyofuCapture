using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;

namespace Cubokta
{
    [TestClass]
    public class FCodeEncoderTest
    {
        private FCodeEncoder encoder = new FCodeEncoder();

        [TestMethod]
        public void EncodeTest()
        {
            Assert.AreEqual("_", encoder.Encode(new List<PairPuyo>()));
            Assert.AreEqual("_58hu4AbGlX", encoder.Encode(new List<PairPuyo>()
            {
                CreateColorPuyo(PuyoType.AKA     , PuyoType.MIDORI, Direction.UP   , 1),
                CreateColorPuyo(PuyoType.AO      , PuyoType.KI    , Direction.LEFT , 3),
                CreateColorPuyo(PuyoType.MURASAKI, PuyoType.AKA   , Direction.DOWN , 4),
                CreateColorPuyo(PuyoType.MIDORI  , PuyoType.AO    , Direction.RIGHT, 5),
                new OjamaPairPuyo() {
                        OjamaRow = 3,
                        OjamaBit = new BitArray(new bool[] { true, false, true, false, true, false })
                },
            }));
        }

        private ColorPairPuyo CreateColorPuyo(PuyoType satellite, PuyoType pivot, Direction dir, int pos)
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
