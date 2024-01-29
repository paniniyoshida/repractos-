using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class HotKey
{
    public ConsoleKey Key { get; set; }
    public string Action { get; set; }
    public string FilePath { get; set; }
}

public static class HotKeyManager
{
    public static List<HotKey> HotKeys { get; set; }

    public static void LoadHotKeys()
    {
        if (File.Exists("hotkeys.json"))
        {
            string json = File.ReadAllText("hotkeys.json");
            HotKeys = JsonConvert.DeserializeObject<List<HotKey>>(json);
        }
        else
        {
            HotKeys = new List<HotKey>();
        }
    }

    public static void SaveHotKeys()
    {
        string json = JsonConvert.SerializeObject(HotKeys);
        File.WriteAllText("hotkeys.json", json);
    }
}

public static class Menu
{
    public static int ShowMenu(string[] options)
    {
        Console.Clear();
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }

        ConsoleKeyInfo keyInfo = Console.ReadKey();
        ConsoleKey key = keyInfo.Key;

        if (key == ConsoleKey.F1)
        {
            return -1;
        }
        else if (key == ConsoleKey.F2)
        {
            return -2;
        }
        else if (key == ConsoleKey.F10)
        {
            return -10;
        }
        else if (key == ConsoleKey.Backspace)
        {
            return -99;
        }
        else if (Enum.TryParse(keyInfo.KeyChar.ToString(), out ConsoleKey pressedKey))
        {
            return (int)pressedKey;
        }
        else
        {
            return 0;
        }
    }
}

public class HotKeyManagerApp
{
    private static List<string> hotKeyOptions = new List<string>() { "Создать горячую клавишу", "Горячая клавиша редактирования", "Удалить горячую клавишу", "Выполнить горячую клавишу", "Выход" };

    public static void Main(string[] args)
    {
        HotKeyManager.LoadHotKeys();

        int menuOption = 0;
        while (menuOption != 5)
        {
            menuOption = Menu.ShowMenu(hotKeyOptions.ToArray());

            switch (menuOption)
            {
                case 1:
                    CreateHotKey();
                    break;
                case 2:
                    EditHotKey();
                    break;
                case 3:
                    DeleteHotKey();
                    break;
                case 4:
                    ExecuteHotKey();
                    break;
                case -1:
                    CreateHotKeyFromMenu();
                    break;
                case -2:
                    EditHotKeyFromMenu();
                    break;
                case -10:
                    ReturnToMenu();
                    break;
                case -99:
                    menuOption = 5;
                    break;
                default:
                    Console.WriteLine("Недопустимый параметр. Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                    break;
            }
        }

        HotKeyManager.SaveHotKeys();
    }

    private static void CreateHotKey()
    {
        Console.Clear();
        Console.WriteLine("Создать горячую клавишу");

        Console.Write("Введите горячую клавишу (например, A, B, F1, F2, Backspace)");
        ConsoleKey key = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), Console.ReadLine(), true);

        Console.Write("Приступайте к действию: ");
        string action = Console.ReadLine();

        Console.Write("Введите путь к файлу: ");
        string filePath = Console.ReadLine();

        HotKey hotKey = new HotKey { Key = key, Action = action, FilePath = filePath };
        HotKeyManager.HotKeys.Add(hotKey);

