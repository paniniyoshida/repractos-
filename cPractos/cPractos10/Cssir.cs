using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private List<Car> selectedCars; // ��������� ���������� ��� ������
        private decimal totalPrice; // ����� ��������� ������
        private decimal totalCarsAmount; // ����� ����� ����������� � ������

        private decimal totalRevenue; // ����� �����
        private decimal totalExpenses; // ����� �������
        private decimal netIncome; // ������ �����

        private void DisplayCashierMenu()
        {
            Console.Clear();
            Console.WriteLine("========== ���� ������� ==========");
            Console.WriteLine("1. ������� ����������");
            Console.WriteLine("2. ����� � ��������");
            Console.WriteLine("0. ��������� ���������");
            Console.WriteLine("==========================");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    PrepareOrder();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    SalesReport();
                    break;
                case ConsoleKey.D0:
                    SaveCarsToJsonFile();
                    Environment.Exit(0);
                    break;
            }
        }

        private void PrepareOrder()
        {
            selectedCars = new List<Car>();
            totalPrice = 0;
            totalCarsAmount = 0;

            DisplayAllCars();
            Console.WriteLine("����������� ��������� ��� ������ ����������, '+' � '-' ��� ��������� ����������, 'S' ��� ���������� ������, 'Enter' ��� �������� � ����.");

            int selectedCarIndex = 0;
            int[] carQuantities = new int[cars.Count];

            ConsoleKeyInfo key;

            do
            {
                Console.CursorVisible = false;
                UpdateCarSelection(selectedCarIndex, carQuantities[selectedCarIndex]);

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedCarIndex = (selectedCarIndex > 0) ? selectedCarIndex - 1 : cars.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedCarIndex = (selectedCarIndex + 1) % cars.Count;
                        break;
                    case ConsoleKey.Add:
                    case ConsoleKey.OemPlus:
                        if (carQuantities[selectedCarIndex] < cars[selectedCarIndex].Quantity)
                        {
                            carQuantities[selectedCarIndex]++;
                            UpdateOrderSummary();
                        }
                        break;
                    case ConsoleKey.Subtract:
                    case ConsoleKey.OemMinus:
                        if (carQuantities[selectedCarIndex] > 0)
                        {
                            carQuantities[selectedCarIndex]--;
                            UpdateOrderSummary();
                        }
                        break;
                    case ConsoleKey.S:
                        FinalizeOrder(carQuantities);
                        return; // ��������� ����� ������
                    case ConsoleKey.Enter: // ��� ������� �� Enter ����� �� �����
                        return; // ������� � ���� �������
                }
            } while (key.Key != ConsoleKey.Escape);

            UpdateOrderSummary();
        }


        private void UpdateCarSelection(int selectedIndex, int selectedQuantity)
        {
            Console.Clear();
            DisplayAllCars();
            // ������� �� ��������� ����������
            Console.SetCursorPosition(0, 2 + selectedIndex);
            Console.Write($"> ");

            // ����� ���������� ��������� �����������
            Console.SetCursorPosition(50, 2 + selectedIndex);
            Console.Write($"[��������: {selectedQuantity}]");
        }

        private void FinalizeOrder(int[] carQuantities)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                if (carQuantities[i] > 0)
                {
                    // ��������� ���������� � ������ ����
                    cars[i].Quantity -= carQuantities[i];
                    // ������� ����� ������ ����������� ����
                    Car orderCar = new Car(cars[i].Brand, cars[i].Model, cars[i].Price, cars[i].Year, carQuantities[i]);
                    selectedCars.Add(orderCar);

                    // ��������� ���������� ������� � �������
                    totalPrice += cars[i].Price * carQuantities[i];
                    totalCarsAmount += carQuantities[i];
                    decimal income = cars[i].Price * carQuantities[i];
                    AddFinancialTransaction(TransactionType.Income, income, $"������ ����������: {cars[i].Brand} {cars[i].Model} - ����������: {carQuantities[i]}");
                    totalRevenue += income;
                }
            }

            // ��������� ���������
            SaveCarsToJsonFile();
            netIncome = CalculateNetIncome();

            // ����� ���������� �������� ��������� ����������
            selectedCars.Clear();
            Console.WriteLine("����� ������� ��������.");
            Console.ReadKey();
        }

        private void UpdateOrderSummary()
        {
            
        }




        private void DisplayAllCars()
        {
            Console.WriteLine("������ ��������� �����������:\n");

            for (int i = 0; i < cars.Count; i++)
            {
                Car car = cars[i];
                Console.WriteLine($"{i + 1}. {car.Brand} {car.Model} {car.Price} ({car.Quantity} � �������)");
            }

            Console.WriteLine();
        }

        private void LoadFinancialTransactionsFromJsonFile()
        {


            if (File.Exists("financial_transactions.json"))
            {
                string json = File.ReadAllText("financial_transactions.json");
                financialTransactions = JsonConvert.DeserializeObject<List<FinancialTransaction>>(json);
            }
            else
            {
                financialTransactions = new List<FinancialTransaction>();
            }
        }

        private void SalesReport()
        {
            Console.WriteLine("�������� ���������� �������");
            LoadFinancialTransactionsFromJsonFile(); // ��������� ������ ���������� ����������

            totalRevenue = CalculateTotalRevenue();
            totalExpenses = CalculateTotalExpenses();
            netIncome = CalculateNetIncome();

            int totalSoldCars = CalculateTotalSoldCars();

            Console.WriteLine("����� �����: " + totalRevenue);
            Console.WriteLine("������ �����: " + (totalRevenue * 0.09m)); // ���������� ������ ������� � ���� 9% �� ����� �������
            Console.WriteLine("����� ����������� � ����� �����������: " + totalSoldCars + " �����������"); // ���������� ����� ���������� ����������� � ��������� ���������� �����������
            Console.WriteLine("���� ��������� �����������: " + totalRevenue);
            Console.WriteLine();
            Console.WriteLine("����� � ��������:");

            foreach (FinancialTransaction transaction in financialTransactions)
            {
                if (transaction.Type == TransactionType.Income)
                {
                    Console.WriteLine("�����: " + transaction.Amount);
                    if (!string.IsNullOrEmpty(transaction.Description)) // ���������, ������ �� ��� ����������
                    {
                        Console.WriteLine(transaction.Description);
                    }
                }
            }

            Console.ReadKey();
        }


        private int CalculateTotalSoldCars()
        {
            int totalSoldCars = 0;
            foreach (FinancialTransaction transaction in financialTransactions)
            {
                if (transaction.Type == TransactionType.Income && !string.IsNullOrEmpty(transaction.Description))
                {
                  
                    string[] parts = transaction.Description.Split(new[] { " - ����������: " }, StringSplitOptions.None);
                    if (parts.Length == 2 && int.TryParse(parts[1], out int quantity))
                    {
                        totalSoldCars += quantity;
                    }
                }
            }
            return totalSoldCars;
        }

        private decimal CalculateTotalCarsAmount()
        {
            return selectedCars.Sum(car => car.Quantity);
        }

        private void SaveCarsToJsonFile()
        {
            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("cars.json", json);
            Console.WriteLine("������ �� ����������� ��������� � ����� cars.json");
        }

        private void AddFinancialTransaction(TransactionType type, decimal amount, string description = "")
        {
            FinancialTransaction transaction = new FinancialTransaction(type, amount, description);
            if (financialTransactions == null)
            {
                financialTransactions = new List<FinancialTransaction>(); //������������� ������
            }
            financialTransactions.Add(transaction);
            SaveFinancialTransactionsToJsonFile();
            Console.WriteLine("���������� ���������� ������� ���������.");
        }


      

  
      




        private decimal CalculateTotalRevenue()
        {
            decimal totalRevenue = 0;
            foreach (FinancialTransaction transaction in financialTransactions)
            {
                if (transaction.Type == TransactionType.Income && !string.IsNullOrEmpty(transaction.Description))
                {
                    // ���� ��� ��������� ����������, ��������� ��� ����� � ����� �������
                    totalRevenue += transaction.Amount;
                }
            }
            return totalRevenue;
        }


        private decimal CalculateTotalExpenses()
        {
            decimal totalExpenses = 0;
            foreach (FinancialTransaction transaction in financialTransactions)
            {
                if (transaction.Type == TransactionType.Expense)
                {
                    totalExpenses += transaction.Amount;
                }
            }
            return totalExpenses;
        }

        private decimal CalculateNetIncome()
        {
            decimal totalRevenue = CalculateTotalRevenue();
            decimal totalExpenses = CalculateTotalExpenses();
            return totalRevenue - totalExpenses;
        }



        private void SaveFinancialTransactionsToJsonFile()
        {
            string json = JsonConvert.SerializeObject(financialTransactions, Formatting.Indented);
            File.WriteAllText("financial_transactions.json", json);
            Console.WriteLine("������ � ���������� ����������� ��������� � ����� financial_transactions.json");
        }

        public class FinancialTransaction
        {
            public string Id { get; set; }
            public TransactionType Type { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }

            public FinancialTransaction(TransactionType type, decimal amount, string description)
            {
                
                Id = Guid.NewGuid().ToString();//����, ��������, ��� ��� ����� �������� �� ��������� Id
                Type = type;
        Amount = amount;
        Description = description;
        Date = DateTime.Now;
            }
        }

    }
}