using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightcontrols
{
    class FileMonitor : LightControlInterface
    {
        public void Read(string input)
        {
            throw new NotImplementedException();
        }

        public void Write(string output)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to.
                Console.WriteLine("Creating file {0}", path);
            }

            File.WriteAllText(path, output + "\n");
        }

        public FileMonitor(SerialLineHandler sh)
        {
            serialLineHandler = sh;
            watcher = new FileSystemWatcher();
            Directory.CreateDirectory(folder);
            if (!File.Exists(path)) {
                using (FileStream fs = File.Create(path)) { }
            }
            
            watcher.Path = folder;

            /* Watch for changes in LastAccess and LastWrite times, and
           the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.cl";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            // Begin watching.
            ClearFile(path);
            watcher.EnableRaisingEvents = true;
        }

        SerialLineHandler serialLineHandler;
        FileSystemWatcher watcher;

        private string folder = @"c:\temp";
        private string path = @"c:\temp\lightcontrol.cl";

        private void ClearFile(string filetoClear)
        {
            //watcher.Changed -= new FileSystemEventHandler(OnChanged); // Disable onChanged event
            watcher.EnableRaisingEvents = false;
            File.WriteAllText(filetoClear, String.Empty); // Clear file
            watcher.EnableRaisingEvents = true;
            //watcher.Changed += new FileSystemEventHandler(OnChanged); // Enable onChanged event
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            try
            {
                string[] input = File.ReadAllLines(e.FullPath);
                ClearFile(e.FullPath);
                foreach (string line in input)
                {
                    serialLineHandler.Write(line);
                }
            }catch (IOException)
            {
                Console.Write("File busy..");
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

    }
}
