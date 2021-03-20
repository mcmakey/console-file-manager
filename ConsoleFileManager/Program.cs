using System;

namespace ConsoleFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            AppInit();
        }

        static void AppInit()
        {
            FileManager fileManager = new FileManager();
            fileManager.Init();
        }
    }
}
