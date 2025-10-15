using System;
using System.Collections.Generic;

namespace WPCalculator.Loggs
{
    /// <summary>
    /// Отслеживает и записывает шаги вычисления слабейшего предусловия
    /// Позволяет визуализировать процесс "протягивания" постусловия через программу
    /// </summary>
    public class StepTracker
    {
        private readonly List<string> _steps = new List<string>();

        /// <summary>
        /// Записывает шаг вычисления
        /// </summary>
        /// <param name="stepDescription">Описание шага</param>
        public void RecordStep(string stepDescription)
        {
            if (string.IsNullOrWhiteSpace(stepDescription))
                throw new ArgumentException("Описание шага не может быть пустым", nameof(stepDescription));

            _steps.Add($"{_steps.Count + 1}. {stepDescription}");
        }

        /// <summary>
        /// Возвращает все записанные шаги
        /// </summary>
        public IReadOnlyList<string> GetSteps() => _steps.AsReadOnly();

        /// <summary>
        /// Очищает историю шагов
        /// </summary>
        public void Clear() => _steps.Clear();

        /// <summary>
        /// Проверяет, есть ли записанные шаги
        /// </summary>
        public bool HasSteps => _steps.Count > 0;
    }
}
