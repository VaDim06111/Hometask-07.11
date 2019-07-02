using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    class MenuHelper
    {
        string n;
        TextAssistent textAssistent = new TextAssistent();
        public MenuHelper()
        {
        }
        public void WriteMenu()
        {
            Console.WriteLine("Choose what to do:");
            Console.WriteLine("1- Parsing by sentences \n2- Parsing by words \n3- Parsing by punctuation marks \n4- Get sorted number of uses of the word \n5- Get longest sentence by symbols \n6- Get shortest sentence by count words \n7- Get the most common letter \n8- Write results to file(items 5-7) \nExit- another key");
            n = Console.ReadLine();
            var timer = new Stopwatch();
            timer.Start();
            UserChose(n);
            timer.Stop();
            Console.WriteLine($"Program finished at {timer.ElapsedMilliseconds} milliseconds");
            timer.Reset();
        }
        public void UserChose(string n)
        {
            switch (n)
            {
                case "1":
                    Task task1 = new Task(() => textAssistent.GetSentences());
                    task1.Start();
                    task1.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "2":
                    Task task2 = new Task(() => textAssistent.GetWords());
                    task2.Start();
                    task2.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "3":
                    Task task3 = new Task(() => textAssistent.GetPhrases());
                    task3.Start();
                    task3.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "4":
                    Task task4 = new Task(() => textAssistent.GetListOfWordsWithCountOfRepeat());
                    task4.Start();
                    task4.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "5":
                    Task task5 = new Task(() => textAssistent.GetLongestSentenceBySymbols());
                    task5.Start();
                    task5.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "6":
                    Task task6 = new Task(() => Console.WriteLine($"The shortest sentence is: {textAssistent.GetShortestSentencebyWords()}"));
                    task6.Start();
                    task6.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "7":
                    Task task7 = new Task(() => Console.WriteLine(textAssistent.GetMostPopularLetter()));
                    task7.Start();
                    task7.Wait();
                    Console.WriteLine("Task finished");
                    break;
                case "8":
                    Task task8 = new Task(() => textAssistent.WriteToFileResult());
                    task8.Start();
                    task8.Wait();
                    Console.WriteLine("File recorded!");
                    Console.WriteLine("Task finished");
                    break;                   
                default:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
