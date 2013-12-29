/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class ColorPairPuyoTest
    {
        [TestMethod]
        public void ぷよ番号で設定したぷよ種別が取得できること()
        {
            ColorPairPuyo pp = new ColorPairPuyo();
            pp[0] = PuyoType.AKA;
            pp[1] = PuyoType.MIDORI;
            Assert.AreEqual(PuyoType.AKA, pp[0]);
            Assert.AreEqual(PuyoType.MIDORI, pp[1]);
            Assert.AreEqual(PuyoType.AKA, pp.Pivot);
            Assert.AreEqual(PuyoType.MIDORI, pp.Satellite);
        }

        [TestMethod]
        public void ぷよ名称で設定したぷよ種別が取得できること()
        {
            ColorPairPuyo pp = new ColorPairPuyo();
            pp.Pivot = PuyoType.KI;
            pp.Satellite = PuyoType.MURASAKI;
            Assert.AreEqual(PuyoType.KI, pp[0]);
            Assert.AreEqual(PuyoType.MURASAKI, pp[1]);
            Assert.AreEqual(PuyoType.KI, pp.Pivot);
            Assert.AreEqual(PuyoType.MURASAKI, pp.Satellite);
        }

        [TestMethod]
        public void お邪魔ぷよと判定されないこと()
        {
            ColorPairPuyo pp = new ColorPairPuyo();
            Assert.IsFalse(pp.IsOjama);
        }
    }

    [TestClass]
    public class OjaamPairPuyoTest
    {
        [TestMethod]
        public void お邪魔ぷよと判定されること()
        {
            OjamaPairPuyo pp = new OjamaPairPuyo();
            Assert.IsTrue(pp.IsOjama);
        }

        [TestMethod]
        public void 指定列のお邪魔数が正しく計算されること()
        {
            OjamaPairPuyo pp = new OjamaPairPuyo();
            pp.OjamaRow = 3;
            BitArray ojamaBit = pp.OjamaBit;
            ojamaBit[0] = true;
            ojamaBit[5] = false;
            pp.OjamaBit = ojamaBit;

            Assert.AreEqual(4, pp.OjamaAt(0));
            Assert.AreEqual(3, pp.OjamaAt(4));
        }
    }
}
