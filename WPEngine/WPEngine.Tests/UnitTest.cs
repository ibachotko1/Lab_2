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
    /// ����� ��� ������ ConstantExpression, ����������� �������� � ������ � ������������ �����������
    /// </summary>
    public class ConstantExpressionTests
    {
        /// <summary>
        /// ��������� ������������ ��������� �������� ��������� ��� ��������
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
        /// ���������, ��� ����������� � ��������� ���������� ����, � �� ������������ ������
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
        /// ���������, ��� ����������� ��������� �� ������� �������������� ������� ��������������
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
        /// ��������� ��������� ������������� ��������� � ������������ ��������
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
        /// ��������� �������� �������� ����� ������������ ���������
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
        /// ��������� ��������� ����������� ��������� � ����������� ����������
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
        /// ��������� ����������� ����������� ��������� � ������� ����������
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
    /// ����� ��� ������ VariableExpression, ����������� �������� � ������ � ����������� �����������
    /// </summary>
    public class VariableExpressionTests
    {
        /// <summary>
        /// ��������� ������������ ��������� ����� ���������� ��� ��������
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
        /// ��������� ������������ ���������� ��� �������� ���������� � ���������� ������
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
        /// ��������� ���������� ����������� ��������� ��� ���������� ����� ����������
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
        /// ���������, ��� ����������� �� ���������� ��� ������������ ����� ����������
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
        /// ���������, ��� ���������� ��������� �� ������� �������������� ������� ��������������
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
        /// ��������� ��������� ������������� ���������� ���������
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
        /// ��������� ��������� ���������� ��������� � ����������� �������
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
        /// ��������� ����������� ���������� ��������� � ������� �������
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
    /// ����� ��� ������ BinaryExpression, ����������� �������� � ������ � ��������� �����������
    /// </summary>
    public class BinaryExpressionTests
    {
        /// <summary>
        /// ��������� ������������ ��������� ������� ��� �������� ��������� ���������
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
        /// ��������� ������������ ���������� ��� �������� � null ����� ���������
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
        /// ��������� ������������ ���������� ��� �������� � null ������ ���������
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
        /// ��������� ������������ ���������� ��� �������� � ���������� ����������
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
        /// ��������� ���������� �������� �������� ��������� �� ����� ��������������� �����������
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
        /// ��������� ����������� ����������� � ����� ��������� ��������� ���������
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
        /// ��������� ���������� ������� ���������� ����������� ��� �������� �������
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
        /// ��������� ���������� ����������� ������� ��� ��-������� ����������
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
        /// ��������� �������������� ���������� ���������� � �������� ��������
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
        /// ��������� �������������� �������������� ���������� ��� ������� ������
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
        /// ��������� ��������� �������� ��������� � ����������� ���������� � ����������
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
        /// ��������� ����������� �������� ��������� � ������� �����������
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
    /// ����� ��� ������ UnaryExpression, ����������� �������� � ������ � �������� �����������
    /// </summary>
    public class UnaryExpressionTests
    {
        /// <summary>
        /// ��������� ������������ ��������� ������� ��� �������� �������� ���������
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
        /// ��������� ������������ ���������� ��� �������� � null ���������
        /// </summary>
        [Fact]
        public void Constructor_NullOperand_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UnaryExpression(null, "!"));
        }

        /// <summary>
        /// ��������� ������������ ���������� ��� �������� � ���������� ����������
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
        /// ��������� ���������� �������� ������� ��������� �� ����� ��������������� �����������
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
        /// ��������� ����������� ����������� � �������� �������� ���������
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
        /// ��������� ��������� ������� �������������� �� �������� �������� ���������
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
        /// ��������� �������������� ��������� abs � �������� ��������
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
        /// ��������� �������������� ��������� ��������� � �������� ��������
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
    /// ����� ��� ������ Assignment, ����������� �������� � ������ ���������� ����������� ��� ������������
    /// </summary>
    public class AssignmentTests
    {
        /// <summary>
        /// ��������� ������������ ��������� ������� ��� �������� ��������� ������������
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
        /// ��������� ������������ ���������� ��� �������� � ���������� ������ ����������
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
        /// ��������� ������������ ���������� ��� �������� � null ���������
        /// </summary>
        [Fact]
        public void Constructor_NullValue_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Assignment("x", null));
        }

        /// <summary>
        /// ��������� ���������� ������� �������������� ��� ���������� WP ��� ������������ � ��������
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
        /// ��������� ���������� ����������� ���������� ��� ���������� WP ��� �������� ������������
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
        /// ��������� ��������� ������������� ��������� ������������
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
    /// ����� ��� ������ IfStatement, ����������� �������� � ������ ���������� ����������� ��� ��������� ���������
    /// </summary>
    public class IfStatementTests
    {
        /// <summary>
        /// ��������� ������������ ��������� ������� ��� �������� ��������� ���������
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
        /// ��������� ������������ ���������� ��� �������� � null ��������
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
        /// ��������� ���������� ���������� ����������� ��������� ��� ���������� WP ��� ��������� ���������
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
        /// ��������� ��������� ������������� ��������� ���������
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
    /// ����� ��� ������ Sequence, ����������� �������� � ������ ���������� ����������� ��� ������������������ ����������
    /// </summary>
    public class SequenceTests
    {
        /// <summary>
        /// ��������� ������������ ��������� ������� ��� �������� ������������������
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
        /// ��������� ������������ ���������� ��� �������� ������ ������������������
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
        /// ��������� ������������ ���������� ��� �������� ������������������ � null �������
        /// </summary>
        [Fact]
        public void Constructor_NullList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Sequence(null));
        }

        /// <summary>
        /// ��������� ������������ ���������� ��� �������� ������������������ � null �����������
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
        /// ��������� ���������� ���������� WP ��� ������������������ ���������� � �������� �������
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
        /// ��������� ��������� ������������� ������������������ ����������
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
    /// ����� ��� ������ StepTracker, ����������� ������ � ���������� ������ ����������
    /// </summary>
    public class StepTrackerTests
    {
        /// <summary>
        /// ��������� ���������� ������ ���� � �������� ���������
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
        /// ��������� ������������ ���������� ��� ������ ���� � ���������� ���������
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
        /// ��������� ��������� ������ ���������� ����� � ���� read-only ���������
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
        /// ��������� ���������� ������� ���� ���������� �����
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
        /// ��������� ������� ����� ��� �� ������� � �������
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
        /// ��������� ���������� ����� ��� ������ �������
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