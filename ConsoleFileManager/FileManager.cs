using System;
using System.Diagnostics;

namespace ConsoleFileManager
{
    class FileManager
    {
        public FileManager()
        {

        }

        public void Start()
        {
            Console.WriteLine("Console file manager!");
            Console.WriteLine("Список команд - 'help'");
            Console.WriteLine("Выйти из приложения - 'exit'");
            CommandProccesing();
        }

        /*** Публичные методы класса ***/

        public void List()
        {
            Console.WriteLine("List");
        }

        public void CopyFile()
        {
            Console.WriteLine("CopyFile");
        }

        public void CopyDirectory()
        {
            Console.WriteLine("CopyDirectory");
        }

        public void FileInfo()
        {
            Console.WriteLine("FileInfo");
        }

        public void DirInfo()
        {
            Console.WriteLine("DirInfo");
        }

        /*** Статичные методы класса ***/

        static void CommandProccesing()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("> ");
                var command = Console.ReadLine();
                Console.WriteLine();

                switch (command)
                {
                    case "help":
                        Help();
                        break;
                    case "exit":
                        CloseApp();
                        break;
                    default:
                        Console.WriteLine("Такой команды не существует, попробуйте еще");
                        break;
                }
            }
        }
        static void Help()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine("help - Показать список команд");
            Console.WriteLine("exit - Выйти из приложения");
        }

        static void CloseApp()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
