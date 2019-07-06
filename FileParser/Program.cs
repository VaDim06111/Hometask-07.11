using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 1;
            MenuHelper menuHelper = new MenuHelper();
            FileWatcher fileWatcher = new FileWatcher(menuHelper);           
            DeleteFileHistory();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\stateDirectory.json"))
            {
                Task taskWatcher1 = new Task(() => fileWatcher.CheckInstance().Wait());
                taskWatcher1.Start();
            }
            Task taskWatcher2 = new Task(()=>fileWatcher.StartWatch());
            taskWatcher2.Start();
            while (a == 1)
            {
                try
                {
                    menuHelper.WriteMenu();                   
                    Console.WriteLine("Wish to continue? - Press 1");
                    a = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                }
                catch (Exception)
                {
                    Console.WriteLine("This item wasn't found! Check the input data!");
                }
            }
            Task.WaitAll();            
        }
        static public void DeleteFileHistory()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\historyOfNewWordsAdded.txt";
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }
    }
}
