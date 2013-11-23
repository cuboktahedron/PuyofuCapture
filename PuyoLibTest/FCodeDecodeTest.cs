using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Cubokta.Common.game;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class FCodeDecoderTest
    {
        private FCodeDecoder decoder = new FCodeDecoder();

        [TestMethod]
        public void Fコードから譜情報が復元されること()
        {
            Assert.AreEqual(0, decoder.Decode("_").Count);
            List<PairPuyo> steps = decoder.Decode("_58hu4AbGlX");
            List<PairPuyo> expected = new List<PairPuyo>()
            {
                CreateColorPuyo(PuyoType.AKA     , PuyoType.MIDORI, Direction4.UP   , 1),
                CreateColorPuyo(PuyoType.AO      , PuyoType.KI    , Direction4.LEFT , 3),
                CreateColorPuyo(PuyoType.MURASAKI, PuyoType.AKA   , Direction4.DOWN , 4),
                CreateColorPuyo(PuyoType.MIDORI  , PuyoType.AO    , Direction4.RIGHT, 5),
                new OjamaPairPuyo() {
                        OjamaRow = 3,
                        OjamaBit = new BitArray(new bool[] { true, false, true, false, true, false })
               }
            };
            Assert.AreEqual(expected.Count, steps.Count);
            for (int i = 0; i < steps.Count; i++)
            {
                if (!steps[i].Equals(expected[i]))
                {
                    Assert.Fail("Decodeされた譜が一致しません。");
                }
            }
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
