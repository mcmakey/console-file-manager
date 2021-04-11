using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleFileManager
{
    /// <summary>
    /// Дерево файлов
    /// </summary>
    class FilesTree
    {
        /// <summary>
        /// Ссылка на окно информации, для вывода сообщений об искоючениях
        /// </summary>
        private FrameInfo Info { get; }

        /// <summary>
        /// Корневой каталог
        /// </summary>
        public DirectoryInfo Root { get; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="root"></param>
        /// <param name="frameInfo"></param>
        public FilesTree(DirectoryInfo root, FrameInfo frameInfo)
        {
            this.Root = root;
            this.Info = frameInfo;
        }

        /// <summary>
        /// Список файлов входящих в дерево файлов
        /// </summary>
        public List<FilesTreeItem> Items {

            get {
                var items = new List<FilesTreeItem>();
                FillItems(Root, items);
                return items;
            }
        }

        /// <summary>
        /// Рекурсивный метод заполнения списка файлов
        /// </summary>
        /// <param name="root"></param>
        /// <param name="items"></param>
        /// <param name="rank"></param>
        private void FillItems(DirectoryInfo root, List<FilesTreeItem> items, int rank = 0)
        {
            int nextRank;

            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Info.ShowInfoContent(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Info.ShowInfoContent(e.Message);
            }

            // Добавление в список каталога
            items.Add(new FilesTreeItem(root.Name, true, rank));

            // Добавление в файла
            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    items.Add(new FilesTreeItem(fi.Name, false, rank + 1));
                }
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                nextRank = ++rank;
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    FillItems(dirInfo, items, nextRank);
                }
            }
        }
    }
}
