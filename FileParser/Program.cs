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
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\dictionary.json"))
            {
                Task taskDictionaryLoad = new Task(() => menuHelper.textAssistent.GetListOfWordsWithCountOfRepeat());
                taskDictionaryLoad.Start();
            }
            Task taskCheckNewFiles = new Task(() => fileWatcher.CheckInstance());
            taskCheckNewFiles.Start();
            Task taskWatcher = new Task(()=>fileWatcher.StartWatch());
            taskWatcher.Start();
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
    }
}
