using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    /// <summary>
    /// Окно дерева файлов
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
        public void DisplayTree(string path,  List<FilesTreeItem> files)
        {
            const string directoryIcon = "■ ";
            const string indentStep = "  ";

            var contentTopPosition = TopPosition + 1;
            var treeCurrentLinePosition = contentTopPosition + 1;

            
            // Очистка экрана
            Clean();

            // Вывод заголовка (путь к корневому каталогу дерева)
            Console.SetCursorPosition(leftPosition, contentTopPosition);
            Console.Write($@"{path}:");

            // Вывод дерева файлов
            foreach (var file in files)
            {
                // Отступы для отображения вложенности
                var indent = "";
                for (int i = 0; i < file.Rank; i++)
                {
                    indent += indentStep;
                }

                // Иконка (каталога если каталог)
                var itemIcon = file.IsDirectory ? directoryIcon : null;

                Console.SetCursorPosition(leftPosition, treeCurrentLinePosition);

                Console.Write($"{indent}{itemIcon}{file.Name}");

                treeCurrentLinePosition++;
            }
        }
    }
}
