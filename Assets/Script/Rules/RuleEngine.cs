using System.Collections.Generic;
using UnityEngine;

namespace GrowGame.Rules
{
    /// <summary>
    /// ��Ģ �򰡱�.
    /// - ��ư Ŭ������ �⺻ +1 ��,
    /// - ��Ģ�� "��ȭ�� ���� ������" �ݺ� ����(���� �ݿ�)
    /// - setLevel�� �������� �ִ뷹���� Ŭ����
    /// </summary>
    public static class RuleEngine
    {
        public static List<Rule> AllRules = new List<Rule>();

        /// <summary>
        /// ���� �� 1ȸ ȣ��: CSV �ε�
        /// </summary>
        public static void Init(string csvNameWithoutExt)
        {
            AllRules = RuleLoader.LoadRules(csvNameWithoutExt);
            Debug.Log($"[RuleEngine] ��Ģ {AllRules.Count}�� �ε� �Ϸ�");
        }

        /// <summary>
        /// ��Ģ�� ���������� ����.
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

                // �� �̻� ��ȭ�� ������ ����
                if (!changed) break;
            }
        }

        /// <summary>
        /// ��� ����(AND)�� �����ؾ� true
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
