using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cubokta;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class PuyoTypeUtilTest
    {
        [TestMethod]
        public void IsColorTest()
        {
            Assert.IsFalse(PuyoTypeUtil.IsColor(PuyoType.NONE));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.AKA));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.MIDORI));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.AO));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.KI));
            Assert.IsFalse(PuyoTypeUtil.IsColor(PuyoType.OJAMA));
        }

        [TestMethod]
        public void IsOjamaTest()
        {
            Assert.IsFalse(PuyoTypeUtil.IsOjama(PuyoType.NONE));
            Assert.IsFalse(PuyoTypeUtil.IsOjama(PuyoType.AKA));
            Assert.IsFalse(PuyoTypeUtil.IsOjama(PuyoType.MIDORI));
            Assert.IsFalse(PuyoTypeUtil.IsOjama(PuyoType.AO));
            Assert.IsFalse(PuyoTypeUtil.IsOjama(PuyoType.KI));
            Assert.IsTrue(PuyoTypeUtil.IsOjama(PuyoType.OJAMA));

        }
    }
}
