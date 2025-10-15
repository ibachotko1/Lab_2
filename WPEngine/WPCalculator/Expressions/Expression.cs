using System.Collections.Generic;

namespace WPEngine.Expressions
{
    /// <summary>
    /// Абстрактное представление выражения в программе
    /// Выражение может быть переменной, константой или бинарной операцией
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// Выполняет подстановку переменной в выражении
        /// </summary>
        /// <param name="variableName">Имя заменяемой переменной</param>
        /// <param name="replacement">Выражение, на которое заменяется переменная</param>
        /// <returns>Новое выражение с выполненной подстановкой</returns>
        public abstract Expression Substitute(string variableName, Expression replacement);

        /// <summary>
        /// Собирает условия определенности для выражения (например, знаменатель ≠ 0)
        /// </summary>
        /// <returns>Список условий, которые должны выполняться для корректного вычисления</returns>
        public abstract List<Expression> GetDefinitenessConditions();

        /// <summary>
        /// Преобразует выражение в строковое представление
        /// </summary>
        public abstract override string ToString();

        /// <summary>
        /// Создает глубокую копию выражения
        /// </summary>
        public abstract Expression Clone();
    }
}
