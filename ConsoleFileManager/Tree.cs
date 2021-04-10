using System;
using System.IO;

namespace ConsoleFileManager
{
    static class Tree
    {
        public static void Display(DirectoryInfo root, int rank = 1, int line = 1 /*кост*/) 
        {
            const string dirNode = "■ ";
            const string indentStep = "  ";
            const int leftPosition = 2; /*rjcn*/

            int nextRank;


            int currentLine = line;
            int nextLine;

            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            string indent = "";

            // Устанавливаем курсор в начальную позицию (костыль)
            Console.SetCursorPosition(leftPosition, currentLine);
            //Console.WriteLine(root.FullName);
            //Console.WriteLine();
            // конец костыля

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
                Console.SetCursorPosition(leftPosition, currentLine);
                Console.WriteLine($"{dirNode}{root.Name}");
                
            }
            else
            {
                Console.SetCursorPosition(leftPosition, currentLine);
                Console.WriteLine($"{indent}{dirNode}{root.Name}");
            }

            // кост
            // ++currentLine;

            if (files != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                indent += indentStep;
                foreach (FileInfo fi in files)
                {
                    ++currentLine; /*кост*/
                    Console.SetCursorPosition(leftPosition, currentLine);
                    Console.WriteLine($"{indent}{fi.Name}");
                }
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                nextRank = ++rank;
                nextLine = ++currentLine; /*кост*/
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    Display(dirInfo, nextRank, nextLine);
                }
            }

            Console.ResetColor();
        }
    }
}
