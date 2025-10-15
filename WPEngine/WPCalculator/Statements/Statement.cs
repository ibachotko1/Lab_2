using WPCalculator.Loggs;
using WPEngine.Expressions;

namespace WPCalculator.Statements
{
    /// <summary>
    /// Абстрактное представление оператора программы
    /// Оператор может быть присваиванием, последовательностью или условным ветвлением
    /// </summary>
    public abstract class Statement
    {
        /// <summary>
        /// Вычисляет слабейшее предусловие для данного оператора и постусловия
        /// </summary>
        /// <param name="postCondition">Постусловие, которое должно выполняться после оператора</param>
        /// <param name="stepTracker">Трекер для записи шагов вычисления (опционально)</param>
        /// <returns>Слабейшее предусловие, гарантирующее выполнение постусловия</returns>
        public abstract Expression CalculateWP(Expression postCondition, StepTracker stepTracker = null);

        /// <summary>
        /// Создает глубокую копию оператора
        /// </summary>
        public abstract Statement Clone();

        /// <summary>
        /// Преобразует оператор в строковое представление
        /// </summary>
        public abstract override string ToString();
    }
}
