using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Cubokta.Puyo.Common
{
    [TestClass]
    public class FirstStepAnalyzerTest
    {
        private FirstStepAnalyzer analyzer = new FirstStepAnalyzer();

        [TestMethod]
        public void GetPattern()
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            Assert.AreEqual("", analyzer.GetPattern(steps));

            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.AKA,
                Satellite = PuyoType.MIDORI,
            });
            Assert.AreEqual("AB", analyzer.GetPattern(steps));
            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.MIDORI,
                Satellite = PuyoType.KI,
            });
            Assert.AreEqual("ABAC", analyzer.GetPattern(steps));

            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.AKA,
                Satellite = PuyoType.KI,
            });
            Assert.AreEqual("ABACBC", analyzer.GetPattern(steps));

            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.AKA,
                Satellite = PuyoType.KI,
            });
            Assert.AreEqual("ABAC", analyzer.GetPattern(steps, 2));

        }

        [TestMethod]
        public void GetPattern2()
        {
            List<PairPuyo> steps = new List<PairPuyo>();
            Assert.AreEqual("", analyzer.GetPattern(steps));

            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.AKA,
                Satellite = PuyoType.AKA,
            });
            Assert.AreEqual("AA", analyzer.GetPattern(steps));
            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.MIDORI,
                Satellite = PuyoType.MIDORI,
            });
            Assert.AreEqual("AABB", analyzer.GetPattern(steps));

            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.AKA,
                Satellite = PuyoType.AKA,
            });
            Assert.AreEqual("AABBAA", analyzer.GetPattern(steps));
        }

        [TestMethod]
        public void GetPattern3()
        {
            List<PairPuyo> steps = new List<PairPuyo>();

            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.KI,
                Satellite = PuyoType.MIDORI,
            });
            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.KI,
                Satellite = PuyoType.MIDORI,
            });
            steps.Add(new ColorPairPuyo()
            {
                Pivot = PuyoType.AKA,
                Satellite = PuyoType.AKA,
            });
            Assert.AreEqual("ABABCC", analyzer.GetPattern(steps));
        }

    }
}
