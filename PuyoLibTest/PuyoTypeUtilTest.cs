/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class PuyoTypeUtilTest
    {
        [TestMethod]
        public void 色ぷよはtrue_色ぷよ以外はfalseになること()
        {
            Assert.IsFalse(PuyoTypeUtil.IsColor(PuyoType.NONE));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.AKA));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.MIDORI));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.AO));
            Assert.IsTrue(PuyoTypeUtil.IsColor(PuyoType.KI));
            Assert.IsFalse(PuyoTypeUtil.IsColor(PuyoType.OJAMA));
        }

        [TestMethod]
        public void お邪魔ぷよはtrue_お邪魔ぷよ以外はfalseになること()
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
