using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private List<Car> cars; // Для хранения автомобилей



        private void DisplayWarehouseManagerMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Меню склад-менеджера ==========");
            Console.WriteLine("1. Прием автомобилей на склад");
            Console.WriteLine("2. Посмотреть все товары на складе");
            Console.WriteLine("3. Поиск товара по атрибутам");
            Console.WriteLine("0. Завершить программу");
            Console.WriteLine("==========================");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    ReceiveCars();
                    break;
               
                case ConsoleKey.D2:
                    Console.Clear();
                    DisplayAllCars();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    SearchCars();
                    break;
                case ConsoleKey.D0:
                    SaveCarsToJsonFile();
                    Environment.Exit(0);
                    break;
            }
        }

        private void ReceiveCars()
        {
            Console.WriteLine("Прием автомобилей на склад");

            while (true)
            {
                Console.WriteLine("Введите марку автомобиля:");
                string brand = Console.ReadLine();

                Console.WriteLine("Введите модель автомобиля:");
                string model = Console.ReadLine();

                Console.WriteLine("Введите год выпуска:");
                if (!TryParseAndValidate(Console.ReadLine(), out int year, y => y > 0 && y <= DateTime.Now.Year))
                {
                    Console.WriteLine("Неверный год, попробуйте еще раз.");
                    continue;
                }

                Console.WriteLine("Введите цену:");
                if (!TryParseAndValidate(Console.ReadLine(), out int price, p => p > 0))
                {
                    Console.WriteLine("Неверная цена, попробуйте еще раз.");
                    continue;
                }

                Console.WriteLine("Введите количество:");
                if (!TryParseAndValidate(Console.ReadLine(), out int quantity, q => q > 0))
                {
                    Console.WriteLine("Неверное количество, попробуйте еще раз.");
                    continue;
                }

                Car newCar = new Car(brand, model, year, price, quantity);
                cars.Add(newCar);

                Console.WriteLine("Автомобиль успешно добавлен на склад!");
                break;
            }
            Console.ReadKey();
        }

        private bool TryParseAndValidate(string input, out int result, Func<int, bool> validateFn)
        {
            bool validInput = int.TryParse(input, out result) && validateFn(result);
            return validInput;
        }


        


        

        private void SearchCars()
        {
            Console.WriteLine("Поиск автомобилей по атрибутам");

            Console.WriteLine("Выберите атрибут для поиска:");
            Console.WriteLine("1. Марка");
            Console.WriteLine("2. Модель");
            Console.WriteLine("3. Год выпуска");
            Console.WriteLine("4. Цена");
            Console.WriteLine("5. Количество");

            ConsoleKeyInfo attributeKey = Console.ReadKey();
            Console.WriteLine();

            switch (attributeKey.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Введите марку автомобиля:");
                    string brand = Console.ReadLine();

                    List<Car> brandCars = cars.FindAll(car => car.Brand.Equals(brand));
                    if (brandCars.Count > 0)
                    {
                        Console.WriteLine("Результаты поиска по марке:");
                        foreach (Car car in brandCars)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Автомобили с такой маркой не найдены.");
                    }
                    break;

                case ConsoleKey.D2:
                    Console.WriteLine("Введите модель автомобиля:");
                    string model = Console.ReadLine();

                    List<Car> modelCars = cars.FindAll(car => car.Model.Equals(model));
                    if (modelCars.Count > 0)
                    {
                        Console.WriteLine("Результаты поиска по модели:");
                        foreach (Car car in modelCars)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Автомобили с такой моделью не найдены.");
                    }
                    break;

                case ConsoleKey.D3:
                    Console.WriteLine("Введите год выпуска:");
                    int year = int.Parse(Console.ReadLine());

                    List<Car> yearCars = cars.FindAll(car => car.Year == year);
                    if (yearCars.Count > 0)
                    {
                        Console.WriteLine("Результаты поиска по году выпуска:");
                        foreach (Car car in yearCars)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Автомобили с таким годом выпуска не найдены.");
                    }
                    break;

                default:
                    Console.WriteLine("Неверный выбор атрибута.");
                    break;

                case ConsoleKey.D4:
                    Console.WriteLine("Введите цену:");
                    int price = int.Parse(Console.ReadLine());

                    List<Car> yearPrice = cars.FindAll(car => car.Price == price);
                    if (yearPrice.Count > 0)
                    {
                        Console.WriteLine("Результаты поиска по цене:");
                        foreach (Car car in yearPrice)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Автомобили с такой ценой не найдены.");
                    }
                    break;

                case ConsoleKey.D5:
                    Console.WriteLine("Введите количество:");
                    int quantity = int.Parse(Console.ReadLine());

                    List<Car> yearQuantity = cars.FindAll(car => car.Price == quantity);
                    if (yearQuantity.Count > 0)
                    {
                        Console.WriteLine("Результаты поиска по цене:");
                        foreach (Car car in yearQuantity)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Автомобили с такой ценой не найдены.");
                    }
                    break;



            }
            Console.ReadKey();
        }

      

        private void LoadCarsFromJsonFile()
        {
            if (File.Exists("cars.json"))
            {
                string json = File.ReadAllText("cars.json");
                cars = JsonConvert.DeserializeObject<List<Car>>(json);
                Console.WriteLine("Данные об автомобилях загружены из файла cars.json");
            }
            else
            {
                cars = new List<Car>();
                Console.WriteLine("Файл cars.json не найден. Создан новый список автомобилей.");
            }
        }
    }

    class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        

        public Car(string brand, string model, int year, int price, int quantity)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Марка: {Brand}, Модель: {Model}, Год выпуска: {Year}, Цена: {Price:C}";
        }
    }
    }