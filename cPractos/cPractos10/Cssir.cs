using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private List<Car> selectedCars; // Выбранные автомобили для заказа
        private decimal totalPrice; // Общая стоимость заказа
        private decimal totalCarsAmount; // Общая сумма автомобилей в заказе

        private decimal totalRevenue; // Общий доход
        private decimal totalExpenses; // Общие расходы
        private decimal netIncome; // Чистый доход

        private void DisplayCashierMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Меню кассира ==========");
            Console.WriteLine("1. Продажа автомобиля");
            Console.WriteLine("2. Отчет о продажах");
            Console.WriteLine("0. Завершить программу");
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
            Console.WriteLine("Используйте стрелочки для выбора автомобиля, '+' и '-' для изменения количества, 'S' для завершения заказа, 'Enter' для возврата в меню.");

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
                        return; // Завершаем прием заказа
                    case ConsoleKey.Enter: // При нажатии на Enter выход из цикла
                        return; // Возврат в меню кассира
                }
            } while (key.Key != ConsoleKey.Escape);

            UpdateOrderSummary();
        }


        private void UpdateCarSelection(int selectedIndex, int selectedQuantity)
        {
            Console.Clear();
            DisplayAllCars();
            // Пометка на выбранный автомобиль
            Console.SetCursorPosition(0, 2 + selectedIndex);
            Console.Write($"> ");

            // Вывод количество выбранных автомобилей
            Console.SetCursorPosition(50, 2 + selectedIndex);
            Console.Write($"[Заказано: {selectedQuantity}]");
        }

        private void FinalizeOrder(int[] carQuantities)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                if (carQuantities[i] > 0)
                {
                    // обновляем количество в списке авто
                    cars[i].Quantity -= carQuantities[i];
                    // создаем новый объект заказанного авто
                    Car orderCar = new Car(cars[i].Brand, cars[i].Model, cars[i].Price, cars[i].Year, carQuantities[i]);
                    selectedCars.Add(orderCar);

                    // обновляем статистику доходов и покупок
                    totalPrice += cars[i].Price * carQuantities[i];
                    totalCarsAmount += carQuantities[i];
                    decimal income = cars[i].Price * carQuantities[i];
                    AddFinancialTransaction(TransactionType.Income, income, $"Продан автомобиль: {cars[i].Brand} {cars[i].Model} - количество: {carQuantities[i]}");
                    totalRevenue += income;
                }
            }

            // Сохраняем изменения
            SaveCarsToJsonFile();
            netIncome = CalculateNetIncome();

            // После сохранения обнуляем выбранные автомобили
            selectedCars.Clear();
            Console.WriteLine("Заказ успешно завершен.");
            Console.ReadKey();
        }

        private void UpdateOrderSummary()
        {
            
        }




        private void DisplayAllCars()
        {
            Console.WriteLine("Список доступных автомобилей:\n");

            for (int i = 0; i < cars.Count; i++)
            {
                Car car = cars[i];
                Console.WriteLine($"{i + 1}. {car.Brand} {car.Model} {car.Price} ({car.Quantity} в наличии)");
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
            Console.WriteLine("Просмотр финансовых отчетов");
            LoadFinancialTransactionsFromJsonFile(); // Загрузить список финансовых транзакций

            totalRevenue = CalculateTotalRevenue();
            totalExpenses = CalculateTotalExpenses();
            netIncome = CalculateNetIncome();

            int totalSoldCars = CalculateTotalSoldCars();

            Console.WriteLine("Общий доход: " + totalRevenue);
            Console.WriteLine("Чистый доход: " + (totalRevenue * 0.09m)); // Отображать чистую прибыль в виде 9% от общей выручки
            Console.WriteLine("Сумма добавленных в заказ автомобилей: " + totalSoldCars + " автомобилей"); // Отобразить общее количество автомобилей с указанием количества автомобилей
            Console.WriteLine("Цена проданных автомобилей: " + totalRevenue);
            Console.WriteLine();
            Console.WriteLine("Отчет о продажах:");

            foreach (FinancialTransaction transaction in financialTransactions)
            {
                if (transaction.Type == TransactionType.Income)
                {
                    Console.WriteLine("Доход: " + transaction.Amount);
                    if (!string.IsNullOrEmpty(transaction.Description)) // Проверяем, продан ли это автомобиль
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
                  
                    string[] parts = transaction.Description.Split(new[] { " - количество: " }, StringSplitOptions.None);
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
            Console.WriteLine("Данные об автомобилях сохранены в файле cars.json");
        }

        private void AddFinancialTransaction(TransactionType type, decimal amount, string description = "")
        {
            FinancialTransaction transaction = new FinancialTransaction(type, amount, description);
            if (financialTransactions == null)
            {
                financialTransactions = new List<FinancialTransaction>(); //инициализации списка
            }
            financialTransactions.Add(transaction);
            SaveFinancialTransactionsToJsonFile();
            Console.WriteLine("Финансовая транзакция успешно добавлена.");
        }


      

  
      




        private decimal CalculateTotalRevenue()
        {
            decimal totalRevenue = 0;
            foreach (FinancialTransaction transaction in financialTransactions)
            {
                if (transaction.Type == TransactionType.Income && !string.IsNullOrEmpty(transaction.Description))
                {
                    // Если это проданный автомобиль, добавляем его сумму к общей выручке
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
            Console.WriteLine("Данные о финансовых транзакциях сохранены в файле financial_transactions.json");
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
                
                Id = Guid.NewGuid().ToString();//Рома, незабудь, что эта херня отвечает за рандомный Id
                Type = type;
        Amount = amount;
        Description = description;
        Date = DateTime.Now;
            }
        }

    }
}