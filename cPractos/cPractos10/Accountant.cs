using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private List<FinancialTransaction> financialTransactions; // ������ ���������� ����������


        private void DisplayAccountantMenu()
        {
            Console.Clear();
            Console.WriteLine("========== ���� ���������� ==========");
            Console.WriteLine("1. �������� ���������� �������");
            Console.WriteLine("2. �������� �����");
            Console.WriteLine("3. �������� ������");
            Console.WriteLine("4. ����������� ��� ���������� ����������");
            Console.WriteLine("5. �������� ���������� ����������");
            Console.WriteLine("6. ������� ���������� ����������");
            Console.WriteLine("7. ����� ���������� ����������");
            Console.WriteLine("0. ��������� ���������");
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
            Console.WriteLine("�������� ���������� �������");
            LoadFinancialTransactionsFromJsonFile(); // ��������� ������ ���������� ����������

            decimal totalRevenue = financialTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal totalManualExpenses = financialTransactions.Where(t => t.Type == TransactionType.Expense && t.Description == "����� �������").Sum(t => t.Amount);

            decimal netIncome = totalRevenue * 0.09m - totalManualExpenses;

            Console.WriteLine("����� ����� �� ������ ����: " + totalRevenue);
            Console.WriteLine("����� �������: " + totalManualExpenses);
            Console.WriteLine("������ �������: " + netIncome);

            Console.ReadKey();
        }

        private void AddIncome()
        {
            Console.WriteLine("���������� ������");
            Console.WriteLine("������� ����� ������:");
            decimal amount;
            if (decimal.TryParse(Console.ReadLine(), out amount))
            {
                AddFinancialTransaction(TransactionType.Income, amount);
            }
            else
            {
                Console.WriteLine("������������ ����!");
                Console.ReadKey();
            }
        }

        private void AddExpense()
        {
            Console.WriteLine("���������� �������");
            Console.WriteLine("������� ��������:");
            string description = Console.ReadLine();

            Console.WriteLine("������� ����� �������:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                bool isManualExpense = true; // ������ ������ �������
                AddFinancialTransaction(TransactionType.Expense, amount, description, isManualExpense);
                Console.WriteLine("������ ������� ��������.");
            }
            else
            {
                Console.WriteLine("������������ ����!");
            }
            Console.ReadKey();
        }





        private void AddFinancialTransaction(TransactionType type, decimal amount, string description, bool isManualExpense = false)
        {
            // ���� ��������� �������, ������� � "����� �������"
            if (isManualExpense)
            {
                description = "����� �������";
            }

            FinancialTransaction transaction = new FinancialTransaction(type, amount, description);
            financialTransactions.Add(transaction);
            SaveFinancialTransactionsToJsonFile();
            Console.WriteLine("���������� ���������� ������� ���������.");
            Console.ReadKey();
        }


        private void ListFinancialTransactions()
        {
            Console.WriteLine("������ ���� ���������� ����������:");
            LoadFinancialTransactionsFromJsonFile();
            foreach (var transaction in financialTransactions)
            {
                string transactionType = transaction.Type == TransactionType.Income ? "�����" : "������";
                Console.WriteLine($"ID: {transaction.Id}, ���: {transactionType}, �����: {transaction.Amount}, ��������: {transaction.Description}, ����: {transaction.Date.ToShortDateString()}");
            }
            Console.ReadKey();
        }


        private void UpdateFinancialTransaction()
        {
            Console.WriteLine("������� ID ���������� ��� ���������:");
            string inputId = Console.ReadLine();
            var transaction = financialTransactions.FirstOrDefault(t => t.Id == inputId);
            if (transaction == null)
            {
                Console.WriteLine($"���������� � ID {inputId} �� �������.");
            }
            else
            {
                while (true)
                {
                    string transactionType = transaction.Type == TransactionType.Income ? "�����" : "������";
                    Console.WriteLine($"������� ������ ����������: ���: {transactionType}, �����: {transaction.Amount}, ��������: {transaction.Description}");
                    Console.WriteLine("������� ����� ��� ���������� (�����/������) ��� ������� Enter, ����� �������� ������� ���:");
                    var inputType = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputType))
                    {
                        if (inputType.ToUpper() == "�����")
                        {
                            transaction.Type = TransactionType.Income;
                        }
                        else if (inputType.ToUpper() == "������")
                        {
                            transaction.Type = TransactionType.Expense;
                        }
                        else
                        {
                            Console.WriteLine("������������ ��� ����������. ����������, ������� '�����' ��� '������'.");
                            continue;
                        }
                    }

                    Console.WriteLine("������� ����� ����� ���������� ��� ������� Enter, ����� �������� �������:");
                    var inputAmount = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputAmount))
                    {
                        if (decimal.TryParse(inputAmount, out var newAmount))
                        {
                            transaction.Amount = newAmount;
                        }
                        else
                        {
                            Console.WriteLine("������������ �������� �����. ���������� ��� ���.");
                            continue;
                        }
                    }

                    Console.WriteLine("������� ����� �������� ���������� ��� ������� Enter, ����� �������� �������:");
                    var inputDescription = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(inputDescription))
                    {
                        transaction.Description = inputDescription;
                    }

                
                    break;
                }

                SaveFinancialTransactionsToJsonFile(); // ���������� � JSON.
                Console.WriteLine("���������� ���� ���������.");
            }
            Console.ReadKey();
        }



        private void DeleteFinancialTransaction()
        {
            Console.WriteLine("������� ID ���������� ��� ��������:");
            string inputId = Console.ReadLine();
            var transaction = financialTransactions.FirstOrDefault(t => t.Id == inputId);
            if (transaction == null)
            {
                Console.WriteLine($"���������� � ID {inputId} �� �������.");
            }
            else
            {
                financialTransactions.Remove(transaction);
                SaveFinancialTransactionsToJsonFile();
                Console.WriteLine("���������� ���� �������.");
            }
            Console.ReadKey();
        }



        private void SearchFinancialTransactions()
        {
            Console.WriteLine("������� ��� ���������� ��� ������ (�����/������):");
            string inputType = Console.ReadLine();
            TransactionType? type = null;

            switch (inputType.ToUpper())
            {
                case "�����":
                    type = TransactionType.Income;
                    break;
                case "������":
                    type = TransactionType.Expense;
                    break;
                default:
                    Console.WriteLine("����������� ��� ����������. ����� �� ���� ����� ��������.");
                    break;
            }


            Console.WriteLine("������� ���� ��� ������ (������ ��.��.����) ��� �������� ������, ����� ����������:");
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
                    Console.WriteLine("�������� ������ ����. ����� �� ���� ����� ��������.");
                }
            }

            Console.WriteLine("������� ����������� ����� ��� ������:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal minAmount))
            {
                Console.WriteLine("������ �����. ����� �� ����� ����� ��������.");
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
                Console.WriteLine("���������� �� �������.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("��������� ����������:");
            foreach (var transaction in results)
            {
                string transactionType = transaction.Type == TransactionType.Income ? "�����" : "������";
                Console.WriteLine($"ID: {transaction.Id}, ���: {transactionType}, �����: {transaction.Amount}, ��������: {transaction.Description}");


            }
            Console.ReadKey();
        }





    }
}