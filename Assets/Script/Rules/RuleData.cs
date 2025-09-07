using System;
using UnityEngine;

namespace GrowGame.Rules
{
    /// <summary>
    /// 규칙의 비교 연산자 (=, >=, <=, !=)
    /// </summary>
    public enum RuleOp
    {
        Equal,      // =
        GreaterEq,  // >=
        LessEq,     // <=
        NotEqual    // !=
    }

    /// <summary>
    /// 단일 조건: "choiceIndex (op) value"
    /// 예: 3=2, 0>=1, 5!=4
    /// </summary>
    [Serializable]
    public class RuleCondition
    {
        public int choiceIndex;
        public RuleOp op;
        public int value;
    }

    /// <summary>
    /// 단일 결과: "choiceIndex = setLevel"
    /// 결과는 '특정 레벨로 설정'만 지원(+= 같은 가산은 추후 확장)
    /// </summary>
    [Serializable]
    public class RuleResult
    {
        public int choiceIndex;
        public int setLevel;
    }

    /// <summary>
    /// 하나의 규칙: 여러 조건(AND) → 여러 결과(동시 적용)
    /// </summary>
    [Serializable]
    public class Rule
    {
        public RuleCondition[] conditions;
        public RuleResult[] results;
    }
}
