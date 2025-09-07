using System.Collections.Generic;
using UnityEngine;

namespace GrowGame.Rules
{
    /// <summary>
    /// 규칙 평가기.
    /// - 버튼 클릭으로 기본 +1 후,
    /// - 규칙을 "변화가 멈출 때까지" 반복 적용(연쇄 반영)
    /// - setLevel은 선택지별 최대레벨로 클램프
    /// </summary>
    public static class RuleEngine
    {
        public static List<Rule> AllRules = new List<Rule>();

        /// <summary>
        /// 시작 시 1회 호출: CSV 로드
        /// </summary>
        public static void Init(string csvNameWithoutExt)
        {
            AllRules = RuleLoader.LoadRules(csvNameWithoutExt);
            Debug.Log($"[RuleEngine] 규칙 {AllRules.Count}개 로드 완료");
        }

        /// <summary>
        /// 규칙을 고정점까지 적용.
        /// </summary>
        public static void ApplyUntilStable(int[] choiceLevels, int[] maxLevels, int maxIterations = 8)
        {
            for (int iter = 0; iter < maxIterations; iter++)
            {
                bool changed = false;

                foreach (var rule in AllRules)
                {
                    if (Check(rule.conditions, choiceLevels))
                    {
                        foreach (var r in rule.results)
                        {
                            int cap = (maxLevels != null &&
                                       r.choiceIndex >= 0 && r.choiceIndex < maxLevels.Length)
                                      ? Mathf.Max(0, maxLevels[r.choiceIndex])
                                      : 99; // fallback

                            int old = choiceLevels[r.choiceIndex];
                            int nw = Mathf.Clamp(r.setLevel, 0, cap);

                            if (old != nw)
                            {
                                choiceLevels[r.choiceIndex] = nw;
                                changed = true;
                            }
                        }
                    }
                }

                // 더 이상 변화가 없으면 종료
                if (!changed) break;
            }
        }

        /// <summary>
        /// 모든 조건(AND)이 만족해야 true
        /// </summary>
        static bool Check(RuleCondition[] conditions, int[] lv)
        {
            foreach (var c in conditions)
            {
                int cur = lv[c.choiceIndex];
                switch (c.op)
                {
                    case RuleOp.Equal: if (cur != c.value) return false; break;
                    case RuleOp.GreaterEq: if (cur < c.value) return false; break;
                    case RuleOp.LessEq: if (cur > c.value) return false; break;
                    case RuleOp.NotEqual: if (cur == c.value) return false; break;
                }
            }
            return true;
        }
    }
}
