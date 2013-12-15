/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/license/LICENSE-MIT.txt
 */
using Cubokta.Puyo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo
{
    /// <summary>
    /// 譜情報クラス
    /// </summary>
    public class Steps
    {
        /// <summary>譜情報</summary>
        private List<PairPuyo> steps = new List<PairPuyo>();
        
        /// <summary>Fコードエンコーダ</summary>
        private FCodeEncoder encoder = new FCodeEncoder();

        /// <summary>手数</summary>
        public int Count
        {
            get
            {
                return steps.Count;
            }
        }

        /// <summary>
        /// 譜を追加する
        /// </summary>
        /// <param name="pp">追加する譜</param>
        public void Add(PairPuyo pp)
        {
            steps.Add(pp);
        }

        /// <summary>
        /// 譜情報をFコードにエンコードする
        /// </summary>
        /// <returns>Fコード</returns>
        public string Encode()
        {
            return encoder.Encode(steps);
        }

        /// <summary>
        /// 初手の配色パターンを取得する
        /// </summary>
        /// <param name="num">手数</param>
        /// <returns>初手の配色パターン</returns>
        public String GetFirstStepsPattern(int num)
        {
            FirstStepAnalyzer firstStepAnalyzer = new FirstStepAnalyzer();
            if (steps.Count < num)
            {
                return firstStepAnalyzer.GetPattern(steps);
            }
            else
            {
                return firstStepAnalyzer.GetPattern(steps, num);
            }
        }
    }
}
