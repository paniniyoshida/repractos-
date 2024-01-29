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
            Console.WriteLine("========== ���� ��������� ��������� ==========");
            Console.WriteLine("1. ����������� ������ �����������");
            Console.WriteLine("2. �������� ����������");
            Console.WriteLine("3. �������� ���������� � ����������");
            Console.WriteLine("4. ������� ����������");
            Console.WriteLine("0. ��������� ���������");
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
            Console.WriteLine("�������� ������ �����������");
            foreach (User user in users)
            {
                if (user.Role != Role.Administrator)
                {
                    Console.WriteLine($"�����: {user.Login}, ����: {user.Role}, ���: {user.Name}");
                }
            }
        }

        private void AddEmployee()
        {
            Console.WriteLine("���������� ����������");

            Console.WriteLine("������� ����� ������ ����������:");
            string login = Console.ReadLine();

            Console.WriteLine("������� ��� ������������:");
            string name = Console.ReadLine();

            Console.WriteLine("������� ������ ������ ����������:");
            string password = GetHiddenPassword();

            Console.WriteLine("�������� ���� ������ ����������:");
            Console.WriteLine("1. ������");
            Console.WriteLine("2. �������� ���������");
            Console.WriteLine("3. �������� ������");
            Console.WriteLine("4. ���������");

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

            Console.WriteLine("��������� ������� ��������.");
        }

        private void UpdateEmployee()
        {
            Console.WriteLine("��������� ���������� � ����������");
            Console.WriteLine("������� ����� ����������, ���������� � ������� ������ ��������:");
            string login = Console.ReadLine();

            User employee = users.Find(u => u.Login == login && u.Role != Role.Administrator);

            if (employee == null)
            {
                Console.WriteLine("��������� �� ������.");
                return;
            }

            Console.WriteLine("�������� ����� ���� ����������:");
            Console.WriteLine("1. ������");
            Console.WriteLine("2. �������� ���������");
            Console.WriteLine("3. �������� ������");
            Console.WriteLine("4. ���������");

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

            Console.WriteLine("���������� � ���������� ������� ��������.");
        }

        private void FireEmployee()
        {
            Console.WriteLine("���������� ����������");
            Console.WriteLine("������� ����� ����������, �������� ������ �������:");
            string login = Console.ReadLine();

            User employee = users.Find(u => u.Login == login && u.Role != Role.Administrator);

            if (employee == null)
            {
                Console.WriteLine("��������� �� ������.");
                return;
            }

            users.Remove(employee);
            Console.WriteLine("��������� ������� ������.");
        }

    }
}