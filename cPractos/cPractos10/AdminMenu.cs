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
            Console.WriteLine("========== ���� �������������� ==========");
            Console.WriteLine("1. ����������� ������ �������������");
            Console.WriteLine("2. �������� ������������");
            Console.WriteLine("3. �������� ������������");
            Console.WriteLine("4. ������� ������������");
            Console.WriteLine("5. ����� ������������");
            Console.WriteLine("0. ��������� ���������");
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
            Console.WriteLine("========== ������ ������������� ==========");
            foreach (var user in users)
            {
                Console.WriteLine($"�����: {user.Login} | ����: {user.Role} | ���: {user.Name}");
            }
            Console.WriteLine("==========================================");
            Console.WriteLine("������� ����� ������� ��� �����������...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void AddUser()
        {
            Console.WriteLine("���������� ������������");
            Console.WriteLine("������� �����:");
            string login = Console.ReadLine();

            // �������� ������� ������������ � ����� �� �������
            if (users.Any(u => u.Login == login))
            {
                Console.WriteLine("������������ � ����� ������� ��� ����������.");
                Console.WriteLine("������� ����� ������� ��� �����������...");
                Console.ReadKey();
                Console.Clear();
                DisplayAdminMenu();
                return;
            }

            Console.WriteLine("������� ������:");
            string password = GetHiddenPassword();

            Console.WriteLine("������� ��� ������������:");
            string name = Console.ReadLine();

            Console.WriteLine("�������� ���� (1 - �������������, 2 - ������, 3 - �������� ���������, 4 - �����-��������, 5 - ���������):");
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
                    Console.WriteLine("������������ ���� ����.");
                    Console.WriteLine("������� ����� ������� ��� �����������...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayAdminMenu();
                    return;
            }

            User newUser = new User(login, password, role, name);
            users.Add(newUser);
            SaveUsers(users);
            Console.WriteLine("������������ ������� ��������.");
            Console.WriteLine("������� ����� ������� ��� �����������...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void UpdateUser()
        {
            Console.WriteLine("��������� ������������");
            Console.WriteLine("������� ����� ������������, �������� ������ ��������:");
            string login = Console.ReadLine();

            User userToUpdate = users.Find(u => u.Login == login);

            if (userToUpdate == null)
            {
                Console.WriteLine("������������ �� ������.");
                Console.WriteLine("������� ����� ������� ��� �����������...");
                Console.ReadKey();
                Console.Clear();
                DisplayAdminMenu();
                return;
            }

            Console.WriteLine("��������, ��� ������ �������� (1 - �����, 2 - ������, 3 - ����, 4 - ���):");
            ConsoleKeyInfo updateKey = Console.ReadKey();

            switch (updateKey.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("\n������� ����� �����:");
                    string newLogin = Console.ReadLine();

                    // �������� ������� ������������ � ����� �� �������
                    if (users.Any(u => u.Login == newLogin))
                    {
                        Console.WriteLine("������������ � ����� ������� ��� ����������.");
                        Console.WriteLine("������� ����� ������� ��� �����������...");
                        Console.ReadKey();
                        Console.Clear();
                        DisplayAdminMenu();
                        return;
                    }

                    userToUpdate.Login = newLogin;
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("\n������� ����� ������:");
                    string newPassword = GetHiddenPassword();
                    userToUpdate.Password = newPassword;
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("\n�������� ����� ���� (1 - �������������, 2 - ������, 3 - �������� ���������, 4 - �����-��������, 5 - ���������):");
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
                            Console.WriteLine("������������ ���� ����.");
                            Console.WriteLine("������� ����� ������� ��� �����������...");
                            Console.ReadKey();
                            Console.Clear();
                            DisplayAdminMenu();
                            return;
                    }

                    userToUpdate.Role = newRole;
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("\n������� ����� ��� ������������:");
                    string newName = Console.ReadLine();
                    userToUpdate.Name = newName;
                    break;
                default:
                    Console.WriteLine("������������ �����.");
                    Console.WriteLine("������� ����� ������� ��� �����������...");
                    Console.ReadKey();
                    Console.Clear();
                    DisplayAdminMenu();
                    return;
            }

            SaveUsers(users);
            Console.WriteLine("������������ ������� �������.");
            Console.WriteLine("������� ����� ������� ��� �����������...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void DeleteUser()
        {
            Console.WriteLine("�������� ������������");
            Console.WriteLine("������� ����� ������������, �������� ������ �������:");
            string login = Console.ReadLine();

            User userToDelete = users.Find(u => u.Login == login);

            if (userToDelete == null)
            {
                Console.WriteLine("������������ �� ������.");
                Console.WriteLine("������� ����� ������� ��� �����������...");
                Console.ReadKey();
                Console.Clear();
                DisplayAdminMenu();
                return;
            }

            users.Remove(userToDelete);
            SaveUsers(users);
            Console.WriteLine("������������ ������� ������.");
            Console.WriteLine("������� ����� ������� ��� �����������...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }

        private void SearchUser()
        {
            Console.WriteLine("����� ������������");
            Console.WriteLine("������� ����� ������������:");
            string login = Console.ReadLine();

            User user = users.Find(u => u.Login == login);

            if (user == null)
            {
                Console.WriteLine("������������ �� ������.");
            }
            else
            {
                Console.WriteLine($"�����: {user.Login} | ����: {user.Role} | ���: {user.Name}");
            }

            Console.WriteLine("������� ����� ������� ��� �����������...");
            Console.ReadKey();
            Console.Clear();
            DisplayAdminMenu();
        }
    }
}