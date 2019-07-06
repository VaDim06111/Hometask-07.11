using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;
using Newtonsoft.Json;

namespace FileParser
{
    public class FileWatcher
    {
        static string Path { get; set; }
        static MenuHelper menuHelper;
        static DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\..");
        public FileWatcher(MenuHelper _menuHelper)
        {
            menuHelper = _menuHelper;
        }
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void StartWatch()
        {
            // Create a new FileSystemWatcher and set its properties. 
            Path = AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\..";
            FileSystemWatcher watcher = new FileSystemWatcher(Path);

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.DirectoryName
                | NotifyFilters.FileName;

            // Only watch text files.
            watcher.Filter = "*.txt";

            // Add event handlers.
            // watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            // watcher.Deleted += OnChanged;
            // watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            GetStateDirectory(directoryInfo.GetFiles("*.txt")).Wait();
        }

        public async Task CheckInstance()
        {           
            string strFiles;
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\stateDirectory.json",Encoding.UTF8))
            {
               strFiles = await sr.ReadLineAsync();
            }
            var files = JsonConvert.DeserializeObject<FileInfo[]>(strFiles);
            if(files != directoryInfo.GetFiles("*.txt"))
            {
                var newAddedFiles = directoryInfo.GetFiles("*.txt").Union(files);
                foreach (var file in newAddedFiles)
                {
                    if(File.Exists(file.FullName))
                    if(file.Name!= "historyOfNewWordsAdded.txt" && file.Name != "sample.txt" && file.Name != "sampleInfo.txt" && file.Name != "sampleWordsCount.txt")
                    {
                        Task task = new Task(() => menuHelper.textAssistent.GetListOfWordsWithCountOfRepeatInDirectoryTextFiles(file.FullName));
                        task.Start();
                        task.Wait();
                    }
                }
            }
        }
        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            
            if (e.Name != "historyOfNewWordsAdded.txt")
            {
                Task task = new Task(() => menuHelper.textAssistent.GetListOfWordsWithCountOfRepeatInDirectoryTextFiles(e.FullPath));
                task.Start();
                task.Wait();
                Console.WriteLine($"Task with added file {e.Name} finished");
            }
            GetStateDirectory(directoryInfo.GetFiles("*.txt")).Wait();
        }
        public static async Task GetStateDirectory(FileInfo[] files)
        {
            var result = JsonConvert.SerializeObject(files);
            using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\.." + "\\.." + @"\stateDirectory.json", false, Encoding.UTF8))
            {
                await sw.WriteLineAsync(result);
            }
        }
        //private static void OnRenamed(object source, RenamedEventArgs e) =>
        //// Specify what is done when a file is renamed.
        //Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}
