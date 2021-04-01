﻿using System;
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
        /// Выводит список команд приложения
        /// </summary>
        private void Help()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine($"{CommandsNames.Help} - Показать список команд");
            Console.WriteLine($"{CommandsNames.Exit} - Выйти из приложения");
            Console.WriteLine(@$"{CommandsNames.List} path - Отобразить файловую структуру в каталоге находящемся по пути path (ls Disk:\source");
            Console.WriteLine(@$"{CommandsNames.FileInfo} path - Отобразить информацию о файле находящемся по пути path (file Disk:\source\file)");
            Console.WriteLine(@$"{CommandsNames.DirectoryInfo} path - Отобразить информацию о каталоге находящемся по пути path (dir Disk:\source)");
            Console.WriteLine(@$"{CommandsNames.Copy} source destination - копировать файл/каталог из source в destination");
            Console.WriteLine(@"Пример копирование файлов: cp disk:\sourcefile.ext disk:\destfile.ext или cp disk:\sourcefile.ext disk:\destdir");
            Console.WriteLine(@"Пример копирование каталога: cp disk:\source disk:\dest");
            Console.WriteLine(@$"{CommandsNames.Remove} path - удалить файл/каталог находящийся по пути path");
        }

        /// <summary>
        /// Вывод файловой структуры
        /// </summary>
        /// <param name="command"></param>
        private void List(string[] arguments)
        {
            // Проверка наличия аргументов
            if (arguments.Length == 0)
            {
                Console.WriteLine("Неверный формат команды, не указан путь к каталогу");
                return;
            };

            var path = arguments[0];

            // Проверка формата пути
            if (!isPathFormatForrect(path))
            {
                Console.WriteLine("Неверный формат пути к каталогу");
                return;
            }

            // Проверка существования каталога по укзанному пути
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
        /// <param name="command"></param>
        private void DirectoryInfo(Command command)
        {
            // валидация аргументов команды (TODO: Потом для всех команд отделный валидатор)
            Regex pathRegex = new Regex(@"([A-Z]:)?\\.*");

            if (command.Arguments.Length == 0)
            {
                Console.WriteLine("Неверный формат команды, нет пути к каталогу");
                return;
            };

            var path = command.Arguments[0];

            if (!pathRegex.IsMatch(path))
            {
                Console.WriteLine("Неверный формат пути к каталогу");
                return;
            }

            // end validation

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Каталог по указанному пути не существует");
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(path);

            Console.WriteLine("Информация о каталоге:");
            Console.WriteLine($"Наименование: {directory.Name}");
            Console.WriteLine($"Полное наименование: {directory.FullName}");
            Console.WriteLine($"Создание: {directory.CreationTime}");
            Console.WriteLine($"Последнее изменение: {directory.LastWriteTime}");
            Console.WriteLine($"Корневой каталог: {directory.Root}");
            Console.WriteLine();
            Console.WriteLine("Системные атрибуты:");
            DisplaySystemAttrFile(path);
            Console.WriteLine();
        }

        /// <summary>
        /// Отображение информации о файле
        /// </summary>
        /// <param name="command"></param>
        private void FileInfo(Command command)
        {
            // валидация аргументов команды (TODO: Потом для всех команд отделный валидатор)
            Regex pathRegex = new Regex(@"([A-Z]:)?\\.*");

            if (command.Arguments.Length == 0)
            {
                Console.WriteLine("Неверный формат команды, нет пути к файлу.");
                return;
            };

            var path = command.Arguments[0];

            if (!pathRegex.IsMatch(path))
            {
                Console.WriteLine("Неверный формат пути к файлу.");
                return;
            }

            // end validation

            if (!File.Exists(path))
            {
                Console.WriteLine("Файл по указанному пути не найден");
                return;
            }

            FileInfo file = new FileInfo(path);

            Console.WriteLine("Информация о файле:");
            Console.WriteLine($"Наименование - {file.Name}");
            Console.WriteLine($"Каталог - {file.DirectoryName}");
            Console.WriteLine($"Создание - {file.CreationTime}");
            Console.WriteLine($"Последнее изменение - {file.LastWriteTime}");
            Console.WriteLine($"Размер - {file.Length} байт");
            Console.WriteLine();
            Console.WriteLine("Системные атрибуты:");
            DisplaySystemAttrFile(path);
            Console.WriteLine();
        }

        /// <summary>
        /// Копирование файла, каталога
        /// </summary>
        /// <param name="command"></param>
        private void Copy(Command command)
        {
            // валидация аргументов команды (TODO: Потом для всех команд отделный валидатор)
            Regex pathRegex = new Regex(@"([A-Z]:)?\\.*");

            if (command.Arguments.Length < 2)
            {
                Console.WriteLine("Недостаточно аргументов, укажите source и target");
                return;
            };

            var source = command.Arguments[0];
            var dest = command.Arguments[1];

            if (!pathRegex.IsMatch(source))
            {
                Console.WriteLine("Неверный формат пути к источнику.");
                return;
            }

            if (!pathRegex.IsMatch(dest))
            {
                Console.WriteLine("Неверный формат пути. куда нужно копировать.");
                return;
            }

            // end validation

            // Копировать файл или каталог
            if (Path.HasExtension(source))
            {
                // Копирование файла
                CopyFile(source, dest);
            }
            else
            {
                // Копирование каталога
                DirectoryInfo sourceDir = new DirectoryInfo(source);
                DirectoryInfo destinationDir = new DirectoryInfo(dest);

                CopyDirectory(sourceDir, destinationDir);
            }

            // локальный метод копирования файла
            void CopyFile(string source, string dest)
            {
                // Проверка наличия исходного файла по указанному пути
                if (!File.Exists(source))
                {
                    Console.WriteLine("Файл, который нужно скопировать, по указанному пути не найден");
                    return;
                }

                try
                {
                    string fileName;
                    string targetPath;

                    // Если в аргументах не указано имя нового файла, то копируем с именем исходного
                    if (Path.HasExtension(dest))
                    {
                        fileName = Path.GetFileName(dest);
                        targetPath = Path.GetDirectoryName(dest);
                    }
                    else
                    {
                        fileName = Path.GetFileName(source);
                        targetPath = dest;
                    }

                    string sourceFile = source;
                    string destFile = Path.Combine(targetPath, fileName);

                    // Создать новый каталог (если он не существует)
                    Directory.CreateDirectory(targetPath);

                    // Скопировать 
                    File.Copy(sourceFile, destFile, true);

                    // Проверка, точно ли файл скопирован
                    if (File.Exists(destFile))
                    {
                        Console.WriteLine("Файл из");
                        Console.WriteLine(source);
                        Console.WriteLine("в");
                        Console.WriteLine(dest);
                        Console.WriteLine("Скопирован");
                    }
                    else
                    {
                        Console.WriteLine("Что-то пошло не так. Файл не скопирован");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }

            // локальный метод копирования каталога
            void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
            {
                // Проверка что целевой путь - каталог, а не файл
                if (Path.HasExtension(dest))
                {
                    Console.WriteLine("Не надо копировать каталог в файл, проверьте целевой путь на корректность");
                    return;
                }

                if (!destination.Exists)
                {
                    destination.Create();
                }

                // Скопировать все файлы
                FileInfo[] files = source.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.CopyTo(Path.Combine(destination.FullName,
                        file.Name));
                }

                // Обработка подкаталогов
                DirectoryInfo[] dirs = source.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    // Новая целевая папка
                    string destinationDir = Path.Combine(destination.FullName, dir.Name);

                    // Рекурсивный вызов копирования кталога
                    CopyDirectory(dir, new DirectoryInfo(destinationDir));
                }

                Console.WriteLine("Каталог из");
                Console.WriteLine(source);
                Console.WriteLine("в");
                Console.WriteLine(dest);
                Console.WriteLine("Скопирован");
            }
        }

        /// <summary>
        /// Удаление файла, каталога
        /// </summary>
        /// <param name="command"></param>
        private void Remove(Command command)
        {
            Console.WriteLine("Remove");

            // валидация аргументов команды (TODO: Потом для всех команд отделный валидатор)
            Regex pathRegex = new Regex(@"([A-Z]:)?\\.*");

            if (command.Arguments.Length == 0)
            {
                Console.WriteLine("Неверный формат команды, нет пути к файлу или каталогу.");
                return;
            };

            var path = command.Arguments[0];

            if (!pathRegex.IsMatch(path))
            {
                Console.WriteLine("Неверный формат пути к файлу или каталогу.");
                return;
            }

            // end validation

            // Удаление файла или каталога (определяем по наличию расширения файла в пути)
            try
            {
                if (Path.HasExtension(path))
                {

                    if (!File.Exists(path))
                    {
                        Console.WriteLine("Удаляемый Файл, по указанному пути не найден");
                        return;
                    }

                    File.Delete(path);
                    Console.WriteLine($"Файл {Path.GetFileName(path)} из каталога {Path.GetDirectoryName(path)} удален");
                }
                else
                {

                    if (!Directory.Exists(path))
                    {
                        Console.WriteLine("Удаляемый каталог, по указанному пути не найден");
                        return;
                    }

                    Directory.Delete(path, true);
                    Console.WriteLine($"Каталог {path} удален");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        /// <summary>
        /// Завершает работу приложения
        /// </summary>
        private void CloseApp()
        {
            Process.GetCurrentProcess().Kill();
        }

        /////////////

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

                switch (command.Name)
                {
                    case CommandsNames.Help:
                        Help();
                        break;
                    case CommandsNames.Exit:
                        CloseApp();
                        break;
                    case CommandsNames.List:
                        List(command.Arguments);
                        break;
                    case CommandsNames.DirectoryInfo:
                        DirectoryInfo(command);
                        break;
                    case CommandsNames.FileInfo:
                        FileInfo(command);
                        break;
                    case CommandsNames.Copy:
                        Copy(command);
                        break;
                    case CommandsNames.Remove:
                        Remove(command);
                        break;
                    default:
                        Console.WriteLine("Такой команды не существует, попробуйте еще");
                        break;
                }
            }
        }

        ///////////// helpers

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

        private void DisplaySystemAttrFile(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);

            Console.Write("Файл является катлогом - ");
            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }

            Console.WriteLine();

            Console.Write("Файл только для чтения - ");
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }

            Console.WriteLine();

            Console.Write("Файл сжат - ");
            if ((attributes & FileAttributes.Compressed) == FileAttributes.Compressed)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }

            Console.WriteLine();

            Console.Write("Файл зашифрован - ");
            if ((attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }

            Console.WriteLine();

            Console.Write("Файл скрытый - ");
            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }

            Console.WriteLine();

            Console.Write("Файл является системным - ");
            if ((attributes & FileAttributes.System) == FileAttributes.System)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }

            Console.WriteLine();

            Console.Write("Файл временный - ");
            if ((attributes & FileAttributes.Temporary) == FileAttributes.Temporary)
            {
                Console.Write("да");
            }
            else
            {
                Console.Write("нет");
            }
        }

        private bool isPathFormatForrect(string path)
        {
            Regex pathRegex = new Regex(@"([A-Z,a-z]:)?\\.*");

            return pathRegex.IsMatch(path);
        }
    }
}
