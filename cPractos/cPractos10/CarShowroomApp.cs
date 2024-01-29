using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CarShowroomApp
{
    partial class CarShowroomApp
    {
        private User currentUser;
        private List<User> users;
        private string usersFilePath = "users.json";

        public CarShowroomApp()
        {
            users = LoadUsers();
            

            // Инициализация пользователей, если файл не найден или пустой
            if (users == null || users.Count == 0)
            {
                InitializeUsers();
                SaveUsers(users);
            }
        }

        public void Run()
        {

            LoadCarsFromJsonFile(); // Инициализация списка автомобилей перед отображением менюшки
            bool isAuthenticated = false;
            do
            {
                isAuthenticated = UserAuthentication();
            } while (!isAuthenticated);

            Console.WriteLine($"Вы авторизовались под пользователем: {GetLoggedInUserName()}");

            while (true)
            {
                if (currentUser.Role == Role.Administrator)
                {
                    DisplayAdminMenu();
                }
                else if (currentUser.Role == Role.Cashier)
                {
                    DisplayCashierMenu();
                }
                else if (currentUser.Role == Role.PersonnelManager)
                {
                    DisplayPersonnelManagerMenu();
                }
                else if (currentUser.Role == Role.WarehouseManager)
                {
                    DisplayWarehouseManagerMenu();
                }
                else if (currentUser.Role == Role.Accountant)
                {
                    DisplayAccountantMenu();
                }

                Console.WriteLine("Выберите операцию (нажмите Esc для выхода):");
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    UserAuthentication();
                    Console.WriteLine($"Вы авторизовались под пользователем: {GetLoggedInUserName()}");
                }

                Console.WriteLine();
            }
        }

        private bool UserAuthentication()
        {
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();


            Console.WriteLine("Введите пароль:");
            string password = GetHiddenPassword();

            User user = users.Find(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                currentUser = user;
                return true;
            }
            else
            {
                Console.WriteLine("Неверные логин или пароль. Пожалуйста, повторите попытку.");
                return false;
            }
        }

        private void InitializeUsers()
        {
            users = new List<User>();
            users.Add(new User("admin", "admin123", Role.Administrator, "Дима"));
            users.Add(new User("cashier", "cashier123", Role.Cashier, "Серёга"));
            users.Add(new User("manager", "manager123", Role.PersonnelManager, "Егор"));
            users.Add(new User("warehouse", "warehouse123", Role.WarehouseManager, "Кирилл"));
            users.Add(new User("accountant", "accountant123", Role.Accountant, "Фил"));
        }

        private void SaveUsers(List<User> userList)
        {
            string jsonString = JsonSerializer.Serialize(userList);
            File.WriteAllText(usersFilePath, jsonString);
        }

        private List<User> LoadUsers()
        {
            if (File.Exists(usersFilePath))
            {
                string jsonString = File.ReadAllText(usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            return null;
        }

        private string GetLoggedInUserName()
        {
            if (currentUser.Role == Role.PersonnelManager)
            {
       
                return "Имя Сотрудника";
            }

            return currentUser.Login;
        }

        private string GetHiddenPassword()
        {
            string password = "";

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return null;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return password;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
        }

       


    }
   
}