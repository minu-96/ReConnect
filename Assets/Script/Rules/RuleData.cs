using System;
using UnityEngine;

namespace GrowGame.Rules
{
    /// <summary>
    /// ��Ģ�� �� ������ (=, >=, <=, !=)
    /// </summary>
    public enum RuleOp
    {
        Equal,      // =
        GreaterEq,  // >=
        LessEq,     // <=
        NotEqual    // !=
    }

    /// <summary>
    /// ���� ����: "choiceIndex (op) value"
    /// ��: 3=2, 0>=1, 5!=4
    /// </summary>
    [Serializable]
    public class RuleCondition
    {
        public int choiceIndex;
        public RuleOp op;
        public int value;
    }

    /// <summary>
    /// ���� ���: "choiceIndex = setLevel"
    /// ����� 'Ư�� ������ ����'�� ����(+= ���� ������ ���� Ȯ��)
    /// </summary>
    [Serializable]
    public class RuleResult
    {
        public int choiceIndex;
        public int setLevel;
    }

    /// <summary>
    /// �ϳ��� ��Ģ: ���� ����(AND) �� ���� ���(���� ����)
    /// </summary>
    [Serializable]
    public class Rule
    {
        public RuleCondition[] conditions;
        public RuleResult[] results;
    }
}
