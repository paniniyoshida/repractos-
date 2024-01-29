using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {

        private void DisplayPersonnelManagerMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Меню менеджера персонала ==========");
            Console.WriteLine("1. Просмотреть список сотрудников");
            Console.WriteLine("2. Добавить сотрудника");
            Console.WriteLine("3. Изменить информацию о сотруднике");
            Console.WriteLine("4. Уволить сотрудника");
            Console.WriteLine("0. Завершить программу");
            Console.WriteLine("==========================");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    ViewEmployeeList();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    AddEmployee();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    UpdateEmployee();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    FireEmployee();
                    break;
                case ConsoleKey.D0:
                    SaveUsers(users);
                    Environment.Exit(0);
                    break;
            }
        }

        private void ViewEmployeeList()
        {
            Console.WriteLine("Просмотр списка сотрудников");
            foreach (User user in users)
            {
                if (user.Role != Role.Administrator)
                {
                    Console.WriteLine($"Логин: {user.Login}, Роль: {user.Role}, Имя: {user.Name}");
                }
            }
        }

        private void AddEmployee()
        {
            Console.WriteLine("Добавление сотрудника");

            Console.WriteLine("Введите логин нового сотрудника:");
            string login = Console.ReadLine();

            Console.WriteLine("Введите имя пользователя:");
            string name = Console.ReadLine();

            Console.WriteLine("Введите пароль нового сотрудника:");
            string password = GetHiddenPassword();

            Console.WriteLine("Выберите роль нового сотрудника:");
            Console.WriteLine("1. Кассир");
            Console.WriteLine("2. Менеджер персонала");
            Console.WriteLine("3. Менеджер склада");
            Console.WriteLine("4. Бухгалтер");

            ConsoleKeyInfo key = Console.ReadKey();
            Role role = Role.Cashier;

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    role = Role.Cashier;
                    break;
                case ConsoleKey.D2:
                    role = Role.PersonnelManager;
                    break;
                case ConsoleKey.D3:
                    role = Role.WarehouseManager;
                    break;
                case ConsoleKey.D4:
                    role = Role.Accountant;
                    break;
            }

            User newEmployee = new User(login, password, role, name);
            users.Add(newEmployee);

            Console.WriteLine("Сотрудник успешно добавлен.");
        }

        private void UpdateEmployee()
        {
            Console.WriteLine("Изменение информации о сотруднике");
            Console.WriteLine("Введите логин сотрудника, информацию о котором хотите изменить:");
            string login = Console.ReadLine();

            User employee = users.Find(u => u.Login == login && u.Role != Role.Administrator);

            if (employee == null)
            {
                Console.WriteLine("Сотрудник не найден.");
                return;
            }

            Console.WriteLine("Выберите новую роль сотрудника:");
            Console.WriteLine("1. Кассир");
            Console.WriteLine("2. Менеджер персонала");
            Console.WriteLine("3. Менеджер склада");
            Console.WriteLine("4. Бухгалтер");

            ConsoleKeyInfo key = Console.ReadKey();
            Role newRole = Role.Cashier;

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    newRole = Role.Cashier;
                    break;
                case ConsoleKey.D2:
                    newRole = Role.PersonnelManager;
                    break;
                case ConsoleKey.D3:
                    newRole = Role.WarehouseManager;
                    break;
                case ConsoleKey.D4:
                    newRole = Role.Accountant;
                    break;
            }

            int index = users.IndexOf(employee);
            users[index].Role = newRole;

            Console.WriteLine("Информация о сотруднике успешно изменена.");
        }

        private void FireEmployee()
        {
            Console.WriteLine("Увольнение сотрудника");
            Console.WriteLine("Введите логин сотрудника, которого хотите уволить:");
            string login = Console.ReadLine();

            User employee = users.Find(u => u.Login == login && u.Role != Role.Administrator);

            if (employee == null)
            {
                Console.WriteLine("Сотрудник не найден.");
                return;
            }

            users.Remove(employee);
            Console.WriteLine("Сотрудник успешно уволен.");
        }

    }
}