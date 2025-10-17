using System;
using System.Collections.Generic;
using System.Linq;
using WPEngine.Expressions;
using WPEngine.Parser;
using WPEngine.Statements;
using WPEngine.Loggs;
using Xunit;

namespace WPEngine.Tests
{
    /// <summary>
    /// Тесты для класса ConstantExpression, проверяющие создание и работу с константными выражениями
    /// </summary>
    public class ConstantExpressionTests
    {
        /// <summary>
        /// Проверяет корректность установки значения константы при создании
        /// </summary>
        [Fact]
        public void Constructor_ValidValue_SetsProperty()
        {
            // Arrange & Act
            var constant = new ConstantExpression(5.5);

            // Assert
            Assert.Equal(5.5, constant.Value);
        }

        /// <summary>
        /// Проверяет, что подстановка в константе возвращает клон, а не оригинальный объект
        /// </summary>
        [Fact]
        public void Substitute_ReturnsClone()
        {
            // Arrange
            var constant = new ConstantExpression(10.0);
            var replacement = new ConstantExpression(20.0);

            // Act
            var result = constant.Substitute("x", replacement);

            // Assert
            Assert.Equal(10.0, ((ConstantExpression)result).Value);
            Assert.NotSame(constant, result); // Should be a clone
        }

        /// <summary>
        /// Проверяет, что константные выражения не требуют дополнительных условий определенности
        /// </summary>
        [Fact]
        public void GetDefinitenessConditions_ReturnsEmptyList()
        {
            // Arrange
            var constant = new ConstantExpression(15.0);

            // Act
            var conditions = constant.GetDefinitenessConditions();

            // Assert
            Assert.Empty(conditions);
        }

        /// <summary>
        /// Проверяет строковое представление константы в инвариантной культуре
        /// </summary>
        [Fact]
        public void ToString_ReturnsInvariantCulture()
        {
            // Arrange
            var constant = new ConstantExpression(3.14);

            // Act
            var result = constant.ToString();

            // Assert
            Assert.Equal("3.14", result);
        }

        /// <summary>
        /// Проверяет создание глубокой копии константного выражения
        /// </summary>
        [Fact]
        public void Clone_CreatesEqualButDifferentInstance()
        {
            // Arrange
            var original = new ConstantExpression(7.5);

            // Act
            var clone = original.Clone();

            // Assert
            Assert.Equal(original.Value, ((ConstantExpression)clone).Value);
            Assert.NotSame(original, clone);
        }

        /// <summary>
        /// Проверяет равенство константных выражений с одинаковыми значениями
        /// </summary>
        [Fact]
        public void Equals_SameValue_ReturnsTrue()
        {
            // Arrange
            var constant1 = new ConstantExpression(5.0);
            var constant2 = new ConstantExpression(5.0);

            // Act & Assert
            Assert.True(constant1.Equals(constant2));
        }

        /// <summary>
        /// Проверяет неравенство константных выражений с разными значениями
        /// </summary>
        [Fact]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            // Arrange
            var constant1 = new ConstantExpression(5.0);
            var constant2 = new ConstantExpression(10.0);

