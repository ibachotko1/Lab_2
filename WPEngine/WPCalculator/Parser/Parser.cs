using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPCalculator.Expressions;
using WPCalculator.Statements;
using WPEngine.Expressions;

namespace WPCalculator.Parser
{
    /// <summary>
    /// Парсер для преобразования строкового представления в объекты Expression и Statement
    /// </summary>
    public static class Parser
    {
        private static readonly Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>()
        {
            ["||"] = 1,
            ["&&"] = 2,
            ["=="] = 3,
            ["!="] = 3,
            [">"] = 4,
            [">="] = 4,
            ["<"] = 4,
            ["<="] = 4,
            ["+"] = 5,
            ["-"] = 5,
            ["*"] = 6,
            ["/"] = 6,
            ["!"] = 7,
            ["abs"] = 7 // унарные операции
        };

        /// <summary>
        /// Парсит выражение из строки
        /// </summary>
        /// <param name="input">Строка для парсинга</param>
        /// <returns>Распарсенное выражение</returns>
        /// <exception cref="FormatException">Выбрасывается при синтаксической ошибке</exception>
        public static Expression ParseExpression(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Входная строка не может быть пустой");

            var tokens = Tokenize(input);
            var reader = new TokenReader(tokens);
            return ParseExpression(reader, 0);
        }

        private static Expression ParseExpression(TokenReader reader, int minPrecedence)
        {
            // Парсим левый операнд (возможно, унарную операцию)
            Expression left = ParsePrimary(reader);

            while (true)
            {
                // Смотрим на следующий токен, если это бинарный оператор с достаточным приоритетом
                var operatorToken = reader.Peek();
                if (operatorToken == null || operatorToken.Type != TokenType.Operator)
                    break;

                string op = operatorToken.Value;
                if (!OperatorPrecedence.ContainsKey(op) || OperatorPrecedence[op] < minPrecedence)
                    break;

                // Потребляем оператор
                reader.Read();
                int nextMinPrecedence = OperatorPrecedence[op] + 1;

                // Парсим правый операнд
                Expression right = ParseExpression(reader, nextMinPrecedence);

                left = new BinaryExpression(left, right, op);
            }

            return left;
        }

        private static Expression ParsePrimary(TokenReader reader)
        {
            var token = reader.Read();
            if (token == null)
                throw new FormatException("Неожиданный конец выражения");

            switch (token.Type)
            {
                case TokenType.Number:
                    if (double.TryParse(token.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                        return new ConstantExpression(value);
                    throw new FormatException($"Неверный формат числа: {token.Value}");

                case TokenType.Identifier:
                    // Проверяем, не является ли это унарной операцией
                    if (token.Value == "abs")
                    {
                        // Ожидаем открывающую скобку
                        var next = reader.Read();
                        if (next == null || next.Type != TokenType.Punctuation || next.Value != "(")
                            throw new FormatException("Ожидалась '(' после 'abs'");

                        var inner = ParseExpression(reader, 0);
                        var close = reader.Read();
                        if (close == null || close.Type != TokenType.Punctuation || close.Value != ")")
                            throw new FormatException("Ожидалась ')' после выражения в 'abs'");

                        return new UnaryExpression(inner, "abs");
                    }
                    else if (token.Value == "!")
                    {
                        // Унарное отрицание
                        var inner = ParsePrimary(reader);
                        return new UnaryExpression(inner, "!");
                    }
                    else
                    {
                        return new VariableExpression(token.Value);
                    }

                case TokenType.Punctuation when token.Value == "(":
                    var expression = ParseExpression(reader, 0);
                    var closeParen = reader.Read();
                    if (closeParen == null || closeParen.Type != TokenType.Punctuation || closeParen.Value != ")")
                        throw new FormatException("Ожидалась ')'");
                    return expression;

                default:
                    throw new FormatException($"Неожиданный токен: {token.Value}");
            }
        }

        // Токенизация: разбиваем строку на токены
        private static List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int index = 0;

            while (index < input.Length)
            {
                char c = input[index];

                if (char.IsWhiteSpace(c))
                {
                    index++;
                    continue;
                }

                if (char.IsDigit(c) || c == '.')
                {
                    // Парсим число
                    int start = index;
                    while (index < input.Length && (char.IsDigit(input[index]) || input[index] == '.'))
                        index++;
                    tokens.Add(new Token(TokenType.Number, input.Substring(start, index - start)));
                    continue;
                }

                if (char.IsLetter(c))
                {
                    // Парсим идентификатор или ключевое слово
                    int start = index;
                    while (index < input.Length && (char.IsLetterOrDigit(input[index]) || input[index] == '_'))
                        index++;
                    tokens.Add(new Token(TokenType.Identifier, input.Substring(start, index - start)));
                    continue;
                }

                // Операторы и пунктуация
                if (c == '&' && index + 1 < input.Length && input[index + 1] == '&')
                {
                    tokens.Add(new Token(TokenType.Operator, "&&"));
                    index += 2;
                    continue;
                }

                if (c == '|' && index + 1 < input.Length && input[index + 1] == '|')
                {
                    tokens.Add(new Token(TokenType.Operator, "||"));
                    index += 2;
                    continue;
                }

                if (c == '=' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, "=="));
                    index += 2;
                    continue;
                }

                if (c == '!' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, "!="));
                    index += 2;
                    continue;
                }

