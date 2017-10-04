using System;
using System.IO;
using System.Threading;

namespace lightcontrols
{
    class FileMonitor : LightControlInterface, IDisposable
    {
        Thread t;
        bool run = false;
        public bool Read(string path)
        {
            Thread.Sleep(1);
            try
            {
                string[] data = File.ReadAllLines(path);
                ClearFile(path);
                foreach (string line in data)
                {
                    serialLineHandler.Write(line);
                    Thread.Sleep(10);
                }
                return true;
            }
            catch (IOException) {
                Console.WriteLine("File busy.. Retry");
                Read(path);
                return false;
            }            
        }

        public bool Write(string output)
        {
            throw new NotImplementedException();
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

            Start();
        }

        ~FileMonitor()
        {
            Stop();
            ClearFile(path);
            serialLineHandler = null;
            Dispose();
        }

        SerialLineHandler serialLineHandler;
        FileSystemWatcher watcher;

        private string folder = @"c:\temp";
        private string path = @"c:\temp\lightcontrol.cl";

        /// <summary>
        /// Add event handlers, and start monitoring the file
        /// </summary>
        public void Start()
        {       
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);
            
            ClearFile(path);
            run = true;
            t = new Thread(new ThreadStart(ReadFile));
            t.IsBackground = true;
            t.Start();
            //watcher.EnableRaisingEvents = true;
            Console.WriteLine("FileMonitor started");
        }

        public void ReadFile()
        {
            while (run)
            {
                Read(path);
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Remove event handlers to stop monitoring of the file
        /// </summary>
        public void Stop()
        {
            run = false;
            //watcher.EnableRaisingEvents = false;
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);
            t.Join();
            Console.WriteLine("FileMonitor stopped");
        }
        
        /// <summary>
        /// Clear the file to get a fresh start
        /// </summary>
        /// <param name="filetoClear"></param>
        private void ClearFile(string filetoClear)
        {
            watcher.EnableRaisingEvents = false;
            File.WriteAllText(filetoClear, String.Empty); // Clear file
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            Read(e.FullPath);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public void Dispose()
        {
            watcher.Dispose();
        }
    }
}
