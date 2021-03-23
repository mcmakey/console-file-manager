using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

        public void RemoveFile()
        {
            Console.WriteLine("RemoveFile");
        }

        public void RemoveDirectory()
        {
            Console.WriteLine("RemoveDirectory");
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
                var command = CommandParser(Console.ReadLine());
                Console.WriteLine();

                switch (command.Name)
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

        static Command CommandParser(string value)
        {
            const char charToTrim = ' ';
            const string delimiter = "[ ]+";
            const int nameIndex = 0;
            const int argsIndex = 1;

            
            string[] splitValue = Regex.Split(value.Trim(charToTrim), delimiter);
            int splitValueLength = splitValue.Length;


            string name = splitValue[nameIndex];

            if (splitValueLength > 1)
            {
                var argsLength = splitValueLength - 1;
                string[] args = new string[argsLength];
                Array.Copy(splitValue, argsIndex, args, 0, argsLength);
                return new Command(name, args);
            }

            return new Command(name, null);

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
