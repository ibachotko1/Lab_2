using System;
using System.Collections.Generic;
using WPEngine.Expressions;

namespace WPEngine.Expressions
{
    /// <summary>
    /// Представляет переменную в выражении
    /// </summary>
    public class VariableExpression : Expression
    {
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Инициализирует новое выражение переменной
        /// </summary>
        /// <param name="name">Имя переменной</param>
        /// <exception cref="ArgumentException">Выбрасывается если имя null или пустое</exception>
        public VariableExpression(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя переменной не может быть пустым", nameof(name));

            Name = name.Trim();
        }

        public override Expression Substitute(string variableName, Expression replacement)
        {
            if (Name == variableName)
            {
                // Заменяем эту переменную на указанное выражение
                return replacement.Clone();
            }

            // Имя не совпадает - возвращаем неизмененную копию
            return this.Clone();
        }

        public override List<Expression> GetDefinitenessConditions()
        {
            // Переменные всегда определены (в контексте wp-вычислений)
            return new List<Expression>();
        }

        public override string ToString()
        {
            return Name;
        }

        public override Expression Clone()
        {
            return new VariableExpression(Name);
        }

        /// <summary>
        /// Проверяет эквивалентность двух переменных выражений
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is VariableExpression other && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
