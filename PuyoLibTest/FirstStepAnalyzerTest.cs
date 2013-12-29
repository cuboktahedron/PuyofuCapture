/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class FirstStepAnalyzerTest
    {
        private FirstStepAnalyzer analyzer = new FirstStepAnalyzer();

        [TestMethod]
        public void 譜情報が空の場合空文字となること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            Assert.AreEqual("", analyzer.GetPattern(steps));
        }

        [TestMethod]
        public void ABACBCが生成されること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.MIDORI, PuyoType.KI));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.KI));

            Assert.AreEqual("ABACBC", analyzer.GetPattern(steps));
        }

        [TestMethod]
        public void 指定した手数分の初手パターンが生成されること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.MIDORI, PuyoType.KI));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.KI));

            Assert.AreEqual("ABAC", analyzer.GetPattern(steps, 2));
        }

        [TestMethod]
        public void AABBAAが生成されること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.AKA));
            steps.Add(CreateColors(PuyoType.MIDORI, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.AKA));

            Assert.AreEqual("AABBAA", analyzer.GetPattern(steps));
        }

        [TestMethod]
        public void ABABCCが生成されること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();

            steps.Add(CreateColors(PuyoType.KI, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.KI, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.AKA));
            Assert.AreEqual("ABABCC", analyzer.GetPattern(steps));
        }

        [TestMethod]
        public void ABCCACが生成されること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();

            steps.Add(CreateColors(PuyoType.KI, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.AKA));
            steps.Add(CreateColors(PuyoType.KI, PuyoType.AKA));
            Assert.AreEqual("ABCCAC", analyzer.GetPattern(steps));
        }

        [TestMethod]
        public void ABCCCCが生成されること()
        {
            List<PairPuyo> steps = new List<PairPuyo>();

            steps.Add(CreateColors(PuyoType.KI, PuyoType.MIDORI));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.AKA));
            steps.Add(CreateColors(PuyoType.AKA, PuyoType.AKA));
            Assert.AreEqual("ABCCCC", analyzer.GetPattern(steps));
        }

        private static ColorPairPuyo CreateColors(PuyoType pivot, PuyoType satellite)
        {
            return new ColorPairPuyo()
            {
                Pivot = pivot,
                Satellite = satellite,
            };
        }
    }
}
