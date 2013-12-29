/*
 * Copyright (c) 2013 cuboktahedron
 * Released under the MIT license
 * https://github.com/cuboktahedron/PuyofuCapture/blob/master/license/LICENSE-MIT.txt
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo.Common
{
    /// <summary>
    /// 初手解析機
    /// </summary>
    public class FirstStepAnalyzer
    {
        /// <summary>
        /// 初手の配色パターンを取得する
        /// </summary>
        /// <param name="steps">譜情報</param>
        /// <param name="stepNum">解析手数</param>
        /// <returns>初手の配色パターン</returns>
        public string GetPattern(List<PairPuyo> steps, int stepNum)
        {
            string pattern = "";
            char mapChar = 'A';
            IDictionary<PuyoType, char> mapping = new Dictionary<PuyoType, char>();

            List<List<PuyoType>> candidates = new List<List<PuyoType>>();
            for (int i = 0; i < stepNum; i++)
            {
                ColorPairPuyo cpp = (ColorPairPuyo)steps[i];
                List<PuyoType> priorList = new List<PuyoType>();
                List<PuyoType> posteriorList = new List<PuyoType>();

                // まずはまだパターン文字が決定していない色を
                // 優先順にごとに分類された候補リストに追加する。
                // 既に前の譜情報で出現している色は優先順位が高い。

                List<PuyoType> list;
                if (candidates.Count == 0)
                {
                    list = new List<PuyoType>();
                }
                else
                {
                    list = candidates[0];
                }

                char dummy;
                if (!mapping.TryGetValue(cpp.Pivot, out dummy))
                {
                    if (list.Contains(cpp.Pivot))
                    {
                        list.Remove(cpp.Pivot);
                        priorList.Add(cpp.Pivot);
                    }
                    else
                    {
                        posteriorList.Add(cpp.Pivot);
                    }
                }

                if (!mapping.TryGetValue(cpp.Satellite, out dummy) && cpp.Pivot != cpp.Satellite)
                {
                    if (list.Contains(cpp.Satellite))
                    {
                        list.Remove(cpp.Satellite);
                        priorList.Add(cpp.Satellite);
                    }
                    else
                    {
                        posteriorList.Add(cpp.Satellite);
                    }
                }

                candidates.Insert(0, priorList);
                candidates.Add(posteriorList);

                List<List<PuyoType>> removeTargets = new List<List<PuyoType>>();
                foreach (List<PuyoType> candidate in candidates)
                {
                    if (candidate.Count >= 2)
                    {
                        // マッピングが未確定のため、以降は処理しない
                        break;
                    }
                    else if (candidate.Count == 1)
                    {
                        if (!mapping.ContainsKey(candidate[0]))
                        {
                            // マッピングが確定
                            mapping[candidate[0]] = mapChar;
                            mapChar++;
                        }
                        removeTargets.Add(candidate);
                    }
                }

                candidates.RemoveAll((candidate) => candidate.Count == 0 || removeTargets.Contains(candidate));
            }

            // ここでも未確定の情報が残っていれば、出現順に文字を割り振る
            foreach (List<PuyoType> candidate in candidates)
            {
                foreach (PuyoType p in candidate)
                {
                    if (!mapping.ContainsKey(p))
                    {
                        mapping[p] = mapChar;
                        mapChar++;
                    }
                }
            }


            // パターン文字を組み立てる
            for (int i = 0; i < stepNum; i++)
            {
                ColorPairPuyo cpp = (ColorPairPuyo)steps[i];
                char c1 = mapping[cpp.Pivot];
                char c2 = mapping[cpp.Satellite];

                if (c1 < c2)
                {
                    pattern += ("" + c1 + c2);
                }
                else
                {
                    pattern += ("" + c2 + c1);
                }
            }

            return pattern;
        }

        /// <summary>
        /// 配色パターンを取得する
        /// 渡された全譜の配色パターンを解析し返却する
        /// </summary>
        /// <param name="steps">譜情報</param>
        /// <returns>配色パターン</returns>
        public string GetPattern(List<PairPuyo> steps)
        {
            return GetPattern(steps, steps.Count());
        }
    }
}
