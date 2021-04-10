using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace ConsoleFileManager
{
    class FileManager
    {
        private int appWindowWidth = Console.LargestWindowWidth;
        private int appWindowHeight = Console.LargestWindowHeight;

        private const int commandFrameHeight = 5;
        private const int InfoFrameHeight = 15;

        private CommandFrame CommandFrame = new CommandFrame(Console.LargestWindowHeight - commandFrameHeight - 1, commandFrameHeight); // TODO: Console.LargestWindowHeight => appWindowHeight getter наверное // хз но -1 нужно чтобы clean не стирал последнюю строку
        private InfoFrame InfoFrame = new InfoFrame(Console.LargestWindowHeight - commandFrameHeight - InfoFrameHeight, InfoFrameHeight); // TODO: Console.LargestWindowHeight => appWindowHeight getter наверное

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
            Display();
            CommandProccesing();
        }

        /*** Приватные методы ***/

        private void Display()
        {
            // Установка размеров окна приложения
            Console.SetWindowSize(appWindowWidth, appWindowHeight);
            Console.SetBufferSize(appWindowWidth, appWindowHeight);

            // Отрисовка окна командной строки 
            CommandFrame.Dispaly();

            // Отрисовка окна информации 
            InfoFrame.Dispaly();
        }

        /// <summary>
        /// Выводит список команд приложения
        /// </summary>
        private void Help()
        {
            string[] commandDescriptions =
            {
                "Список команд:",
                $"{CommandsNames.Help} - Показать список команд",
                $"{CommandsNames.Exit} - Выйти из приложения",
                @$"{CommandsNames.List} path - Отобразить файловую структуру в каталоге находящемся по пути path (ls Disk:\source)",
                @$"{CommandsNames.FileInfo} path - Отобразить информацию о файле находящемся по пути path (file Disk:\source\file)",
                @$"{CommandsNames.DirectoryInfo} path - Отобразить информацию о каталоге находящемся по пути path (dir Disk:\source)",
                @$"{CommandsNames.Copy} source destination - копировать файл/каталог из source в destination",
                @"Пример копирование файлов: cp disk:\sourcefile.ext disk:\destfile.ext или cp disk:\sourcefile.ext disk:\destdir",
                @"Пример копирование каталога: cp disk:\source disk:\dest",
                @$"{CommandsNames.Remove} path - удалить файл/каталог находящийся по пути path"
            };

            InfoFrame.ShowInfoContent(commandDescriptions);
        }

        /// <summary>
        /// Вывод файловой структуры
        /// </summary>
        /// <param name="source"></param>
        private void List(string source)
        {
            // Проверка существования каталога по укзанному пути
            if (!Directory.Exists(source))
            {
                InfoFrame.ShowInfoContent("Каталог по указанному пути не существует");
                return;
            }

            // отображение дерева элементов
            Console.WriteLine($"List {source}");
            Console.WriteLine();

            DirectoryInfo rootDirInfo = new DirectoryInfo($"{source}");
            Tree.Display(rootDirInfo);
        }

        /// <summary>
        /// Отображение информации о каталоге
        /// </summary>
        /// <param name="source"></param>
        private void DirectoryInfo(string source)
        {
            // Проверка существования каталога по укзанному пути
            if (!Directory.Exists(source))
            {
                InfoFrame.ShowInfoContent("Каталог по указанному пути не существует");
                return;
            }

            // Получение информации о каталоге
            DirectoryInfo directory = new DirectoryInfo(source);

            var directoryAttributes = new string[]
            {
                "Информация о каталоге:",
                $"Наименование: {directory.Name}",
                $"Полное наименование: {directory.FullName}",
                $"Создание: {directory.CreationTime}",
                $"Последнее изменение: {directory.LastWriteTime}",
                $"Корневой каталог: {directory.Root}"
            };

            var systemDirectoryAttributes = getSystemAttrFileText(source);

            var info = directoryAttributes.Concat(systemDirectoryAttributes).ToArray();

            // Отобразить информацию в окне
            InfoFrame.ShowInfoContent(info);
        }

        /// <summary>
        /// Отображение информации о файле
        /// </summary>
        /// <param name="source"></param>
        private void FileInfo(string source)
        {
            // Проверка существования файла по укзанному пути
            if (!File.Exists(source))
            {
                InfoFrame.ShowInfoContent("Файл по указанному пути не найден");
                return;
            }

            // Получение информации о файле
            FileInfo file = new FileInfo(source);

            var fileAttributes = new string[]
            {
                "Информация о файле:",
                $"Наименование - {file.Name}",
                $"Каталог - {file.DirectoryName}",
                $"Создание - {file.CreationTime}",
                $"Последнее изменение - {file.LastWriteTime}",
                $"Размер - {file.Length} байт",

            };

            var fileSystemAttributes = getSystemAttrFileText(source);
            var info = fileAttributes.Concat(fileSystemAttributes).ToArray();

            // Отобразить информацию в окне
            InfoFrame.ShowInfoContent(info);
        }

        /// <summary>
        /// Копирование файла, каталога
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        private void Copy(string source, string dest)
        {
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
                    InfoFrame.ShowInfoContent("Файл, который нужно скопировать, по указанному пути не найден");
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
                        InfoFrame.ShowInfoContent($"Файл из {source} cкопирован в {dest}");
                    }
                    else
                    {
                        InfoFrame.ShowInfoContent("Что-то пошло не так. Файл не скопирован");
                    }
                }
                catch (Exception e)
                {
                    InfoFrame.ShowInfoContent($"The process failed: {e.ToString()}");
                }
            }

            // локальный метод копирования каталога
            void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
            {
                // Проверка что целевой путь - каталог, а не файл
                if (Path.HasExtension(dest))
                {
                    InfoFrame.ShowInfoContent("Не надо копировать каталог в файл, проверьте целевой путь на корректность");
                    return;
                }

                // Проверка что целевой каталог существует, если нет то создается
                if (!destination.Exists)
                {
                    destination.Create();
                }

                // Скопировать все файлы
                FileInfo[] files = source.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.CopyTo(Path.Combine(destination.FullName, file.Name));
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

                InfoFrame.ShowInfoContent($"Каталог из {source} скопирован в {dest}");
            }
        }

        /// <summary>
        /// Удаление файла, каталога
        /// </summary>
        /// <param name="source"></param>
        private void Remove(string source)
        {
            // Удаление файла или каталога (определяем по наличию расширения файла в пути)
            try
            {
                if (Path.HasExtension(source))
                {

                    if (!File.Exists(source))
                    {
                        InfoFrame.ShowInfoContent("Удаляемый Файл, по указанному пути не найден");
                        return;
                    }

                    File.Delete(source);
                    InfoFrame.ShowInfoContent($"Файл {Path.GetFileName(source)} из каталога {Path.GetDirectoryName(source)} удален");
                }
                else
                {

                    if (!Directory.Exists(source))
                    {
                        InfoFrame.ShowInfoContent("Удаляемый каталог, по указанному пути не найден");
                        return;
                    }

                    Directory.Delete(source, true);
                    InfoFrame.ShowInfoContent("Удаляемый каталог, по указанному пути не найден");
                }
            }
            catch (Exception e)
            {
                InfoFrame.ShowInfoContent($"The process failed: {e.ToString()}");
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
                CommandFrame.CommandLineReady();
                var command = CommandParser(Console.ReadLine());

                switch (command.Name)
                {
                    case CommandsNames.Help:
                        Help();
                        break;
                    case CommandsNames.Exit:
                        CloseApp();
                        break;
                    case CommandsNames.List:
                        List(command.Source);
                        break;
                    case CommandsNames.DirectoryInfo:
                        DirectoryInfo(command.Source);
                        break;
                    case CommandsNames.FileInfo:
                        FileInfo(command.Source);
                        break;
                    case CommandsNames.Copy:
                        Copy(command.Source, command.Destination);
                        break;
                    case CommandsNames.Remove:
                        Remove(command.Source);
                        break;
                    default:
                        InfoFrame.ShowInfoContent("Некорректный ввод (имя команды или количество/формат аргументов команды), попробуйте еще (см. help)");
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

            // массив строковых значений из строки ввода
            string[] splitValue = Regex.Split(value.Trim(charToTrim), delimiter);
            var splitValueLength = splitValue.Length;

            // наименование команды
            string commandName = splitValue[nameIndex];

            // массив аргументов команды (без имени)
            var argsLength = splitValueLength - 1;
            string[] args = new string[argsLength];
            Array.Copy(splitValue, argsIndex, args, 0, argsLength);


            if (commandName == CommandsNames.Help || commandName == CommandsNames.Exit)
            {
                return new Command(commandName);
            }

            var isSingleArgumentCommand = (commandName == CommandsNames.FileInfo) ||
                (commandName == CommandsNames.DirectoryInfo) ||
                (commandName == CommandsNames.Remove);

            if ((commandName == CommandsNames.List) &&
                args.Length != 0 &&
                isPathFormatСorrect(args[0])
            )
            {
                return new Command(commandName, args[0]);
            }
            else if (isSingleArgumentCommand &&
                args.Length != 0 &&
                isPathFormatСorrect(args[0])
            )
            {
                return new Command(commandName, args[0]);
            }
            else if ((commandName == CommandsNames.Copy) &&
                args.Length == 2 &&
                (isPathFormatСorrect(args[0]) && isPathFormatСorrect(args[1]))
            )
            {
                return new Command(commandName, args[0], args[1]);
            }
            else
            {
                return new Command(null); // TODO: ?
            }
        }

        /// <summary>
        /// Метод возвращает текстовый массив системных атрибутов 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] getSystemAttrFileText(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);

            // Проверка системнго атрибута, возвр. текст (да/нет)
            string CheckAttrText(FileAttributes attributes, FileAttributes attr)
            {
                return (attributes & attr) == attr ? "да" : "нет";
            }

            return new string[]
            {
                $"Файл является каталогом - {CheckAttrText(attributes, FileAttributes.Directory)}",
                $"Файл только для чтения - {CheckAttrText(attributes, FileAttributes.ReadOnly)}",
                $"Файл сжат - {CheckAttrText(attributes, FileAttributes.Compressed)}",
                $"Файл зашифрован - {CheckAttrText(attributes, FileAttributes.Encrypted)}",
                $"Файл является системным - {CheckAttrText(attributes, FileAttributes.System)}",
                $"Файл скрытый - {CheckAttrText(attributes, FileAttributes.Hidden)}",
                $"Файл временный - {CheckAttrText(attributes, FileAttributes.Temporary)}"
            };
        }

        private bool isPathFormatСorrect(string path)
        {
            Regex pathRegex = new Regex(@"([A-Z,a-z]:)?\\.*");
            return pathRegex.IsMatch(path);
        }
    }
}
