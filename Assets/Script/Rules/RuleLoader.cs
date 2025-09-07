using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace GrowGame.Rules
{
    /// <summary>
    /// Resources/rules.csv �� �о� Rule ����Ʈ�� ��ȯ
    /// CSV ����
    ///  - ���: ����,���
    ///  - ����: "indexopvalue" �� ; �� ����(��: "3=2;5>=1")
    ///  - ���: "index=value"   �� ; �� ����(��: "2=2;7=4")
    ///  - ������: =, >=, <=, != ����
    /// </summary>
    public static class RuleLoader
    {
        public static List<Rule> LoadRules(string fileNameWithoutExt)
        {
            var rules = new List<Rule>();

            // Resources.Load�� Ȯ���� ���� ���ϸ��� ����
            TextAsset csv = Resources.Load<TextAsset>(fileNameWithoutExt);
            if (csv == null)
            {
                Debug.LogError($"[RuleLoader] CSV ������ ã�� �� �����ϴ�: {fileNameWithoutExt}");
                return rules;
            }

            string[] lines = csv.text.Split('\n');
            if (lines.Length <= 1)
            {
                Debug.LogWarning("[RuleLoader] CSV ������ ����ֽ��ϴ�.");
                return rules;
            }

            // 0���� ���(����,���)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                // Į�� �и�: ����,���
                // ��ǥ ���� split (CSV Ư������ ó�� �ʿ�� ���� �ļ� ��ü)
                string[] parts = line.Split(',');
                if (parts.Length < 2) continue;

                string condStr = parts[0].Trim();
                string resStr = parts[1].Trim();

                var conds = ParseConditions(condStr);
                var ress = ParseResults(resStr);

                if (conds.Count == 0 || ress.Count == 0) continue;

                rules.Add(new Rule
                {
                    conditions = conds.ToArray(),
                    results = ress.ToArray()
                });
            }

            return rules;
        }

        static List<RuleCondition> ParseConditions(string condStr)
        {
            var list = new List<RuleCondition>();
            if (string.IsNullOrWhiteSpace(condStr)) return list;

            string[] tokens = condStr.Split(';');
            foreach (var t in tokens)
            {
                string s = t.Trim();
                if (string.IsNullOrEmpty(s)) continue;

                // ������ Ž�� ���� �߿�: >=, <=, != ���� �� �� ���� =
                RuleOp op;
                int idx = -1;

                if ((idx = s.IndexOf(">=", StringComparison.Ordinal)) >= 0)
                    op = RuleOp.GreaterEq;
                else if ((idx = s.IndexOf("<=", StringComparison.Ordinal)) >= 0)
                    op = RuleOp.LessEq;
                else if ((idx = s.IndexOf("!=", StringComparison.Ordinal)) >= 0)
                    op = RuleOp.NotEqual;
                else if ((idx = s.IndexOf('=')) >= 0)
                    op = RuleOp.Equal;
                else
                {
                    Debug.LogWarning($"[RuleLoader] ���� �Ľ� ����: {s}");
                    continue;
                }

                string left = s.Substring(0, idx).Trim();
                string right = s.Substring(idx + (op == RuleOp.Equal ? 1 : 2)).Trim();

                if (!int.TryParse(left, NumberStyles.Integer, CultureInfo.InvariantCulture, out int choiceIdx) ||
                    !int.TryParse(right, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val))
                {
                    Debug.LogWarning($"[RuleLoader] ���� ���� �Ľ� ����: {s}");
                    continue;
                }

                list.Add(new RuleCondition { choiceIndex = choiceIdx, op = op, value = val });
            }
            return list;
        }

        static List<RuleResult> ParseResults(string resStr)
        {
            var list = new List<RuleResult>();
            if (string.IsNullOrWhiteSpace(resStr)) return list;

            string[] tokens = resStr.Split(';');
            foreach (var t in tokens)
            {
                string s = t.Trim();
                if (string.IsNullOrEmpty(s)) continue;

                int idx = s.IndexOf('=');
                if (idx < 0)
                {
                    Debug.LogWarning($"[RuleLoader] ��� �Ľ� ����(= ����): {s}");
                    continue;
                }

                string left = s.Substring(0, idx).Trim();
                string right = s.Substring(idx + 1).Trim();

                if (!int.TryParse(left, out int choiceIdx) || !int.TryParse(right, out int lvl))
                {
                    Debug.LogWarning($"[RuleLoader] ��� ���� �Ľ� ����: {s}");
                    continue;
                }

                list.Add(new RuleResult { choiceIndex = choiceIdx, setLevel = Mathf.Max(0, lvl) });
            }
            return list;
        }
    }
}
