using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    static class Tree
    {

        public static void ttt(DirectoryInfo root)
        {
            WalkDirectoryTree(root, 0);
        }
        private static void WalkDirectoryTree(DirectoryInfo root,int rank) 
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            
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

            //// my
            ///
            Console.WriteLine($"{rank} {root.Name}");

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    Console.WriteLine(fi.Name);
                }

                // Now find all the subdirectories under this directory.
                
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                nextRank = ++rank;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, nextRank);
                }
            }
        }
    }
}
