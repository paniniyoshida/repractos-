using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private void DisplayAdminMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Меню администратора ==========");
            Console.WriteLine("1. Просмотреть список пользователей");
            Console.WriteLine("2. Добавить пользователя");
            Console.WriteLine("3. Изменить пользователя");
            Console.WriteLine("4. Удалить пользователя");
            Console.WriteLine("5. Поиск пользователя");
            Console.WriteLine("0. Завершить программу");
            Console.WriteLine("==========================");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    ViewUserList();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    AddUser();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    UpdateUser();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    DeleteUser();
                    break;
                case ConsoleKey.D5:
                    Console.Clear();
                    SearchUser();
                    break;
                case ConsoleKey.D0:
                    SaveUsers(users);
                    Environment.Exit(0);
                    break;
            }
        }

        private void ViewUserList()
        {
            Console.Clear();
            Console.WriteLine("========== Список пользователей ==========");
            foreach (var user in users)
            {
                Console.WriteLine($"Логин: {user.Login} | Роль: {user.Role} | Имя: {user.Name}");
            }
            Console.WriteLine("==========================================");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void AddUser()
        {
            Console.WriteLine("Добавление пользователя");
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();

            // Проверка наличия пользователя с таким же логином
            if (users.Any(u => u.Login == login))
            {
                Console.WriteLine("Пользователь с таким логином уже существует.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
                DisplayAdminMenu();
                return;
            }

            Console.WriteLine("Введите пароль:");
            string password = GetHiddenPassword();

            Console.WriteLine("Введите имя пользователя:");
            string name = Console.ReadLine();

            Console.WriteLine("Выберите роль (1 - Администратор, 2 - Кассир, 3 - Менеджер персонала, 4 - Склад-менеджер, 5 - Бухгалтер):");
            ConsoleKeyInfo roleKey = Console.ReadKey();
            Role role = Role.Administrator;

            switch (roleKey.Key)
            {
                case ConsoleKey.D1:
                    role = Role.Administrator;
                    break;
                case ConsoleKey.D2:
                    role = Role.Cashier;
                    break;
                case ConsoleKey.D3:
                    role = Role.PersonnelManager;
                    break;
                case ConsoleKey.D4:
                    role = Role.WarehouseManager;
                    break;
                case ConsoleKey.D5:
                    role = Role.Accountant;
                    break;
                default:
                    Console.WriteLine("Неправильный ввод роли.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayAdminMenu();
                    return;
            }

            User newUser = new User(login, password, role, name);
            users.Add(newUser);
            SaveUsers(users);
            Console.WriteLine("Пользователь успешно добавлен.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void UpdateUser()
        {
            Console.WriteLine("Изменение пользователя");
            Console.WriteLine("Введите логин пользователя, которого хотите изменить:");
            string login = Console.ReadLine();

            User userToUpdate = users.Find(u => u.Login == login);

            if (userToUpdate == null)
            {
                Console.WriteLine("Пользователь не найден.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
                DisplayAdminMenu();
                return;
            }

            Console.WriteLine("Выберите, что хотите изменить (1 - логин, 2 - пароль, 3 - роль, 4 - имя):");
            ConsoleKeyInfo updateKey = Console.ReadKey();

            switch (updateKey.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("\nВведите новый логин:");
                    string newLogin = Console.ReadLine();

                    // Проверка наличия пользователя с таким же логином
                    if (users.Any(u => u.Login == newLogin))
                    {
                        Console.WriteLine("Пользователь с таким логином уже существует.");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                        DisplayAdminMenu();
                        return;
                    }

                    userToUpdate.Login = newLogin;
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("\nВведите новый пароль:");
                    string newPassword = GetHiddenPassword();
                    userToUpdate.Password = newPassword;
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("\nВыберите новую роль (1 - Администратор, 2 - Кассир, 3 - Менеджер персонала, 4 - Склад-менеджер, 5 - Бухгалтер):");
                    ConsoleKeyInfo roleKey = Console.ReadKey();
                    Role newRole = Role.Administrator;

                    switch (roleKey.Key)
                    {
                        case ConsoleKey.D1:
                            newRole = Role.Administrator;
                            break;
                        case ConsoleKey.D2:
                            newRole = Role.Cashier;
                            break;
                        case ConsoleKey.D3:
                            newRole = Role.PersonnelManager;
                            break;
                        case ConsoleKey.D4:
                            newRole = Role.WarehouseManager;
                            break;
                        case ConsoleKey.D5:
                            newRole = Role.Accountant;
                            break;
                        default:
                            Console.WriteLine("Неправильный ввод роли.");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            Console.Clear();
                            DisplayAdminMenu();
                            return;
                    }

                    userToUpdate.Role = newRole;
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("\nВведите новое имя пользователя:");
                    string newName = Console.ReadLine();
                    userToUpdate.Name = newName;
                    break;
                default:
                    Console.WriteLine("Неправильный выбор.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayAdminMenu();
                    return;
            }

            SaveUsers(users);
            Console.WriteLine("Пользователь успешно изменен.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void DeleteUser()
        {
            Console.WriteLine("Удаление пользователя");
            Console.WriteLine("Введите логин пользователя, которого хотите удалить:");
            string login = Console.ReadLine();

            User userToDelete = users.Find(u => u.Login == login);

            if (userToDelete == null)
            {
                Console.WriteLine("Пользователь не найден.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
                DisplayAdminMenu();
                return;
            }

            users.Remove(userToDelete);
            SaveUsers(users);
            Console.WriteLine("Пользователь успешно удален.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void SearchUser()
        {
            Console.WriteLine("Поиск пользователя");
            Console.WriteLine("Введите логин пользователя:");
            string login = Console.ReadLine();

            User user = users.Find(u => u.Login == login);

            if (user == null)
            {
                Console.WriteLine("Пользователь не найден.");
            }
            else
            {
                Console.WriteLine($"Логин: {user.Login} | Роль: {user.Role} | Имя: {user.Name}");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }
    }
}