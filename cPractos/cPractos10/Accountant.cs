using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private List<FinancialTransaction> financialTransactions; // Список финансовых транзакций


        private void DisplayAccountantMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Меню бухгалтера ==========");
            Console.WriteLine("1. Просмотр финансовых отчетов");
            Console.WriteLine("2. Добавить доход");
            Console.WriteLine("3. Добавить расход");
            Console.WriteLine("4. Просмотреть все финансовые транзакции");
            Console.WriteLine("5. Изменить финансовую транзакцию");
            Console.WriteLine("6. Удалить финансовую транзакцию");
            Console.WriteLine("7. Поиск финансовых транзакций");
            Console.WriteLine("0. Завершить программу");
            Console.WriteLine("==========================");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    DisplayFinancialReports();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    AddIncome();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    AddExpense();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    ListFinancialTransactions();
                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    UpdateFinancialTransaction();
                    break;
                case ConsoleKey.D6:
                    Console.Clear();
                    DeleteFinancialTransaction();
                    break;
                case ConsoleKey.D7:
                    Console.Clear();
                    SearchFinancialTransactions();
                    break;
                case ConsoleKey.D0:
                    Environment.Exit(0);
                    break;
                
            }
        }



        private void DisplayFinancialReports()
        {
            Console.WriteLine("Просмотр финансовых отчетов");
            LoadFinancialTransactionsFromJsonFile(); // Загрузить список финансовых транзакций

            decimal totalRevenue = financialTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal totalManualExpenses = financialTransactions.Where(t => t.Type == TransactionType.Expense && t.Description == "Общие расходы").Sum(t => t.Amount);

            decimal netIncome = totalRevenue * 0.09m - totalManualExpenses;

            Console.WriteLine("Общий доход от продаж авто: " + totalRevenue);
            Console.WriteLine("Общие расходы: " + totalManualExpenses);
            Console.WriteLine("Чистая прибыль: " + netIncome);

            Console.ReadKey();
        }

        private void AddIncome()
        {
            Console.WriteLine("Добавление дохода");
            Console.WriteLine("Введите сумму дохода:");
            decimal amount;
            if (decimal.TryParse(Console.ReadLine(), out amount))
            {
                AddFinancialTransaction(TransactionType.Income, amount);
            }
            else
            {
                Console.WriteLine("Некорректный ввод!");
                Console.ReadKey();
            }
        }

        private void AddExpense()
        {
            Console.WriteLine("Добавление расхода");
            Console.WriteLine("Введите описание:");
            string description = Console.ReadLine();

            Console.WriteLine("Введите сумму расхода:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                bool isManualExpense = true; // Расход внесен вручную
                AddFinancialTransaction(TransactionType.Expense, amount, description, isManualExpense);
                Console.WriteLine("Расход успешно добавлен.");
            }
            else
            {
                Console.WriteLine("Некорректный ввод!");
            }
            Console.ReadKey();
        }





        private void AddFinancialTransaction(TransactionType type, decimal amount, string description, bool isManualExpense = false)
        {
            // Если внесенный вручную, фигачим в "Общие расходы"
            if (isManualExpense)
            {
                description = "Общие расходы";
            }

            FinancialTransaction transaction = new FinancialTransaction(type, amount, description);
            financialTransactions.Add(transaction);
            SaveFinancialTransactionsToJsonFile();
            Console.WriteLine("Финансовая транзакция успешно добавлена.");
            Console.ReadKey();
        }


        private void ListFinancialTransactions()
        {
            Console.WriteLine("Список всех финансовых транзакций:");
            LoadFinancialTransactionsFromJsonFile();
            foreach (var transaction in financialTransactions)
            {
                string transactionType = transaction.Type == TransactionType.Income ? "Доход" : "Расход";
                Console.WriteLine($"ID: {transaction.Id}, Тип: {transactionType}, Сумма: {transaction.Amount}, Описание: {transaction.Description}, Дата: {transaction.Date.ToShortDateString()}");
            }
            Console.ReadKey();
        }


        private void UpdateFinancialTransaction()
        {
            Console.WriteLine("Введите ID транзакции для изменения:");
            string inputId = Console.ReadLine();
            var transaction = financialTransactions.FirstOrDefault(t => t.Id == inputId);
            if (transaction == null)
            {
                Console.WriteLine($"Транзакция с ID {inputId} не найдена.");
            }
            else
            {
                while (true)
                {
                    string transactionType = transaction.Type == TransactionType.Income ? "Доход" : "Расход";
                    Console.WriteLine($"Текущие данные транзакции: Тип: {transactionType}, Сумма: {transaction.Amount}, Описание: {transaction.Description}");
                    Console.WriteLine("Введите новый тип транзакции (Доход/Расход) или нажмите Enter, чтобы оставить текущий тип:");
                    var inputType = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputType))
                    {
                        if (inputType.ToUpper() == "Доход")
                        {
                            transaction.Type = TransactionType.Income;
                        }
                        else if (inputType.ToUpper() == "Расход")
                        {
                            transaction.Type = TransactionType.Expense;
                        }
                        else
                        {
                            Console.WriteLine("Некорректный тип транзакции. Пожалуйста, введите 'Доход' или 'Расход'.");
                            continue;
                        }
                    }

                    Console.WriteLine("Введите новую сумму транзакции или нажмите Enter, чтобы оставить текущую:");
                    var inputAmount = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputAmount))
                    {
                        if (decimal.TryParse(inputAmount, out var newAmount))
                        {
                            transaction.Amount = newAmount;
                        }
                        else
                        {
                            Console.WriteLine("Некорректное значение суммы. Попробуйте еще раз.");
                            continue;
                        }
                    }

                    Console.WriteLine("Введите новое описание транзакции или нажмите Enter, чтобы оставить текущее:");
                    var inputDescription = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputDescription))
                    {
                        transaction.Description = inputDescription;
                    }

                
                    break;
                }

                SaveFinancialTransactionsToJsonFile(); // Сохранение в JSON.
                Console.WriteLine("Транзакция была обновлена.");
            }
            Console.ReadKey();
        }



        private void DeleteFinancialTransaction()
        {
            Console.WriteLine("Введите ID транзакции для удаления:");
            string inputId = Console.ReadLine();
            var transaction = financialTransactions.FirstOrDefault(t => t.Id == inputId);
            if (transaction == null)
            {
                Console.WriteLine($"Транзакция с ID {inputId} не найдена.");
            }
            else
            {
                financialTransactions.Remove(transaction);
                SaveFinancialTransactionsToJsonFile();
                Console.WriteLine("Транзакция была удалена.");
            }
            Console.ReadKey();
        }



        private void SearchFinancialTransactions()
        {
            Console.WriteLine("Введите тип транзакции для поиска (Доход/Расход):");
            string inputType = Console.ReadLine();
            TransactionType? type = null;

            switch (inputType.ToUpper())
            {
                case "Доход":
                    type = TransactionType.Income;
                    break;
                case "Расход":
                    type = TransactionType.Expense;
                    break;
                default:
                    Console.WriteLine("Неизвестный тип транзакции. Поиск по типу будет пропущен.");
                    break;
            }


            Console.WriteLine("Введите дату для поиска (формат ДД.ММ.ГГГГ) или оставьте пустым, чтобы пропустить:");
            DateTime? dateForSearch = null;
            string inputDate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputDate))
            {
                if (DateTime.TryParseExact(inputDate, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    dateForSearch = parsedDate.Date;
                }
                else
                {
                    Console.WriteLine("Неверный формат даты. Поиск по дате будет пропущен.");
                }
            }

            Console.WriteLine("Введите минимальную сумму для поиска:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal minAmount))
            {
                Console.WriteLine("Ошибка ввода. Поиск по сумме будет пропущен.");
                minAmount = 0;
            }



            var query = financialTransactions.AsEnumerable();
            if (type.HasValue)
            {
                query = query.Where(t => t.Type == type);
            }
            if (minAmount > 0)
            {
                query = query.Where(t => t.Amount >= minAmount);
            }

            var results = query.ToList();

            if (!results.Any())
            {
                Console.WriteLine("Транзакции не найдены.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Найденные транзакции:");
            foreach (var transaction in results)
            {
                string transactionType = transaction.Type == TransactionType.Income ? "Доход" : "Расход";
                Console.WriteLine($"ID: {transaction.Id}, Тип: {transactionType}, Сумма: {transaction.Amount}, Описание: {transaction.Description}");


            }
            Console.ReadKey();
        }





    }
}