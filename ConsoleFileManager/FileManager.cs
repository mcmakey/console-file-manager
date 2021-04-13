using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Configuration;

namespace ConsoleFileManager
{
    class FileManager
    {
        private static int appWindowWidth = Console.LargestWindowWidth;
        private static int appWindowHeight = Console.LargestWindowHeight;

        private const int commandFrameHeight = 5;
        private const int infoFrameHeight = 15;
        private static int TreeFrameHeight = appWindowHeight - commandFrameHeight - infoFrameHeight;

        private static int commandFrameTopPosition = appWindowHeight - commandFrameHeight - 1;
        private static int infoFrameTopPosition = appWindowHeight - commandFrameHeight - infoFrameHeight;

        private FrameCommand CommandFrame = new FrameCommand(commandFrameTopPosition, commandFrameHeight);
        private FrameInfo InfoFrame = new FrameInfo(infoFrameTopPosition, infoFrameHeight);
        private FrameTreeFiles TreeFrame = new FrameTreeFiles(0, TreeFrameHeight);

        private string CurrentRoot { get; set; }
        private string CurrentFile { get; set; }

        /*** Конструктор ***/
        public FileManager()
        {

        }

        public FileManager(string root = "", string file = "")
        {
            this.CurrentRoot = root;
            this.CurrentFile = file;
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

            //  Отрисовка окна дерева каталогов и дерева последнее просмотренное в предыдущем сеансе (если в конфиге есть запись).
            TreeFrame.Dispaly();
            if (CurrentRoot != "")
            {
                List(CurrentRoot, 1);
            }

            // Отрисовка окна информации и последнего просмотренного файла в предыдущем сеансе (если в конфиге есть запись).
            InfoFrame.Dispaly();
            if (CurrentFile != "")
            {
                FileInfo(CurrentFile);
            }

            // Отрисовка окна командной строки 
            CommandFrame.Dispaly();
        }

        /// <summary>
        /// Выводит список команд приложения
        /// </summary>
        private void Help()
        {
            string[] commandDescriptions =
            {
                "Список команд:",
                $"{AppConstants.Commands.Help} - Показать список команд",
                @$"{AppConstants.Commands.Exit} - Выйти из приложения",
                @$"{AppConstants.Commands.List} -  Отобразить файловую структуру в каталоге, {AppConstants.Commands.ListPageArgument} - отобразить конкректную страницу этой структуры ({AppConstants.Commands.List} Disk:\source {AppConstants.Commands.ListPageArgument}<number>)",
                @$"{AppConstants.Commands.FileInfo} - Отобразить информацию о файле ({AppConstants.Commands.FileInfo} Disk:\source\file)",
                @$"{AppConstants.Commands.DirectoryInfo} - Отобразить информацию о каталоге ({AppConstants.Commands.DirectoryInfo} Disk:\source)",
                @$"{AppConstants.Commands.Copy} - копировать файл/каталог из в ({AppConstants.Commands.Copy} Disk:\source Disk:\dest)",
                @$"{AppConstants.Commands.Remove} - удалить файл/каталог ({AppConstants.Commands.Remove} Disk:\source)"
            };

            InfoFrame.ShowInfoContent(commandDescriptions);
        }