            // Act & Assert
            Assert.False(constant1.Equals(constant2));
        }
    }

    /// <summary>
    /// Тесты для класса VariableExpression, проверяющие создание и работу с переменными выражениями
    /// </summary>
    public class VariableExpressionTests
    {
        /// <summary>
        /// Проверяет корректность установки имени переменной при создании
        /// </summary>
        [Fact]
        public void Constructor_ValidName_SetsProperty()
        {
            // Arrange & Act
            var variable = new VariableExpression("x");

            // Assert
            Assert.Equal("x", variable.Name);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании переменной с невалидным именем
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_InvalidName_ThrowsArgumentException(string name)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new VariableExpression(name));
        }

        /// <summary>
        /// Проверяет корректную подстановку выражения при совпадении имени переменной
        /// </summary>
        [Fact]
        public void Substitute_MatchingVariable_ReturnsReplacement()
        {
            // Arrange
            var variable = new VariableExpression("x");
            var replacement = new ConstantExpression(5.0);

            // Act
            var result = variable.Substitute("x", replacement);

            // Assert
            Assert.Equal(5.0, ((ConstantExpression)result).Value);
        }

        /// <summary>
        /// Проверяет, что подстановка не происходит при несовпадении имени переменной
        /// </summary>
        [Fact]
        public void Substitute_NonMatchingVariable_ReturnsClone()
        {
            // Arrange
            var variable = new VariableExpression("y");
            var replacement = new ConstantExpression(5.0);

            // Act
            var result = variable.Substitute("x", replacement);

            // Assert
            Assert.Equal("y", ((VariableExpression)result).Name);
            Assert.NotSame(variable, result);
        }

        /// <summary>
        /// Проверяет, что переменные выражения не требуют дополнительных условий определенности
        /// </summary>
        [Fact]
        public void GetDefinitenessConditions_ReturnsEmptyList()
        {
            // Arrange
            var variable = new VariableExpression("x");

            // Act
            var conditions = variable.GetDefinitenessConditions();

            // Assert
            Assert.Empty(conditions);
        }

        /// <summary>
        /// Проверяет строковое представление переменной выражения
        /// </summary>
        [Fact]
        public void ToString_ReturnsVariableName()
        {
            // Arrange
            var variable = new VariableExpression("myVar");

            // Act
            var result = variable.ToString();

            // Assert
            Assert.Equal("myVar", result);
        }

        /// <summary>
        /// Проверяет равенство переменных выражений с одинаковыми именами
        /// </summary>
        [Fact]
        public void Equals_SameName_ReturnsTrue()
        {
            // Arrange
            var var1 = new VariableExpression("x");
            var var2 = new VariableExpression("x");

            // Act & Assert
            Assert.True(var1.Equals(var2));
        }

        /// <summary>
        /// Проверяет неравенство переменных выражений с разными именами
        /// </summary>
        [Fact]
        public void Equals_DifferentName_ReturnsFalse()
        {
            // Arrange
            var var1 = new VariableExpression("x");
            var var2 = new VariableExpression("y");

            // Act & Assert
            Assert.False(var1.Equals(var2));
        }
    }

    /// <summary>
    /// Тесты для класса BinaryExpression, проверяющие создание и работу с бинарными выражениями
    /// </summary>
    public class BinaryExpressionTests
    {
        /// <summary>
        /// Проверяет корректность установки свойств при создании бинарного выражения
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            var left = new ConstantExpression(5.0);
            var right = new ConstantExpression(10.0);

            // Act
            var binary = new BinaryExpression(left, right, "+");

            // Assert
            Assert.Same(left, binary.Left);
            Assert.Same(right, binary.Right);
            Assert.Equal("+", binary.Operator);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с null левым операндом
        /// </summary>
        [Fact]
        public void Constructor_NullLeft_ThrowsArgumentNullException()
        {
            // Arrange
            var right = new ConstantExpression(10.0);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BinaryExpression(null, right, "+"));
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с null правым операндом
        /// </summary>
        [Fact]
        public void Constructor_NullRight_ThrowsArgumentNullException()
        {
            // Arrange
            var left = new ConstantExpression(5.0);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BinaryExpression(left, null, "+"));
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с невалидным оператором
        /// </summary>
        [Theory]
        [InlineData("invalid")]
        [InlineData("^")]
        [InlineData("%")]
        public void Constructor_InvalidOperator_ThrowsArgumentException(string op)
        {
            // Arrange
            var left = new ConstantExpression(5.0);
            var right = new ConstantExpression(10.0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new BinaryExpression(left, right, op));
        }

        /// <summary>
        /// Проверяет корректное создание бинарных выражений со всеми поддерживаемыми операторами
        /// </summary>
        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">")]
        [InlineData(">=")]
        [InlineData("<")]
        [InlineData("<=")]
        [InlineData("&&")]
        [InlineData("||")]
        public void Constructor_ValidOperators_DoesNotThrow(string op)
        {
            // Arrange
            var left = new ConstantExpression(5.0);
            var right = new ConstantExpression(10.0);

            // Act & Assert
            var exception = Record.Exception(() => new BinaryExpression(left, right, op));
            Assert.Null(exception);
        }

        /// <summary>
        /// Проверяет рекурсивную подстановку в обоих операндах бинарного выражения
        /// </summary>
        [Fact]
        public void Substitute_RecursivelySubstitutesBothOperands()
        {
            // Arrange
            var left = new VariableExpression("x");
            var right = new VariableExpression("y");
            var binary = new BinaryExpression(left, right, "+");
            var replacement = new ConstantExpression(5.0);

            // Act
            var result = binary.Substitute("x", replacement);

            // Assert
            var resultBinary = (BinaryExpression)result;
            Assert.Equal(5.0, ((ConstantExpression)resultBinary.Left).Value);
            Assert.Equal("y", ((VariableExpression)resultBinary.Right).Name);
            Assert.Equal("+", resultBinary.Operator);
        }

        /// <summary>
        /// Проверяет добавление условия ненулевого знаменателя для операции деления
        /// </summary>
        [Fact]
        public void GetDefinitenessConditions_DivisionOperator_AddsDenominatorCondition()
        {
            // Arrange
            var left = new ConstantExpression(10.0);
            var right = new VariableExpression("y");
            var binary = new BinaryExpression(left, right, "/");

            // Act
            var conditions = binary.GetDefinitenessConditions();

            // Assert
            Assert.Single(conditions);
            var condition = (BinaryExpression)conditions[0];
            Assert.Equal("y", ((VariableExpression)condition.Left).Name);
            Assert.Equal(0.0, ((ConstantExpression)condition.Right).Value);
            Assert.Equal("!=", condition.Operator);
        }

        /// <summary>
        /// Проверяет отсутствие специальных условий для не-деления операторов
        /// </summary>
        [Fact]
        public void GetDefinitenessConditions_NonDivisionOperator_NoSpecialConditions()
        {
            // Arrange
            var left = new ConstantExpression(10.0);
            var right = new VariableExpression("y");
            var binary = new BinaryExpression(left, right, "+");

            // Act
            var conditions = binary.GetDefinitenessConditions();

            // Assert
            Assert.Empty(conditions); // Only gets conditions from operands, which are constants and variables
        }

        /// <summary>
        /// Проверяет форматирование логических операторов с круглыми скобками
        /// </summary>
        [Fact]
        public void ToString_LogicalOperator_AddsParentheses()
        {
            // Arrange
            var left = new VariableExpression("a");
            var right = new VariableExpression("b");
            var binary = new BinaryExpression(left, right, "&&");

            // Act
            var result = binary.ToString();

            // Assert
            Assert.Equal("(a && b)", result);
        }

        /// <summary>
        /// Проверяет форматирование арифметических операторов без круглых скобок
        /// </summary>
        [Fact]
        public void ToString_ArithmeticOperator_NoParentheses()
        {
            // Arrange
            var left = new VariableExpression("a");
            var right = new VariableExpression("b");
            var binary = new BinaryExpression(left, right, "+");

            // Act
            var result = binary.ToString();

            // Assert
            Assert.Equal("a + b", result);
        }

        /// <summary>
        /// Проверяет равенство бинарных выражений с одинаковыми операндами и оператором
        /// </summary>
        [Fact]
        public void Equals_SameExpression_ReturnsTrue()
        {
            // Arrange
            var left = new ConstantExpression(5.0);
            var right = new ConstantExpression(10.0);
            var binary1 = new BinaryExpression(left, right, "+");
            var binary2 = new BinaryExpression(left.Clone(), right.Clone(), "+");

            // Act & Assert
            Assert.True(binary1.Equals(binary2));
        }

        /// <summary>
        /// Проверяет неравенство бинарных выражений с разными операторами
        /// </summary>
        [Fact]
        public void Equals_DifferentOperator_ReturnsFalse()
        {
            // Arrange
            var left = new ConstantExpression(5.0);
            var right = new ConstantExpression(10.0);
            var binary1 = new BinaryExpression(left, right, "+");
            var binary2 = new BinaryExpression(left, right, "-");

            // Act & Assert
            Assert.False(binary1.Equals(binary2));
        }
    }

    /// <summary>
    /// Тесты для класса UnaryExpression, проверяющие создание и работу с унарными выражениями
    /// </summary>
    public class UnaryExpressionTests
    {
        /// <summary>
        /// Проверяет корректность установки свойств при создании унарного выражения
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            var operand = new ConstantExpression(5.0);

            // Act
            var unary = new UnaryExpression(operand, "!");

            // Assert
            Assert.Same(operand, unary.Operand);
            Assert.Equal("!", unary.Operator);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с null операндом
        /// </summary>
        [Fact]
        public void Constructor_NullOperand_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UnaryExpression(null, "!"));
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с невалидным оператором
        /// </summary>
        [Theory]
        [InlineData("invalid")]
        [InlineData("+")]
        [InlineData("-")]
        public void Constructor_InvalidOperator_ThrowsArgumentException(string op)
        {
            // Arrange
            var operand = new ConstantExpression(5.0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new UnaryExpression(operand, op));
        }

        /// <summary>
        /// Проверяет корректное создание унарных выражений со всеми поддерживаемыми операторами
        /// </summary>
        [Theory]
        [InlineData("!")]
        [InlineData("abs")]
        public void Constructor_ValidOperators_DoesNotThrow(string op)
        {
            // Arrange
            var operand = new ConstantExpression(5.0);

            // Act & Assert
            var exception = Record.Exception(() => new UnaryExpression(operand, op));
            Assert.Null(exception);
        }

        /// <summary>
        /// Проверяет рекурсивную подстановку в операнде унарного выражения
        /// </summary>
        [Fact]
        public void Substitute_RecursivelySubstitutesOperand()
        {
            // Arrange
            var operand = new VariableExpression("x");
            var unary = new UnaryExpression(operand, "!");
            var replacement = new ConstantExpression(5.0);

            // Act
            var result = unary.Substitute("x", replacement);

            // Assert
            var resultUnary = (UnaryExpression)result;
            Assert.Equal(5.0, ((ConstantExpression)resultUnary.Operand).Value);
            Assert.Equal("!", resultUnary.Operator);
        }

        /// <summary>
        /// Проверяет получение условий определенности от операнда унарного выражения
        /// </summary>
        [Fact]
        public void GetDefinitenessConditions_ReturnsOperandConditions()
        {
            // Arrange
            var operand = new BinaryExpression(
                new ConstantExpression(10.0),
                new VariableExpression("y"),
                "/"
            );
            var unary = new UnaryExpression(operand, "abs");

            // Act
            var conditions = unary.GetDefinitenessConditions();

            // Assert
            Assert.Single(conditions); // Should get the division condition from operand
        }

        /// <summary>
        /// Проверяет форматирование оператора abs с круглыми скобками
        /// </summary>
        [Fact]
        public void ToString_AbsOperator_FormatsWithParentheses()
        {
            // Arrange
            var operand = new VariableExpression("x");
            var unary = new UnaryExpression(operand, "abs");

            // Act
            var result = unary.ToString();

            // Assert
            Assert.Equal("abs(x)", result);
        }

        /// <summary>
        /// Проверяет форматирование оператора отрицания с круглыми скобками
        /// </summary>
        [Fact]
        public void ToString_NotOperator_FormatsWithParentheses()
        {
            // Arrange
            var operand = new VariableExpression("x");
            var unary = new UnaryExpression(operand, "!");

            // Act
            var result = unary.ToString();

            // Assert
            Assert.Equal("!(x)", result);
        }
    }

    /// <summary>
    /// Тесты для класса Assignment, проверяющие создание и расчет слабейшего предусловия для присваивания
    /// </summary>
    public class AssignmentTests
    {
        /// <summary>
        /// Проверяет корректность установки свойств при создании оператора присваивания
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            var value = new ConstantExpression(5.0);

            // Act
            var assignment = new Assignment("x", value);

            // Assert
            Assert.Equal("x", assignment.VariableName);
            Assert.Same(value, assignment.Value);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с невалидным именем переменной
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_InvalidVariableName_ThrowsArgumentException(string name)
        {
            // Arrange
            var value = new ConstantExpression(5.0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Assignment(name, value));
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с null значением
        /// </summary>
        [Fact]
        public void Constructor_NullValue_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Assignment("x", null));
        }

        /// <summary>
        /// Проверяет добавление условий определенности при вычислении WP для присваивания с делением
        /// </summary>
        [Fact]
        public void CalculateWP_WithDivision_AddsDefinitenessConditions()
        {
            // Arrange
            var value = new BinaryExpression(
                new ConstantExpression(10.0),
                new VariableExpression("y"),
                "/"
            );
            var assignment = new Assignment("x", value);
            var postCondition = new VariableExpression("x");
            var tracker = new StepTracker();

            // Act
            var result = assignment.CalculateWP(postCondition, tracker);

            // Assert
            // Should be: (y != 0) && (10 / y)
            var binaryResult = (BinaryExpression)result;
            Assert.Equal("&&", binaryResult.Operator);

            var leftCondition = (BinaryExpression)binaryResult.Left;
            Assert.Equal("!=", leftCondition.Operator);

            Assert.True(tracker.HasSteps);
        }

        /// <summary>
        /// Проверяет корректную подстановку переменной при вычислении WP для простого присваивания
        /// </summary>
        [Fact]
        public void CalculateWP_SimpleAssignment_SubstitutesVariable()
        {
            // Arrange
            var value = new ConstantExpression(5.0);
            var assignment = new Assignment("x", value);
            var postCondition = new BinaryExpression(
                new VariableExpression("x"),
                new ConstantExpression(10.0),
                ">"
            );

            // Act
            var result = assignment.CalculateWP(postCondition);

            // Assert
            // Should be: 5 > 10
            var binaryResult = (BinaryExpression)result;
            Assert.Equal(5.0, ((ConstantExpression)binaryResult.Left).Value);
            Assert.Equal(10.0, ((ConstantExpression)binaryResult.Right).Value);
            Assert.Equal(">", binaryResult.Operator);
        }

        /// <summary>
        /// Проверяет строковое представление оператора присваивания
        /// </summary>
        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var value = new ConstantExpression(5.0);
            var assignment = new Assignment("myVar", value);

            // Act
            var result = assignment.ToString();

            // Assert
            Assert.Equal("myVar := 5", result);
        }
    }

    /// <summary>
    /// Тесты для класса IfStatement, проверяющие создание и расчет слабейшего предусловия для условного оператора
    /// </summary>
    public class IfStatementTests
    {
        /// <summary>
        /// Проверяет корректность установки свойств при создании условного оператора
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            var condition = new VariableExpression("x");
            var thenBranch = new Assignment("y", new ConstantExpression(1.0));
            var elseBranch = new Assignment("y", new ConstantExpression(0.0));

            // Act
            var ifStatement = new IfStatement(condition, thenBranch, elseBranch);

            // Assert
            Assert.Same(condition, ifStatement.Condition);
            Assert.Same(thenBranch, ifStatement.ThenBranch);
            Assert.Same(elseBranch, ifStatement.ElseBranch);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании с null условием
        /// </summary>
        [Fact]
        public void Constructor_NullCondition_ThrowsArgumentNullException()
        {
            // Arrange
            var thenBranch = new Assignment("y", new ConstantExpression(1.0));
            var elseBranch = new Assignment("y", new ConstantExpression(0.0));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new IfStatement(null, thenBranch, elseBranch));
        }

        /// <summary>
        /// Проверяет корректное построение логического выражения при вычислении WP для условного оператора
        /// </summary>
        [Fact]
        public void CalculateWP_CreatesCorrectLogicalExpression()
        {
            // Arrange
            var condition = new VariableExpression("x");
            var thenBranch = new Assignment("y", new ConstantExpression(1.0));
            var elseBranch = new Assignment("y", new ConstantExpression(0.0));
            var ifStatement = new IfStatement(condition, thenBranch, elseBranch);
            var postCondition = new BinaryExpression(
                new VariableExpression("y"),
                new ConstantExpression(0.0),
                ">"
            );
            var tracker = new StepTracker();

            // Act
            var result = ifStatement.CalculateWP(postCondition, tracker);

            // Assert
            // Should be: (x && (1 > 0)) || (!x && (0 > 0))
            var orExpression = (BinaryExpression)result;
            Assert.Equal("||", orExpression.Operator);

            var thenPart = (BinaryExpression)orExpression.Left;
            Assert.Equal("&&", thenPart.Operator);

            var elsePart = (BinaryExpression)orExpression.Right;
            Assert.Equal("&&", elsePart.Operator);

            Assert.True(tracker.HasSteps);
        }

        /// <summary>
        /// Проверяет строковое представление условного оператора
        /// </summary>
        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var condition = new VariableExpression("x");
            var thenBranch = new Assignment("y", new ConstantExpression(1.0));
            var elseBranch = new Assignment("y", new ConstantExpression(0.0));
            var ifStatement = new IfStatement(condition, thenBranch, elseBranch);

            // Act
            var result = ifStatement.ToString();

            // Assert
            Assert.Equal("if (x) then { y := 1 } else { y := 0 }", result);
        }
    }

    /// <summary>
    /// Тесты для класса Sequence, проверяющие создание и расчет слабейшего предусловия для последовательности операторов
    /// </summary>
    public class SequenceTests
    {
        /// <summary>
        /// Проверяет корректность установки свойств при создании последовательности
        /// </summary>
        [Fact]
        public void Constructor_ValidStatements_SetsProperties()
        {
            // Arrange
            var statements = new List<Statement>
            {
                new Assignment("x", new ConstantExpression(1.0)),
                new Assignment("y", new ConstantExpression(2.0))
            };

            // Act
            var sequence = new Sequence(statements);

            // Assert
            Assert.Equal(2, sequence.Statements.Count);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании пустой последовательности
        /// </summary>
        [Fact]
        public void Constructor_EmptyList_ThrowsArgumentException()
        {
            // Arrange
            var statements = new List<Statement>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Sequence(statements));
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании последовательности с null списком
        /// </summary>
        [Fact]
        public void Constructor_NullList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Sequence(null));
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при создании последовательности с null операторами
        /// </summary>
        [Fact]
        public void Constructor_ListWithNull_ThrowsArgumentException()
        {
            // Arrange
            var statements = new List<Statement>
            {
                new Assignment("x", new ConstantExpression(1.0)),
                null
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Sequence(statements));
        }

        /// <summary>
        /// Проверяет корректное вычисление WP для последовательности операторов в обратном порядке
        /// </summary>
        [Fact]
        public void CalculateWP_ProcessesStatementsInReverseOrder()
        {
            // Arrange
            var statements = new List<Statement>
            {
                new Assignment("x", new ConstantExpression(1.0)),
                new Assignment("y", new VariableExpression("x"))
            };
            var sequence = new Sequence(statements);
            var postCondition = new BinaryExpression(
                new VariableExpression("y"),
                new ConstantExpression(0.0),
                ">"
            );
            var tracker = new StepTracker();

            // Act
            var result = sequence.CalculateWP(postCondition, tracker);

            // Assert
            // Should be: wp(x := 1, wp(y := x, y > 0)) = wp(x := 1, x > 0) = 1 > 0
            var binaryResult = (BinaryExpression)result;
            Assert.Equal(1.0, ((ConstantExpression)binaryResult.Left).Value);
            Assert.Equal(0.0, ((ConstantExpression)binaryResult.Right).Value);
            Assert.Equal(">", binaryResult.Operator);

            Assert.True(tracker.HasSteps);
        }

        /// <summary>
        /// Проверяет строковое представление последовательности операторов
        /// </summary>
        [Fact]
        public void ToString_ReturnsSemicolonSeparatedStatements()
        {
            // Arrange
            var statements = new List<Statement>
            {
                new Assignment("x", new ConstantExpression(1.0)),
                new Assignment("y", new ConstantExpression(2.0))
            };
            var sequence = new Sequence(statements);

            // Act
            var result = sequence.ToString();

            // Assert
            Assert.Equal("x := 1; y := 2", result);
        }
    }

    /// <summary>
    /// Тесты для класса StepTracker, проверяющие запись и управление шагами вычислений
    /// </summary>
    public class StepTrackerTests
    {
        /// <summary>
        /// Проверяет корректную запись шага с валидным описанием
        /// </summary>
        [Fact]
        public void RecordStep_ValidDescription_AddsStep()
        {
            // Arrange
            var tracker = new StepTracker();
            var description = "Test step";

            // Act
            tracker.RecordStep(description);

            // Assert
            var steps = tracker.GetSteps();
            Assert.Single(steps);
            Assert.Contains("1. Test step", steps[0]);
        }

        /// <summary>
        /// Проверяет выбрасывание исключения при записи шага с невалидным описанием
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void RecordStep_InvalidDescription_ThrowsArgumentException(string description)
        {
            // Arrange
            var tracker = new StepTracker();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => tracker.RecordStep(description));
        }

        /// <summary>
        /// Проверяет получение списка записанных шагов в виде read-only коллекции
        /// </summary>
        [Fact]
        public void GetSteps_ReturnsReadOnlyList()
        {
            // Arrange
            var tracker = new StepTracker();
            tracker.RecordStep("Step 1");
            tracker.RecordStep("Step 2");

            // Act
            var steps = tracker.GetSteps();

            // Assert
            Assert.Equal(2, steps.Count);
            Assert.Contains("1. Step 1", steps[0]);
            Assert.Contains("2. Step 2", steps[1]);
        }

        /// <summary>
        /// Проверяет корректную очистку всех записанных шагов
        /// </summary>
        [Fact]
        public void Clear_RemovesAllSteps()
        {
            // Arrange
            var tracker = new StepTracker();
            tracker.RecordStep("Step 1");
            tracker.RecordStep("Step 2");

            // Act
            tracker.Clear();

            // Assert
            Assert.Empty(tracker.GetSteps());
            Assert.False(tracker.HasSteps);
        }

        /// <summary>
        /// Проверяет наличие шагов при их наличии в трекере
        /// </summary>
        [Fact]
        public void HasSteps_WithSteps_ReturnsTrue()
        {
            // Arrange
            var tracker = new StepTracker();
            tracker.RecordStep("Step 1");

            // Act & Assert
            Assert.True(tracker.HasSteps);
        }

        /// <summary>
        /// Проверяет отсутствие шагов при пустом трекере
        /// </summary>
        [Fact]
        public void HasSteps_WithoutSteps_ReturnsFalse()
        {
            // Arrange
            var tracker = new StepTracker();

            // Act & Assert
            Assert.False(tracker.HasSteps);
        }
    }
}