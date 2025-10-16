using System;
using System.Collections.Generic;
using WPEngine.Expressions;
using WPEngine.Loggs;
using WPEngine.Expressions;

namespace WPEngine.Statements
{
    /// <summary>
    /// Представляет оператор присваивания переменной значения выражения
    /// Согласно правилу wp: wp(x := e, R) = R[x → e] + условия_определенности(e)
    /// </summary>
    public class Assignment : Statement
    {
        /// <summary>
        /// Имя переменной, которой присваивается значение
        /// </summary>
        public string VariableName { get; }

        /// <summary>
        /// Выражение, значение которого присваивается переменной
        /// </summary>
        public Expression Value { get; }

        /// <summary>
        /// Инициализирует новый оператор присваивания
        /// </summary>
        /// <param name="variableName">Имя переменной</param>
        /// <param name="value">Присваиваемое значение</param>
        /// <exception cref="ArgumentException">Выбрасывается при невалидном имени переменной</exception>
        public Assignment(string variableName, Expression value)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentException("Имя переменной не может быть пустым", nameof(variableName));

            VariableName = variableName.Trim();
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override Expression CalculateWP(Expression postCondition, StepTracker stepTracker = null)
        {
            // Правило wp для присваивания: подстановка переменной в постусловии
            var substituted = postCondition.Substitute(VariableName, Value);

            // Добавляем условия определенности для выражения
            var definitenessConditions = Value.GetDefinitenessConditions();
            var result = CombineWithDefinitenessConditions(substituted, definitenessConditions);

            // Логируем шаг если предоставлен трекер
            stepTracker?.RecordStep($"wp({this}, {postCondition}) = {result}");

            return result;
        }

        /// <summary>
        /// Объединяет основное условие с условиями определенности через конъюнкцию
        /// </summary>
        private Expression CombineWithDefinitenessConditions(Expression mainCondition, List<Expression> definitenessConditions)
        {
            if (definitenessConditions.Count == 0)
                return mainCondition;

            Expression result = mainCondition;

            foreach (var condition in definitenessConditions)
            {
                result = new BinaryExpression(condition, result, "&&");
            }

            return result;
        }

        public override Statement Clone()
        {
            return new Assignment(VariableName, Value.Clone());
        }

        public override string ToString()
        {
            return $"{VariableName} := {Value}";
        }

        /// <summary>
        /// Проверяет эквивалентность двух операторов присваивания
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Assignment other &&
                   VariableName == other.VariableName &&
                   Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(VariableName.GetHashCode(), Value.GetHashCode());
        }
    }
}
