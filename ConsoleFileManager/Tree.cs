using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    static class Tree
    {

        public static void ttt(DirectoryInfo root)
        {
            WalkDirectoryTree(root, 1);
        }
        private static void WalkDirectoryTree(DirectoryInfo root,int rank) 
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            const string node = "|--";
            string indent = "";
            const string indentStep = "   ";
            
            int nextRank;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                Console.WriteLine(e.Message);
            }

            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            for (int i = 2; i < rank; i++)
            {
                indent += indentStep;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            if (rank == 1)
            {
                Console.WriteLine(root.Name);
            }
            else
            {
                Console.WriteLine($"{indent}{node}{root.Name}");
            }

            if (files != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                indent += indentStep;
                foreach (FileInfo fi in files)
                {
                    Console.WriteLine($"{indent}{node}{fi.Name}");
                }
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                nextRank = ++rank;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    WalkDirectoryTree(dirInfo, nextRank);
                }
            }

            Console.ResetColor();
        }
    }
}
