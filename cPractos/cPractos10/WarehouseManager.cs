using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private List<Car> cars; // ��� �������� �����������



        private void DisplayWarehouseManagerMenu()
        {
            Console.Clear();
            Console.WriteLine("========== ���� �����-��������� ==========");
            Console.WriteLine("1. ����� ����������� �� �����");
            Console.WriteLine("2. ���������� ��� ������ �� ������");
            Console.WriteLine("3. ����� ������ �� ���������");
            Console.WriteLine("0. ��������� ���������");
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
            Console.WriteLine("����� ����������� �� �����");

            while (true)
            {
                Console.WriteLine("������� ����� ����������:");
                string brand = Console.ReadLine();

                Console.WriteLine("������� ������ ����������:");
                string model = Console.ReadLine();

                Console.WriteLine("������� ��� �������:");
                if (!TryParseAndValidate(Console.ReadLine(), out int year, y => y > 0 && y <= DateTime.Now.Year))
                {
                    Console.WriteLine("�������� ���, ���������� ��� ���.");
                    continue;
                }

                Console.WriteLine("������� ����:");
                if (!TryParseAndValidate(Console.ReadLine(), out int price, p => p > 0))
                {
                    Console.WriteLine("�������� ����, ���������� ��� ���.");
                    continue;
                }

                Console.WriteLine("������� ����������:");
                if (!TryParseAndValidate(Console.ReadLine(), out int quantity, q => q > 0))
                {
                    Console.WriteLine("�������� ����������, ���������� ��� ���.");
                    continue;
                }

                Car newCar = new Car(brand, model, year, price, quantity);
                cars.Add(newCar);

                Console.WriteLine("���������� ������� �������� �� �����!");
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
            Console.WriteLine("����� ����������� �� ���������");

            Console.WriteLine("�������� ������� ��� ������:");
            Console.WriteLine("1. �����");
            Console.WriteLine("2. ������");
            Console.WriteLine("3. ��� �������");
            Console.WriteLine("4. ����");
            Console.WriteLine("5. ����������");

            ConsoleKeyInfo attributeKey = Console.ReadKey();
            Console.WriteLine();

            switch (attributeKey.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("������� ����� ����������:");
                    string brand = Console.ReadLine();

                    List<Car> brandCars = cars.FindAll(car => car.Brand.Equals(brand));
                    if (brandCars.Count > 0)
                    {
                        Console.WriteLine("���������� ������ �� �����:");
                        foreach (Car car in brandCars)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("���������� � ����� ������ �� �������.");
                    }
                    break;

                case ConsoleKey.D2:
                    Console.WriteLine("������� ������ ����������:");
                    string model = Console.ReadLine();

                    List<Car> modelCars = cars.FindAll(car => car.Model.Equals(model));
                    if (modelCars.Count > 0)
                    {
                        Console.WriteLine("���������� ������ �� ������:");
                        foreach (Car car in modelCars)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("���������� � ����� ������� �� �������.");
                    }
                    break;

                case ConsoleKey.D3:
                    Console.WriteLine("������� ��� �������:");
                    int year = int.Parse(Console.ReadLine());

                    List<Car> yearCars = cars.FindAll(car => car.Year == year);
                    if (yearCars.Count > 0)
                    {
                        Console.WriteLine("���������� ������ �� ���� �������:");
                        foreach (Car car in yearCars)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("���������� � ����� ����� ������� �� �������.");
                    }
                    break;

                default:
                    Console.WriteLine("�������� ����� ��������.");
                    break;

                case ConsoleKey.D4:
                    Console.WriteLine("������� ����:");
                    int price = int.Parse(Console.ReadLine());

                    List<Car> yearPrice = cars.FindAll(car => car.Price == price);
                    if (yearPrice.Count > 0)
                    {
                        Console.WriteLine("���������� ������ �� ����:");
                        foreach (Car car in yearPrice)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("���������� � ����� ����� �� �������.");
                    }
                    break;

                case ConsoleKey.D5:
                    Console.WriteLine("������� ����������:");
                    int quantity = int.Parse(Console.ReadLine());

                    List<Car> yearQuantity = cars.FindAll(car => car.Price == quantity);
                    if (yearQuantity.Count > 0)
                    {
                        Console.WriteLine("���������� ������ �� ����:");
                        foreach (Car car in yearQuantity)
                        {
                            Console.WriteLine(car.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("���������� � ����� ����� �� �������.");
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
                Console.WriteLine("������ �� ����������� ��������� �� ����� cars.json");
            }
            else
            {
                cars = new List<Car>();
                Console.WriteLine("���� cars.json �� ������. ������ ����� ������ �����������.");
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
            return $"�����: {Brand}, ������: {Model}, ��� �������: {Year}, ����: {Price:C}";
        }
    }
    }