using System;
using System.IO;

namespace ConsoleFileManager
{
    static class Tree
    {
        public static void Display(DirectoryInfo root, int rank = 1) 
        {
            const string dirNode = "■ ";
            const string indentStep = "  ";

            int nextRank;

            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            string indent = "";

            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }

            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            for (int i = 1; i < rank; i++)
            {
                indent += indentStep;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            if (rank == 1)
            {
                Console.WriteLine($"{dirNode}{root.Name}");
            }
            else
            {
                Console.WriteLine($"{indent}{dirNode}{root.Name}");
            }

            if (files != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                indent += indentStep;
                foreach (FileInfo fi in files)
                {
                    Console.WriteLine($"{indent}{fi.Name}");
                }
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                nextRank = ++rank;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    Display(dirInfo, nextRank);
                }
            }

            Console.ResetColor();
        }
    }
}