        /// <summary>
        /// Вывод файловой структуры
        /// </summary>
        /// <param name="source"></param>
        private void List(string source, int page = 1)
        {
            // Проверка существования каталога по укзанному пути
            if (!Directory.Exists(source))
            {
                InfoFrame.ShowInfoContent($"Каталог по указанному пути {source} не существует.");
                return;
            }

            // Отрисовка дерева каталогов
            DirectoryInfo rootDirInfo = new DirectoryInfo($"{source}");

            FilesTree filesTree = new FilesTree(rootDirInfo, InfoFrame);
            TreeFrame.DisplayTree(rootDirInfo.FullName, filesTree.Items, page);

            // Сохранить значение текущего корневого каталога дерева 
            CurrentRoot = source;
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
                InfoFrame.ShowInfoContent($"Каталог по указанному пути {source} не существует");
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

            // Сохранить значение о текущем корневом каталоге
            CurrentRoot = source;
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
                InfoFrame.ShowInfoContent($"Файл по указанному пути {source} не найден");
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

            // Сохранить значение последнего файла о котором запрашивалась информация 
            CurrentFile = source;
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
            // Сохранить информацию в конфиг о текущем корневом каталоге и последнем просмотретнном файле
            UpdateAppSettings(AppConstants.ConfigKeys.LastRoot, CurrentRoot);
            UpdateAppSettings(AppConstants.ConfigKeys.LastFile, CurrentFile);

            // Завершить
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
                    case AppConstants.Commands.Help:
                        Help();
                        break;
                    case AppConstants.Commands.Exit:
                        CloseApp();
                        break;
                    case AppConstants.Commands.List:
                        List(command.Source, command.Page);
                        break;
                    case AppConstants.Commands.DirectoryInfo:
                        DirectoryInfo(command.Source);
                        break;
                    case AppConstants.Commands.FileInfo:
                        FileInfo(command.Source);
                        break;
                    case AppConstants.Commands.Copy:
                        Copy(command.Source, command.Destination);
                        break;
                    case AppConstants.Commands.Remove:
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
            const string pathPattern = @"([A-Z,a-z]:)?\\.*";

            var pageArgumentPattern = @$"{AppConstants.Commands.ListPageArgument}\d+";

            // массив строковых значений из строки ввода
            string[] splitValue = Regex.Split(value.Trim(charToTrim), delimiter);
            var splitValueLength = splitValue.Length;

            // наименование команды
            string commandName = splitValue[nameIndex];

            // массив аргументов команды (без имени)
            var argsLength = splitValueLength - 1;
            string[] args = new string[argsLength];
            Array.Copy(splitValue, argsIndex, args, 0, argsLength);

            // является ли команда команlой с оним аргументом ? 
            var isSingleArgumentCommand = (commandName == AppConstants.Commands.FileInfo) ||
                (commandName == AppConstants.Commands.DirectoryInfo) ||
                (commandName == AppConstants.Commands.Remove);

            // Комадны без аргументов (помощь, выход)
            if (commandName == AppConstants.Commands.Help || commandName == AppConstants.Commands.Exit)
            {
                return new Command(commandName);
            }

            // Команда с "показать дерево файлов" с одним обязательным и одним необязательным аргументами
            if ((commandName == AppConstants.Commands.List))
            {
                if (args.Length == 1 && CheckMatchesPattern(args[0], pathPattern))
                {
                    Command ListCommand = new Command(AppConstants.Commands.List);
                    ListCommand.Source = args[0];
                    ListCommand.Page = 1;
                    return ListCommand;
                }
                else if (args.Length > 1 &&
                  CheckMatchesPattern(args[0], pathPattern) &&
                  CheckMatchesPattern(args[1], pageArgumentPattern))
                {
                    Command ListCommand = new Command(AppConstants.Commands.List);
                    ListCommand.Source = args[0];
                    ListCommand.Page = Convert.ToInt32(args[1].Replace(AppConstants.Commands.ListPageArgument, ""));
                    return ListCommand;
                };

                // если количество аргументов и их формат не соотв. требованиям, то пустая команда - для сообщю об ошибке ввода
                return new Command(null);
            };

            // Команды с одним обязательным аргументом
            if (isSingleArgumentCommand &&
                args.Length != 0 &&
                CheckMatchesPattern(args[0], pathPattern)
            )
            {
                return new Command(commandName, args[0]);
            }
            else if ((commandName == AppConstants.Commands.Copy) &&
                args.Length == 2 &&
                CheckMatchesPattern(args[0], pathPattern) && 
                CheckMatchesPattern(args[1], pathPattern)
            )
            {
                return new Command(commandName, args[0], args[1]);
            };

            // Пустая команда (для вывода сообщения об ошибке при вводе команды)
            return new Command(null);

            // Локальный метод проверки соответсвия строки зданнаму шаблону
            bool CheckMatchesPattern(string text, string pattern)
            {
                Regex pathRegex = new Regex(pattern);
                return pathRegex.IsMatch(text);
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

        // 
        private void UpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                InfoFrame.ShowInfoContent("Произошла ошибка записи настроек");
            }
        }
    }
}
