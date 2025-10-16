using System;
using System.Collections.Generic;
using System.Globalization;
using WPEngine.Expressions;

namespace WPEngine.Expressions
{
    /// <summary>
    /// Представляет константное значение (числовую константу)
    /// </summary>
    public class ConstantExpression : Expression
    {
        /// <summary>
        /// Числовое значение константы
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Инициализирует новое константное выражение
        /// </summary>
        /// <param name="value">Числовое значение константы</param>
        public ConstantExpression(double value)
        {
            Value = value;
        }

        public override Expression Substitute(string variableName, Expression replacement)
        {
            // Константы не содержат переменных, возвращаем неизмененную копию
            return this.Clone();
        }

        public override List<Expression> GetDefinitenessConditions()
        {
            // Константы всегда определены, дополнительных условий не требуется
            return new List<Expression>();
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override Expression Clone()
        {
            return new ConstantExpression(Value);
        }

        /// <summary>
        /// Проверяет эквивалентность двух константных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is ConstantExpression other && Math.Abs(Value - other.Value) < 1e-10;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
