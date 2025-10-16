using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Backend — подключаем нужные пространства имён
using WPEngine.Parser;
using WPEngine.Loggs;

// Разрешаем конфликт имён: ваш Expression ≠ System.Windows.Expression
using Expression = WPEngine.Expressions.Expression;
using Statement = WPEngine.Statements.Statement;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Expression _postCondition;
        private Statement _parsedProgram;
        private Expression _finalWP;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCalculateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Парсим постусловие и программу
                _postCondition = Parser.ParseExpression(PostConditionInput.Text.Trim());
                _parsedProgram = Parser.ParseStatement(CodeInput.Text.Trim());

                // Вычисляем wp с трекингом шагов
                var stepTracker = new StepTracker();
                _finalWP = _parsedProgram.CalculateWP(_postCondition, stepTracker);

                // Отображаем результат
                DisplayResult();
                DisplaySteps(stepTracker.GetSteps());
                TriadButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка парсинга или вычисления",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                TriadButton.IsEnabled = false;
            }
        }

        private void DisplayResult()
        {
            ResultPanel.Children.Clear();

            ResultPanel.Children.Add(new TextBlock
            {
                Text = $"Итоговое предусловие (wp): {_finalWP}",
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            });

            ResultPanel.Children.Add(new TextBlock
            {
                Text = $"Человеческим языком: {PostHumanInput.Text}",
                TextWrapping = TextWrapping.Wrap,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 0, 0, 10)
            });
        }

        private void DisplaySteps(System.Collections.Generic.IReadOnlyList<string> steps)
        {
            StepsList.Items.Clear();
            foreach (var step in steps)
            {
                StepsList.Items.Add(step);
            }
        }

        private void OnShowTriadClick(object sender, RoutedEventArgs e)
        {
            if (_finalWP == null || _parsedProgram == null || _postCondition == null)
            {
                MessageBox.Show("Сначала рассчитайте wp.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string triad = $"{{ {_finalWP} }}\n\n{_parsedProgram}\n\n{{ {_postCondition} }}";
            string human = $"Если выполняется «{_finalWP}», то после выполнения программы «{_parsedProgram}» будет выполнено постусловие «{_postCondition}».";

            MessageBox.Show($"{triad}\n\nЧеловеческим языком:\n{human}",
                "Триада Хоара", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnPresetSelected(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem == null) return;

            var item = comboBox.SelectedItem as ComboBoxItem;
            if (item?.Content == null) return;

            string content = item.Content.ToString();
            if (content == "— Выберите пресет —") return;

            switch (content)
            {
                case "Max из двух":
                    PostConditionInput.Text = "max > 100";
                    PostHumanInput.Text = "Максимум больше 100";
                    CodeInput.Text = "if (x1 >= x2) { max := x1; } else { max := x2; }";
                    break;

                case "Квадратное уравнение (D)":
                    PostConditionInput.Text = "(D >= 0 && root == 1) || (D < 0 && root == -999)";
                    PostHumanInput.Text = "Если дискриминант неотрицателен, корень равен 1; иначе — специальное значение -999";
                    CodeInput.Text = "D := b * b - 4 * a * c; if (D >= 0) { root := 1; } else { root := -999; }";
                    break;

                case "Последовательность присваиваний":
                    PostConditionInput.Text = "y == x - 9 && x > 15";
                    PostHumanInput.Text = "y равен x минус 9, и x больше 15";
                    CodeInput.Text = "x := x + 10; y := x + 1;";
                    break;
            }
        }
    }
}