using System;
using System.Collections.Generic;
using System.Linq;
using WPCalculator.Loggs;
using WPEngine.Expressions;

namespace WPCalculator.Statements
{
    /// <summary>
    /// Представляет последовательность операторов (композицию)
    /// Согласно правилу wp: wp(S1; S2, R) = wp(S1, wp(S2, R))
    /// </summary>
    public class Sequence : Statement
    {
        /// <summary>
        /// Список операторов в последовательности
        /// </summary>
        public IReadOnlyList<Statement> Statements { get; }

        /// <summary>
        /// Инициализирует новую последовательность операторов
        /// </summary>
        /// <param name="statements">Список операторов</param>
        /// <exception cref="ArgumentException">Выбрасывается, если список операторов пуст или содержит null</exception>
        public Sequence(IEnumerable<Statement> statements)
        {
            if (statements == null)
                throw new ArgumentNullException(nameof(statements));

            var statementsList = statements.ToList();
            if (statementsList.Count == 0)
                throw new ArgumentException("Последовательность операторов не может быть пустой", nameof(statements));

            if (statementsList.Any(s => s == null))
                throw new ArgumentException("Последовательность операторов не может содержать null", nameof(statements));

            Statements = statementsList.AsReadOnly();
        }

        public override Expression CalculateWP(Expression postCondition, StepTracker stepTracker = null)
        {
            // Начинаем с конечного постусловия и проходим последовательность в обратном порядке
            Expression currentCondition = postCondition;

            // Проходим с последнего оператора к первому
            for (int i = Statements.Count - 1; i >= 0; i--)
            {
                var statement = Statements[i];
                currentCondition = statement.CalculateWP(currentCondition, stepTracker);
            }

            return currentCondition;
        }

        public override Statement Clone()
        {
            var clonedStatements = Statements.Select(s => s.Clone()).ToList();
            return new Sequence(clonedStatements);
        }

        public override string ToString()
        {
            return string.Join("; ", Statements);
        }

        /// <summary>
        /// Проверяет эквивалентность двух последовательностей
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Sequence other &&
                   Statements.SequenceEqual(other.Statements);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var statement in Statements)
            {
                hash.Add(statement);
            }
            return hash.ToHashCode();
        }
    }
}