        Console.WriteLine("Горячая клавиша создана. Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    private static void EditHotKey()
    {
        Console.Clear();
        Console.WriteLine("Горячая клавиша редактирования");

        Console.WriteLine("Выберите горячую клавишу для редактирования: ");

        int index = 1;
        foreach (HotKey hotKey in HotKeyManager.HotKeys)
        {
            Console.WriteLine($"{index}. {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            index++;
        }

        Console.Write("Введите номер горячей клавиши для редактирования: ");
        int hotKeyIndex = int.Parse(Console.ReadLine()) - 1;

        if (hotKeyIndex >= 0 && hotKeyIndex < HotKeyManager.HotKeys.Count)
        {
            HotKey hotKey = HotKeyManager.HotKeys[hotKeyIndex];

            Console.WriteLine($"Горячая клавиша редактирования: {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            Console.WriteLine("Нажмите клавишу F1 для редактирования действия, клавишу F2 для редактирования пути к файлу или пробел назад для отмены.");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;

            if (key == ConsoleKey.F1)
            {
                Console.WriteLine("Введите новое действие: ");
                string newAction = Console.ReadLine();
                hotKey.Action = newAction;
                Console.WriteLine("Действие горячей клавиши обновлено. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else if (key == ConsoleKey.F2)
            {
                Console.WriteLine("Enter new file path: ");
                string newFilePath = Console.ReadLine();
                hotKey.FilePath = newFilePath;
                Console.WriteLine("Обновлен путь к файлу с помощью горячей клавиши. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else if (key == ConsoleKey.Backspace)
            {
                Console.WriteLine("Редактирование отменено. Нажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Нажата недопустимая клавиша. Редактирование отменено. Нажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Неверный номер горячей клавиши. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private static void DeleteHotKey()
    {
        Console.Clear();
        Console.WriteLine("Удалить горячую клавишу");

        Console.WriteLine("Выберите горячую клавишу для удаления:");

        int index = 1;
        foreach (HotKey hotKey in HotKeyManager.HotKeys)
        {
            Console.WriteLine($"{index}. {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            index++;
        }

        Console.Write("Введите номер горячей клавиши для удаления: ");
        int hotKeyIndex = int.Parse(Console.ReadLine()) - 1;

        if (hotKeyIndex >= 0 && hotKeyIndex < HotKeyManager.HotKeys.Count)
        {
            HotKey hotKey = HotKeyManager.HotKeys[hotKeyIndex];
            HotKeyManager.HotKeys.Remove(hotKey);
            Console.WriteLine("Горячая клавиша удалена. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Неверный номер горячей клавиши. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private static void ExecuteHotKey()
    {
        Console.Clear();
        Console.WriteLine("Выполнить горячую клавишу");

        Console.WriteLine("Выберите горячую клавишу для выполнения:");

        int index = 1;
        foreach (HotKey hotKey in HotKeyManager.HotKeys)
        {
            Console.WriteLine($"{index}. {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            index++;
        }

        Console.Write("Введите номер горячей клавиши для выполнения: ");
        int hotKeyIndex = int.Parse(Console.ReadLine()) - 1;

        if (hotKeyIndex >= 0 && hotKeyIndex < HotKeyManager.HotKeys.Count)
        {
            HotKey hotKey = HotKeyManager.HotKeys[hotKeyIndex];
            Console.WriteLine($"Выполнение горячей клавиши: {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Неверный номер горячей клавиши. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private static void CreateHotKeyFromMenu()
    {
        Console.Clear();
        Console.WriteLine("Создать горячую клавишу");

        Console.Write("Введите горячую клавишу (например, A, B, F1, F2, Backspace).: ");
        ConsoleKey key = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), Console.ReadLine(), true);

        Console.Write("Приступайте к действию: ");
        string action = Console.ReadLine();
        Console.Write("Введите путь к файлу: ");
        string filePath = Console.ReadLine();

        HotKey hotKey = new HotKey { Key = key, Action = action, FilePath = filePath };
        HotKeyManager.HotKeys.Add(hotKey);

        Console.WriteLine("Горячая клавиша создана. Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    private static void EditHotKeyFromMenu()
    {
        Console.Clear();
        Console.WriteLine("Горячая клавиша редактирования");

        Console.WriteLine("Выберите горячую клавишу для редактирования:");

        int index = 1;
        foreach (HotKey hotKey in HotKeyManager.HotKeys)
        {
            Console.WriteLine($"{index}. {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            index++;
        }

        Console.Write("Введите номер горячей клавиши для редактирования: ");
        int hotKeyIndex = int.Parse(Console.ReadLine()) - 1;

        if (hotKeyIndex >= 0 && hotKeyIndex < HotKeyManager.HotKeys.Count)
        {
            HotKey hotKey = HotKeyManager.HotKeys[hotKeyIndex];

            Console.WriteLine($"Горячая клавиша редактирования: {hotKey.Key} - {hotKey.Action} - {hotKey.FilePath}");
            Console.WriteLine("Нажмите клавишу F1 для редактирования действия, клавишу F2 для редактирования пути к файлу или пробел назад для отмены.");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;

            if (key == ConsoleKey.F1)
            {
                Console.WriteLine("Введите новое действие:");
                string newAction = Console.ReadLine();
                hotKey.Action = newAction;
                Console.WriteLine("Действие горячей клавиши обновлено. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else if (key == ConsoleKey.F2)
            {
                Console.WriteLine("Введите новый путь к файлу: ");
                string newFilePath = Console.ReadLine();
                hotKey.FilePath = newFilePath;
                Console.WriteLine("Обновлен путь к файлу с помощью горячей клавиши. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            else if (key == ConsoleKey.Backspace)
            {
                Console.WriteLine("Редактирование отменено. Нажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Нажата недопустимая клавиша. Редактирование отменено. Нажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Неверный номер горячей клавиши. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    private static void ReturnToMenu()
    {
        Console.Clear();
        Console.WriteLine("Возврат в главное меню. Нажмите любую клавишу, чтобы продолжить...");
        Console.ReadKey();
    }
}