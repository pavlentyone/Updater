using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Updater
{
    public static class Updater
    {
        public static string updatePath = "C:\\Users\\USeR-050716\\Desktop\\net5.0-windows";
        public static string currentPath = Directory.GetCurrentDirectory();
        public static string exeFile = "TrainsManagement.exe";
        public async static Task ReplaceFilesAsync()
        {

            var updateTask = Task.Run(() =>
                (from f in Directory.GetFiles(updatePath)
                 select new FileInfo(f))
                .ToArray());

            var currentTask = Task.Run(() =>
                (from f in Directory.GetFiles(currentPath)
                 select new FileInfo(f))
                .ToArray());

            var updateFiles = await updateTask;
            var currentFiles = await currentTask;

            Parallel.ForEach(updateFiles, 
                (file) => file.CopyTo(currentPath + "\\" + file.Name, true));
        }

        public static void ReplaceFiles()
        {
            var updateFiles = 
                (from f in Directory.GetFiles(updatePath)
                 select new FileInfo(f))
                .ToArray();

            var currentFiles = 
                (from f in Directory.GetFiles(currentPath)
                 select new FileInfo(f))
                .ToArray();

            foreach(var file in updateFiles)
                file.CopyTo(currentPath + "\\" + file.Name, true);
        }

        // Just open file safely. Returns excaption if file is busy
        public static Exception OpenFile(string filename)
        {
            try
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo(filename)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
            catch (Exception exc)
            {
                return exc;
            }

            return null;
        }

        // Checks if file is in use
        public static bool IsFileInUse(string filename)
        {
            try
            {
                File.Open(
                  filename, FileMode.OpenOrCreate,
                  FileAccess.Read, FileShare.None)
                  .Close();
            }
            catch
            { return true; }

            return false;
        }
    }
}
