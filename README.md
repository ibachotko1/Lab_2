# 🧪 ЛР-2: WPCalculator
WPCalculator - визуальный калькулятор слабейшего предусловия (wp) для анализа последовательности операторов и условных конструкций.

###📋 Описание
Реализован движок слабейшего предусловия, который "протягивает" постусловие от конца к началу через присваивания, последовательности и ветвления if, демонстрируя пошаговые преобразования и финальную триаду Хоара.

### 🏗️ Структура проекта
```
WPCalculator/
│
├── WPEngine/              # Основной движок wp
│   ├── Expressions/       # Система выражений
│   │   ├── Expression.cs
│   │   ├── BinaryExpression.cs
│   │   ├── UnaryExpression.cs
│   │   ├── VariableExpression.cs
│   │   └── ConstantExpression.cs
│   │
│   ├── Statements/        # Операторы программы
│   │   ├── Statement.cs
│   │   ├── Assignment.cs
│   │   ├── Sequence.cs
│   │   └── IfStatement.cs
│   │
│   ├── Parser/           # Парсер входных программ
│   │   ├── Token.cs
│   │   ├── TokenType.cs
│   │   ├── TokenReader.cs
│   │   └── Parser.cs
│   │
│   └── Verification/     # Верификация и вычисление wp
│       ├── WpCalculator.cs
│       └── StepTracker.cs
│
├── WpfApp1/              # WPF приложение
│   ├── MainWindow.xaml   # Основной интерфейс
│   └── App.xaml
│
└── WPEngine.Tests/       # Модульные тесты
```
### ✨ Основные возможности
🔄 Протягивание постусловия через присваивания, последовательности и ветвления

📊 Пошаговый трейс преобразований

🎯 Вычисление слабейшего предусловия (wp)

📝 Формирование триады Хоара

⚠️ Автоматическая обработка определённости выражений

### 🚀 Установка и запуск
```
# Клонирование репозитория
git clone <repository-url>
cd WPCalculator

# Восстановление зависимостей
dotnet restore

# Сборка проекта
dotnet build -c Release
```

# Запуск приложения
dotnet run --project WpfApp1

# Запуск тестов
dotnet test
