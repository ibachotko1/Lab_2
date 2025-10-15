using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPEngine.Expressions;

namespace WPCalculator.Expressions
{
    /// <summary>
    /// Представляет унарную операцию
    /// Поддерживаемые операции: ! (логическое отрицание), abs() (модуль)
    /// </summary>
    public class UnaryExpression : Expression
    {
        /// <summary>
        /// Операнд унарной операции
        /// </summary>
        public Expression Operand { get; }

        /// <summary>
        /// Оператор унарной операции
        /// </summary>
        public string Operator { get; }

        private static readonly HashSet<string> ValidOperators = new HashSet<string>() { "!", "abs" };

        /// <summary>
        /// Инициализирует новое унарное выражение
        /// </summary>
        /// <param name="operand">Операнд</param>
        /// <param name="operator">Оператор</param>
        /// <exception cref="ArgumentException">Выбрасывается при невалидном операторе</exception>
        public UnaryExpression(Expression operand, string @operator)
        {
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));

            if (!ValidOperators.Contains(@operator))
                throw new ArgumentException($"Недопустимый унарный оператор: {@operator}", nameof(@operator));

            Operator = @operator;
        }

        public override Expression Substitute(string variableName, Expression replacement)
        {
            var newOperand = Operand.Substitute(variableName, replacement);
            return new UnaryExpression(newOperand, Operator);
        }

        public override List<Expression> GetDefinitenessConditions()
        {
            // Унарные операции всегда определены, но собираем условия из операнда
            return Operand.GetDefinitenessConditions();
        }

        public override string ToString()
        {
            return Operator == "abs"
                ? $"abs({Operand})"
                : $"{Operator}({Operand})";
        }

        public override Expression Clone()
        {
            return new UnaryExpression(Operand.Clone(), Operator);
        }

        /// <summary>
        /// Проверяет эквивалентность двух унарных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is UnaryExpression other &&
                   Operand.Equals(other.Operand) &&
                   Operator == other.Operator;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Operand.GetHashCode(), Operator.GetHashCode());
        }
    }
}
