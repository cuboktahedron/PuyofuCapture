﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubokta.Puyo.Common
{
    public class FirstStepAnalyzer
    {
        public string GetPattern(List<PairPuyo> steps, int stepNum)
        {
            // TODO: ここはリファクタリングする

            string pattern = "";
            IDictionary<PuyoType, char> mapping = new Dictionary<PuyoType, char>();
            char mapChar = 'A';
            List<List<PuyoType>> candidates = new List<List<PuyoType>>();
            for (int i = 0; i < stepNum; i++)
            {
                ColorPairPuyo cpp = (ColorPairPuyo)steps[i];
                char dummy;
                if (candidates.Count == 0)
                {
                    List<PuyoType> priorList = new List<PuyoType>();
                    if (!mapping.TryGetValue(cpp.Pivot, out dummy))
                    {
                        priorList.Add(cpp.Pivot);
                    }

                    if (!mapping.TryGetValue(cpp.Satellite, out dummy) && cpp.Pivot != cpp.Satellite)
                    {
                        priorList.Add(cpp.Satellite);
                    }
                    candidates.Add(priorList);
                }
                else
                {
                    List<PuyoType> priorList = new List<PuyoType>();
                    List<PuyoType> posteriorList = new List<PuyoType>();

                    List<PuyoType> list = candidates[0];
                    if (!mapping.TryGetValue(cpp.Pivot, out dummy))
                    {
                        if (list.Contains(cpp.Pivot))
                        {
                            list.Remove(cpp.Pivot);
                            priorList.Add(cpp.Pivot);
                        }
                        else
                        {
                            list.Remove(cpp.Pivot);
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
                            list.Remove(cpp.Satellite);
                            posteriorList.Add(cpp.Satellite);
                        }
                    }

                    candidates.Insert(0, priorList);
                    candidates.Add(posteriorList);
                }

                foreach (List<PuyoType> candidate in candidates)
                {
                    if (candidate.Count >= 2)
                    {
                        break;
                    }
                    else if (candidate.Count == 1)
                    {
                        mapping[candidate[0]] = mapChar;
                        mapChar++;
                    }
                }

                candidates.RemoveAll((candidate) => candidate.Count <= 1);
            }

            // ここでも未確定の情報が残っていれば、出現順に文字を割り振る
            foreach (List<PuyoType> candidate in candidates)
            {
                foreach (PuyoType p in candidate)
                {
                    mapping[p] = mapChar;
                    mapChar++;
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

        public string GetPattern(List<PairPuyo> steps)
        {
            return GetPattern(steps, steps.Count());
        }
    }
}