                if (c == '>' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, ">="));
                    index += 2;
                    continue;
                }

                if (c == '<' && index + 1 < input.Length && input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.Operator, "<="));
                    index += 2;
                    continue;
                }

                // Одиночные символы операторов и пунктуации
                if (OperatorPrecedence.ContainsKey(c.ToString()))
                {
                    tokens.Add(new Token(TokenType.Operator, c.ToString()));
                    index++;
                    continue;
                }

                if (c == '(' || c == ')' || c == ';' || c == '{' || c == '}')
                {
                    tokens.Add(new Token(TokenType.Punctuation, c.ToString()));
                    index++;
                    continue;
                }

                throw new FormatException($"Неизвестный символ: {c}");
            }

            return tokens;
        }


        /// <summary>
        /// Парсит оператор из строки
        /// </summary>
        /// <param name="input">Строка для парсинга</param>
        /// <returns>Распарсенный оператор</returns>
        /// <exception cref="FormatException">Выбрасывается при синтаксической ошибке</exception>
        public static Statement ParseStatement(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Входная строка не может быть пустой");

            var tokens = Tokenize(input);
            var reader = new TokenReader(tokens);
            var statement = ParseStatement(reader);

            // Убедимся, что достигли конца
            if (reader.Peek() != null)
                throw new FormatException("Неожиданный токен в конце оператора");

            return statement;
        }

        private static Statement ParseStatement(TokenReader reader)
        {
            // Пробуем распарсить if
            var token = reader.Peek();
            if (token != null && token.Type == TokenType.Identifier && token.Value == "if")
            {
                return ParseIfStatement(reader);
            }

            // Иначе парсим присваивание или последовательность
            return ParseSequence(reader);
        }

        private static Statement ParseIfStatement(TokenReader reader)
        {
            // Потребляем "if"
            reader.Read();

            // Ожидаем условие в скобках
            var openParen = reader.Read();
            if (openParen == null || openParen.Type != TokenType.Punctuation || openParen.Value != "(")
                throw new FormatException("Ожидалась '(' после 'if'");

            var condition = ParseExpression(reader, 0);

            var closeParen = reader.Read();
            if (closeParen == null || closeParen.Type != TokenType.Punctuation || closeParen.Value != ")")
                throw new FormatException("Ожидалась ')' после условия if");

            // Парсим then ветку
            var thenBranch = ParseStatement(reader);

            // Проверяем, есть ли else
            var elseToken = reader.Peek();
            if (elseToken != null && elseToken.Type == TokenType.Identifier && elseToken.Value == "else")
            {
                reader.Read();
                var elseBranch = ParseStatement(reader);
                return new IfStatement(condition, thenBranch, elseBranch);
            }

            // Если else нет, то создаем пустой оператор для else ветки?
            // Но в задании указана полная форма if, так что ожидаем else.
            throw new FormatException("Ожидалось 'else'");
        }

        private static Statement ParseSequence(TokenReader reader)
        {
            var statements = new List<Statement>();

            while (true)
            {
                statements.Add(ParseSingleStatement(reader));

                var next = reader.Peek();
                if (next == null || next.Value != ";")
                    break;

                reader.Read(); // потребляем ';'
            }

            return statements.Count == 1 ? statements[0] : new Sequence(statements);
        }

        private static Statement ParseSingleStatement(TokenReader reader)
        {
            // Ожидаем присваивание: идентификатор, затем ":=", затем выражение
            var identifier = reader.Read();
            if (identifier == null || identifier.Type != TokenType.Identifier)
                throw new FormatException("Ожидался идентификатор");

            var assignOp = reader.Read();
            if (assignOp == null || assignOp.Type != TokenType.Operator || assignOp.Value != ":=")
                throw new FormatException("Ожидался оператор присваивания ':='");

            var expression = ParseExpression(reader, 0);
            return new Assignment(identifier.Value, expression);
        }
    }
}
