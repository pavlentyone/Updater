using System;
using System.IO;
using System.Diagnostics;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[3]
            {
                 @"\\10.3.1.164\D$\TrainsManagement-hatelinq2\TrainsManagement\bin\Debug\net5.0",
                 @"D:\\TrainsManagement\TrainsManagement\bin\Debug\net5.0-windows",
                 "TrainsManagement.exe"
            };

            if (args.Length < 3)
            {
                Console.WriteLine($"Amounts of arguments should 3 or more");
                Console.Read();
                return;
            }

            Updater.updatePath = args[0];
            Updater.currentPath = args[1];
            Updater.exeFile = args[2];

            var fullExeName = $"{Updater.currentPath}\\{Updater.exeFile}";

            var check1 = Directory.Exists(Updater.updatePath);
            var check2 = Directory.Exists(Updater.currentPath);
            var check3 = args.Length >= 3 ? 
                File.Exists(fullExeName) || File.Exists($"{Updater.updatePath}\\{Updater.exeFile}") : 
                false;

            Console.WriteLine($"Folder from\n{check1}\nFolder to\n{check2}\nFile exe\n{check3}");

            if (!check1 || !check2 || !check3)
            {
                Console.Read();
                return;
            }

            var isInUse = Updater.IsFileInUse(fullExeName);

            if (isInUse)
            {
                Console.WriteLine($"Folder from\n{check1}\nFolder to\n{check2}\nFile exe\n{check3}");
                Console.Read();
                return;
            }

            var sw = new Stopwatch();

            sw.Start();
            Updater.ReplaceFiles();
            sw.Stop();
            Console.WriteLine($"Sync: {sw.ElapsedMilliseconds}");

            //sw.Start();
            //Updater.ReplaceFilesAsync().GetAwaiter().GetResult();
            //sw.Stop();
            //Console.WriteLine($"Async: {sw.ElapsedMilliseconds}");

            if(check3)
                Updater.OpenFile(fullExeName);
        }
    }
}