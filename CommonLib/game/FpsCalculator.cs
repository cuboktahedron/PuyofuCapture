/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/license/LICENSE-MIT.txt
 */
using System;

namespace Cubokta.Puyo
{
    /// <summary>
    /// FPS計算機
    /// </summary>
    /// <remarks>FPSの計算はミリ秒精度で行われる。</remarks>
    public class FpsCalculator
    {
        /// <summary>
        /// 周波数
        /// </summary>
        private const int FREQUENCY = 1000;
        private float fps;
        private int prevMills = DateTime.Now.Millisecond;

        /// <summary>
        /// FPSを更新する
        /// </summary>
        public void Refresh()
        {
            int curMills = DateTime.Now.Millisecond;
            if (curMills - prevMills > 0)
            {
                fps = FREQUENCY / (curMills - prevMills);
            }
            prevMills = curMills;
        }

        /// <summary>
        /// FPSを取得（整数）
        /// </summary>
        /// <returns>FPS</returns>
        public int GetFpsInt()
        {
            return (int)fps;
        }
    }
}
