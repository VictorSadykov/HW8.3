using System;
using System.IO;

namespace HW8._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\test";
            long startDirSize = GetDirectorySize(path);
            var dirInfo = DeleteUnderMinutes(path, 30);
            long newDirSize = dirInfo.Item1;
            int fileDeleteCounter = dirInfo.Item2;

            Console.WriteLine($"Папка по пути {path}\nИсходный размер папки: {startDirSize} байт\nРезультирующий размер папки: {newDirSize} байт\nУдалено элементов: {fileDeleteCounter}");

        }
        static (long, int) DeleteUnderMinutes(string path, int minutes, long filesSize = 0, int fileCounter = 0)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists == true)
            {
                var dirs = directory.GetDirectories();
                var files = directory.GetFiles();

                foreach (var dir in dirs)
                {
                    (long var1, int var2) = DeleteUnderMinutes(dir.FullName, minutes, filesSize, fileCounter);
                    filesSize += var1;
                    fileCounter += var2;
                    if (DateTime.Now - dir.CreationTime < TimeSpan.FromMinutes(30))
                    {
                        try
                        {
                            dir.Delete();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }


                foreach (var file in files)
                {
                    if (DateTime.Now - file.CreationTime < TimeSpan.FromMinutes(30))
                    {
                        try
                        {
                            filesSize += file.Length;
                            fileCounter++;
                            file.Delete();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }
            }

            return (filesSize, fileCounter);


        }


        static long GetDirectorySize(string path, long output = 0)
        {
            var directory = new DirectoryInfo(path);


            if (directory.Exists)
            {
                var dirs = directory.GetDirectories();
                var files = directory.GetFiles();

                foreach (var file in files)
                {
                    try
                    {
                        output += file.Length;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                foreach (var dir in dirs)
                {
                    try
                    {
                        output += GetDirectorySize(dir.FullName, output);

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }
                }


            }

            return output;
        }
    }
}
