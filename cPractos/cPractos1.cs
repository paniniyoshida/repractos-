using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main()
        {
            string txt = " ";
            do
            {
                float one, two, result, n;
                int s = 3;
                Console.WriteLine("Выберите действие:");
                Console.WriteLine(" ");
                Console.WriteLine("1. Сложить 2 числа");
                Console.WriteLine("2. Вычесть 1-ое из 2-ого");
                Console.WriteLine("3. Перемножить два числа");
                Console.WriteLine("4. Разделить первое на второе");
                Console.WriteLine("5. Возвести в степень N первое число");
                Console.WriteLine("6. Найти квадратный корень из числа");
                Console.WriteLine("7. Найти 1 процент от числа");
                Console.WriteLine("8. Найти факториал из числа");
                Console.WriteLine("9. Выйти из программы");
                Console.WriteLine(" ");
                txt = Convert.ToString(Console.ReadLine());
                if (txt == "1")
                {
                    Console.WriteLine("Введите 1-ое число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    Console.WriteLine("Введите 2-ое число:");
                    two = Convert.ToSingle(Console.ReadLine());
                    result = one + two;
                    Console.WriteLine("Сумма ваших чисел равна " + result);
                    Console.WriteLine(" ");

                }


                else if (txt == "2")
                {
                    Console.WriteLine("Введите 1-ое число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    Console.WriteLine("Введите 2-ое число:");
                    two = Convert.ToSingle(Console.ReadLine());
                    result = one - two;
                    Console.WriteLine("Разность ваших чисел равна " + result);
                    Console.WriteLine(" ");
                }


                else if (txt == "3")
                {
                    Console.WriteLine("Введите 1-ое число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    Console.WriteLine("Введите 2-ое число:");
                    two = Convert.ToSingle(Console.ReadLine());
                    result = one * two;
                    Console.WriteLine("Ответ: " + result);
                    Console.WriteLine(" ");
                }

                else if (txt == "4")
                {
                    Console.WriteLine("Введите 1-ое число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    Console.WriteLine("Введите 2-ое число:");
                    two = Convert.ToSingle(Console.ReadLine());
                    result = one / two;
                    Console.WriteLine("Ответ: " + result);
                    Console.WriteLine(" ");
                }

                else if (txt == "5")
                {
                    Console.WriteLine("Введите число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    Console.WriteLine("Введите степень:");
                    n = Convert.ToSingle(Console.ReadLine());
                    result = (float)Math.Pow(one, n);
                    Console.WriteLine("Ответ: " + result);
                    Console.WriteLine(" ");
                }


                else if (txt == "6")
                {
                    Console.WriteLine("Введите число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    result = (float)Math.Sqrt(one);
                    Console.WriteLine("Ответ: " + result);
                    Console.WriteLine(" ");
                }

                else if (txt == "7")
                {
                    Console.WriteLine("Введите число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    result = one / 100 * 1;
                    Console.WriteLine("Ответ: " + result);
                    Console.WriteLine(" ");
                }


                else if (txt == "8")
                {
                    Console.WriteLine("Введите число:");
                    one = Convert.ToSingle(Console.ReadLine());
                    result = 1;
                    for (var i = 1; i <= one; i++)
                    {
                        result *= i;
                    }
                    Console.WriteLine("Ответ: " + result);
                    Console.WriteLine(" ");
                }

            } while (txt != "9");
            Console.WriteLine(" ");
            Console.WriteLine("Всего хорошего!");

        }

    }
}