using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleFileManager
{
    class FileManager
    {
        /*** Конструктор ***/
        public FileManager()
        {

        }

        /*** Публичные методы ***/
        /// <summary>
        /// Начало работы класса
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Console file manager!");
            Console.WriteLine("Список команд - 'help'");
            Console.WriteLine("Выйти из приложения - 'exit'");
            CommandProccesing();
        }

        /*** Приватные методы ***/
        /// <summary>
        /// Вывод файловой структуры
        /// </summary>
        /// <param name="args"></param>
        private void List(string[] args)
        {

            // валидация аргументов

            //
            var path = args[0];

            // отображение дерева элементов
            Console.WriteLine($"List {path}");
        }

        private void CopyFile()
        {
            Console.WriteLine("CopyFile");
        }

        private void CopyDirectory()
        {
            Console.WriteLine("CopyDirectory");
        }

        private void RemoveFile()
        {
            Console.WriteLine("RemoveFile");
        }

        private void RemoveDirectory()
        {
            Console.WriteLine("RemoveDirectory");
        }

        private void FileInfo()
        {
            Console.WriteLine("FileInfo");
        }

        private void DirInfo()
        {
            Console.WriteLine("DirInfo");
        }

        /// <summary>
        /// командная строка приложения
        /// </summary>
        private void CommandProccesing()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("> ");
                var command = CommandParser(Console.ReadLine());
                Console.WriteLine();

                switch (command.name)
                {
                    case "help":
                        Help();
                        break;
                    case "exit":
                        CloseApp();
                        break;
                    case "ls":
                        List(command.args);
                        break;
                    default:
                        Console.WriteLine("Такой команды не существует, попробуйте еще");
                        break;
                }
            }
        }

        /// <summary>
        /// Парсит введенное занчение в комадной строке и возвращает экземпляр класса "Command"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Command CommandParser(string value)
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

            return new Command(name);

        }

        /// <summary>
        /// Выводит список команд приложения
        /// </summary>
        private void Help()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine("help - Показать список команд");
            Console.WriteLine("exit - Выйти из приложения");
            Console.WriteLine("ls Disk:Source -pn - Отобразить файловую структуру в папке Source на диске Disk, n - номер страницы пейджинга");
        }

        /// <summary>
        /// Завершает работу приложения
        /// </summary>
        private void CloseApp()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
