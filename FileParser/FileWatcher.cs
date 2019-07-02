using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace FileParser
{
    public class FileWatcher
    {
        static string Path { get; set; }
        static MenuHelper menuHelper;
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
        }

        //private static void OnRenamed(object source, RenamedEventArgs e) =>
        //// Specify what is done when a file is renamed.
        //Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}
