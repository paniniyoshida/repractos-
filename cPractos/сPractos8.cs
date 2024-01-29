using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ConsoleApp2;

namespace ConsoleApp2
{
    public static class Leaderboard
    {
        private const string LeaderboardFilePath = "leaderboard.json";

        public static List<User> LoadLeaderboard()
        {
            if (File.Exists(LeaderboardFilePath))
            {
                string json = File.ReadAllText(LeaderboardFilePath);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(json);
            }

            return new List<User>();
        }

        public static void SaveLeaderboard(List<User> leaderboard)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(leaderboard);
            File.WriteAllText(LeaderboardFilePath, json);
        }

        public static void AddUser(User user)
        {
            List<User> leaderboard = LoadLeaderboard();
            leaderboard.Add(user);
            SaveLeaderboard(leaderboard);
        }

        public static List<User> GetSortedLeaderboard()
        {
            List<User> leaderboard = LoadLeaderboard();
            return leaderboard.OrderByDescending(u => u.CharactersPerMinute).ToList();
        }
    }

    public class TypingTest
    {
        private const int TestDurationSeconds = 60;
        private string textToType;

        public void StartTest()
        {
            Console.WriteLine("Введите свое имя:");
            string name = Console.ReadLine();

            textToType = "Hunter on ghoul, я убил их всех, Уворот от пуль, у меня есть вес, Нафиг граммовка, у меня есть весы, Я не злодей, но у меня свои бесы, Много валюты, имею и песо, Мало хп, я накину им вессел, Много энергии, я будто Тесла, Стреляю так метко, все пули прям в висок.";

            Console.WriteLine("\n" + textToType + "\n");  // Перенос строки перед выводом текста для лучшей читаемости

            Thread timerThread = new Thread(TimerThread);
            timerThread.Start();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ConsoleKeyInfo keyInfo;
            List<char> enteredCharacters = new List<char>();
            int startX = Console.CursorLeft;
            int startY = Console.CursorTop;
            int highlightedCharacters = 0;  // Количество выделенных символов
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter && stopwatch.Elapsed.TotalSeconds < TestDurationSeconds)
            {
                if (keyInfo.Key != ConsoleKey.Backspace)
                {
                    char enteredChar = keyInfo.KeyChar;
                    if (textToType.Contains(enteredChar))
                    {
                        enteredCharacters.Add(enteredChar);
                        Console.SetCursorPosition(startX, startY - 1);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(new string(' ', textToType.Length));
                        Console.SetCursorPosition(startX, startY - 1);
                        Console.Write(new string(enteredCharacters.ToArray()));
                        Console.ResetColor();
                        highlightedCharacters++;
                    }
                    else
                    {
                        Console.SetCursorPosition(startX, startY - 1);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(new string(' ', textToType.Length));
                        Console.SetCursorPosition(startX, startY - 1);
                        Console.Write(new string(enteredCharacters.ToArray()));
                        Console.ResetColor();
                    }
                }
                else if (enteredCharacters.Any())
                {
                    enteredCharacters.RemoveAt(enteredCharacters.Count - 1);
                    Console.SetCursorPosition(startX + enteredCharacters.Count, startY - 1);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(new string(' ', textToType.Length - enteredCharacters.Count));
                    Console.SetCursorPosition(startX, startY - 1);
                    Console.Write(new string(enteredCharacters.ToArray()));
                    Console.ResetColor();
                    highlightedCharacters = Math.Max(0, highlightedCharacters - 1);
                }
                Console.ResetColor();
            }

            stopwatch.Stop();

            int charactersTyped = enteredCharacters.Count;
            double durationMinutes = stopwatch.Elapsed.TotalMinutes;
            double charactersPerMinute = charactersTyped / durationMinutes;
            double charactersPerSecond = charactersTyped / stopwatch.Elapsed.TotalSeconds;

            User user = new User
            {
                Name = name,
                CharactersPerMinute = (int)charactersPerMinute,
                CharactersPerSecond = (int)charactersPerSecond
            };

            Leaderboard.AddUser(user);

            Console.WriteLine("\nТаблица рекордов:");
            List<User> leaderboard = Leaderboard.GetSortedLeaderboard();
            foreach (var u in leaderboard)
            {
                Console.WriteLine($"Имя: {u.Name}, Символов в минуту: {u.CharactersPerMinute}, Символов в секунду: {u.CharactersPerSecond}");
            }
        }

        private void TimerThread()
        {
            Stopwatch timer = Stopwatch.StartNew();
            while (true)
            {
                if (timer.Elapsed.TotalSeconds >= TestDurationSeconds)
                {
                    break;
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"Осталось времени: {TestDurationSeconds - (int)timer.Elapsed.TotalSeconds} сек");
                    Thread.Sleep(500);
                }
            }
            Console.WriteLine();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                TypingTest typingTest = new TypingTest();
                typingTest.StartTest();

                Console.WriteLine("\nХотите пройти тест еще раз? (y/n)");
                string choice = Console.ReadLine();
                if (choice.ToLower() != "y")
                {
                    break;
                }
            }
        }
    }
}