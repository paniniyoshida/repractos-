using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Serialization;
using ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        List<Human> humans = new List<Human>();

        Console.WriteLine("Какой файл читаем?");
        string path = Console.ReadLine();

        if (path.EndsWith(".txt"))
        {
            string[] lines = File.ReadAllLines(path);



        }
        else if (path.EndsWith(".json"))
        {
            string json = File.ReadAllText(path);
            humans = JsonSerializer.Deserialize<List<Human>>(json);
        }
        else if (path.EndsWith(".xml"))
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Human>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                humans = (List<Human>)xml.Deserialize(fs);
            }
        }

        foreach (var item in humans)
        {
            Console.WriteLine(item.Name);
            Console.WriteLine(item.Age);
            Console.WriteLine(item.MyColor);
        }

        Console.WriteLine("Куда и в какой формат сохраняем?");
        path = Console.ReadLine();

        if (path.EndsWith(".txt"))
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (var item in humans)
                {
                    writer.WriteLine($"{item.Name},{item.Age},{item.MyColor}");
                }
            }
        }
        else if (path.EndsWith(".json"))
        {
            string json = JsonSerializer.Serialize(humans);
            File.WriteAllText(path, json);
        }
        else if (path.EndsWith(".xml"))
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Human>));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                xml.Serialize(fs, humans);
            }
        }

        Console.WriteLine("Конвертация завершена.");
    }
}
