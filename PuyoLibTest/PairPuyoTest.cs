using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class ColorPairPuyoTest
    {
        [TestMethod]
        public void ぷよタイプ取得設定の確認()
        {
            ColorPairPuyo pp = new ColorPairPuyo();
            pp[0] = PuyoType.AKA;
            pp[1] = PuyoType.MIDORI;
            Assert.AreEqual(PuyoType.AKA, pp[0]);
            Assert.AreEqual(PuyoType.MIDORI, pp[1]);
            Assert.AreEqual(PuyoType.AKA, pp.Pivot);
            Assert.AreEqual(PuyoType.MIDORI, pp.Satellite);

            pp.Pivot = PuyoType.KI;
            pp.Satellite = PuyoType.MURASAKI;
            Assert.AreEqual(PuyoType.KI, pp[0]);
            Assert.AreEqual(PuyoType.MURASAKI, pp[1]);
            Assert.AreEqual(PuyoType.KI, pp.Pivot);
            Assert.AreEqual(PuyoType.MURASAKI, pp.Satellite);
        }

        [TestMethod]
        public void お邪魔ぷよではないことを確認()
        {
            ColorPairPuyo pp = new ColorPairPuyo();
            Assert.IsFalse(pp.IsOjama);
        }
    }

    [TestClass]
    public class OjaamPairPuyoTest
    {
        [TestMethod]
        public void お邪魔ぷよではないことを確認()
        {
            OjamaPairPuyo pp = new OjamaPairPuyo();
            Assert.IsTrue(pp.IsOjama);
        }

        [TestMethod]
        public void お邪魔段数の取得設定の確認()
        {
            OjamaPairPuyo pp = new OjamaPairPuyo();
            Assert.AreEqual(0, pp.OjamaRow);

            pp.OjamaRow = 5;
            Assert.AreEqual(5, pp.OjamaRow);
        }

        [TestMethod]
        public void お邪魔数の確認()
        {
            OjamaPairPuyo pp = new OjamaPairPuyo();
            pp.OjamaRow = 3;
            pp.OjamaBit[0] = true;
            pp.OjamaBit[5] = false;
            Assert.AreEqual(4, pp.OjamaAt(0));
            Assert.AreEqual(3, pp.OjamaAt(4));
        }
    }
}
