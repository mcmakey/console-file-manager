using System;
using System.Diagnostics;
using System.IO;
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
        private void List(Command command)
        {
            // валидация аргументов команды (TODO: Потом для всех команд отделный валидатор)
            Regex pathRegex = new Regex(@"([A-Z]:)?\\.*");

            if (command.args.Length == 0)
            {
                Console.WriteLine("Неверный формат команды, нет пути к каталогу");
                return;
            };

            var path = command.args[0];

            if (!pathRegex.IsMatch(path))
            {
                Console.WriteLine("Неверный формат пути к каталогу");
                return;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Каталог по указанному пути не существует");
                return;
            }

            // отображение дерева элементов
            Console.WriteLine($"List {path}");

            DirectoryInfo rootDirInfo = new DirectoryInfo($"{path}");
            Tree.Display(rootDirInfo);
        }

        /// <summary>
        /// Отображение информации о каталоге
        /// </summary>
        private void DirectoryInfo(Command command)
        {
            // валидация аргументов команды (TODO: Потом для всех команд отделный валидатор)
            Regex pathRegex = new Regex(@"([A-Z]:)?\\.*");

            if (command.args.Length == 0)
            {
                Console.WriteLine("Неверный формат команды, нет пути к каталогу");
                return;
            };

            var path = command.args[0];

            if (!pathRegex.IsMatch(path))
            {
                Console.WriteLine("Неверный формат пути к каталогу");
                return;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Каталог по указанному пути не существует");
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(path);

            Console.WriteLine($"Название каталога: {directory.Name}");
            Console.WriteLine($"Полное название каталога: {directory.FullName}");
            Console.WriteLine($"Время создания каталога: {directory.CreationTime}");
            Console.WriteLine($"Корневой каталог: {directory.Root}");
        }

        private void FileInfo()
        {
            Console.WriteLine("FileInfo");
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
                        List(command);
                        break;
                    case "dir":
                        DirectoryInfo(command);
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
            Console.WriteLine(@"ls path - Отобразить файловую структуру в каталоге находящемся по пути path (ls Disk:\source");
            Console.WriteLine(@"dir path - Отобразить информацию о каталоге находящемся по пути path (dir Disk:\source)");
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
