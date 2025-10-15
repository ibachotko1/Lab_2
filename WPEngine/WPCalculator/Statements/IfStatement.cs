using System;
using WPCalculator.Expressions;
using WPCalculator.Loggs;
using WPEngine.Expressions;

namespace WPCalculator.Statements
{
    /// <summary>
    /// Представляет условный оператор (ветвление)
    /// Согласно правилу wp: wp(if B then S1 else S2, R) = (B ∧ wp(S1, R)) ∨ (¬B ∧ wp(S2, R))
    /// </summary>
    public class IfStatement : Statement
    {
        /// <summary>
        /// Условие ветвления
        /// </summary>
        public Expression Condition { get; }

        /// <summary>
        /// Оператор, выполняемый при истинности условия
        /// </summary>
        public Statement ThenBranch { get; }

        /// <summary>
        /// Оператор, выполняемый при ложности условия
        /// </summary>
        public Statement ElseBranch { get; }

        /// <summary>
        /// Инициализирует новый условный оператор
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <param name="thenBranch">Ветка "then"</param>
        /// <param name="elseBranch">Ветка "else"</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если условие или любая из веток null</exception>
        public IfStatement(Expression condition, Statement thenBranch, Statement elseBranch)
        {
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            ThenBranch = thenBranch ?? throw new ArgumentNullException(nameof(thenBranch));
            ElseBranch = elseBranch ?? throw new ArgumentNullException(nameof(elseBranch));
        }

        public override Expression CalculateWP(Expression postCondition, StepTracker stepTracker = null)
        {
            // Вычисляем wp для обеих веток
            var wpThen = ThenBranch.CalculateWP(postCondition, stepTracker);
            var wpElse = ElseBranch.CalculateWP(postCondition, stepTracker);

            // Строим выражение: (Condition && wpThen) || (!Condition && wpElse)
            var thenPart = new BinaryExpression(Condition.Clone(), wpThen, "&&");
            var notCondition = new UnaryExpression(Condition.Clone(), "!");
            var elsePart = new BinaryExpression(notCondition, wpElse, "&&");
            var result = new BinaryExpression(thenPart, elsePart, "||");

            stepTracker?.RecordStep($"wp(if {Condition} then {ThenBranch} else {ElseBranch}, {postCondition}) = {result}");

            return result;
        }

        public override Statement Clone()
        {
            return new IfStatement(
                Condition.Clone(),
                ThenBranch.Clone(),
                ElseBranch.Clone()
            );
        }

        public override string ToString()
        {
            return $"if ({Condition}) then {{ {ThenBranch} }} else {{ {ElseBranch} }}";
        }

        /// <summary>
        /// Проверяет эквивалентность двух условных операторов
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is IfStatement other &&
                   Condition.Equals(other.Condition) &&
                   ThenBranch.Equals(other.ThenBranch) &&
                   ElseBranch.Equals(other.ElseBranch);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Condition, ThenBranch, ElseBranch);
        }
    }
}
