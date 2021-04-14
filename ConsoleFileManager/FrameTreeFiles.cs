using System;
using System.Collections.Generic;

namespace ConsoleFileManager
{
    /// <summary>
    /// Фрейм дерева файлов (наследует класс "Frame")
    /// </summary>
    class FrameTreeFiles : Frame
    {
        public FrameTreeFiles(int topPosition, int height) : base(topPosition, height)
        {

        }

        /// <summary>
        /// Отображение дерева файлов
        /// </summary>
        /// <param name="files"></param>
        public void DisplayTree(string path, List<FilesTreeItem> files, int page)
        {
            const string directoryIcon = "■ ";
            const string indentStep = "  ";
            const int bottomOffset = 1; // Отступ снизу - строка для указания страницы например

            var contentTopPosition = TopPosition + 1;
            var treeTopPosition = contentTopPosition + 1;

            // доступное количество строк во фрейме для отображения дерева
            var availabelTreeHeight = Height - treeTopPosition - bottomOffset;

            // Параметры для  пагинации
            var maxNumberPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(files.Count) / Convert.ToDecimal(availabelTreeHeight)));
            if (page > maxNumberPages)
            {
                page = maxNumberPages;
            };
            var startFilesIndex = GetStartIndex(page, files.Count, availabelTreeHeight);
            var endFilesIndex = GetEndIndex(page, files.Count, availabelTreeHeight, maxNumberPages);

            // Очистка экрана
            Clean();

            // Вывод заголовка (путь к корневому каталогу дерева)
            Console.SetCursorPosition(leftPosition, contentTopPosition);
            Console.Write($@"{path}:");

            // Вывод дерева файлов
            var currentLine = treeTopPosition;

            for (int i = startFilesIndex; i < endFilesIndex; i++)
            {
                // Отступы для отображения вложенности
                var indent = "";
                for (int j = 0; j < files[i].Rank; j++)
                {
                    indent += indentStep;
                }

                // Подсветка типа файла (каталог/файл)
                if (files[i].IsDirectory)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                // Иконка (каталога если каталог)
                var itemIcon = files[i].IsDirectory ? directoryIcon : null;

                // Вывод пункта дерева файлов
                Console.SetCursorPosition(leftPosition, currentLine);
                Console.Write($"{indent}{itemIcon}{files[i].Name}");

                currentLine++;
                Console.ResetColor();
            }

            // Отобржение текущей/максимальной страниц
            Console.WriteLine();
            var paginationInfo = $"Страница {page} из {maxNumberPages}";
            var xPos = Console.LargestWindowWidth / 2 - paginationInfo.Length / 2;
            var yPos = Height - contentTopPosition - bottomOffset;
            Console.SetCursorPosition(xPos, yPos);
            Console.WriteLine(paginationInfo);

            // Лок. методы получения начального/конечного индекса списка файлов для отображения в окне
            int GetStartIndex(int page, int filesCount, int maxLines)
            {
                if (filesCount > maxLines && page > 1)
                {
                    return maxLines * (page - 1) - 1;
                } 

                return 0;
            }

            int GetEndIndex(int page, int filesCount, int maxLines, int maxPages)
            {
                if (page != maxPages)
                {
                    return maxLines * page - 1;
                }

                return filesCount - 1;
            }
        }
    }
}
