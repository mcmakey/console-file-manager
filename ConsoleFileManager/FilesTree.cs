﻿using System;
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
        public DirectoryInfo Root { get; }

        public FilesTree(DirectoryInfo root)
        {
            this.Root = root;
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
                Console.WriteLine(e.Message); // TODO infoFrame
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message); // TODO infoFrame
            }

            // add directory
            items.Add(new FilesTreeItem(root.Name, true, rank));

            // add file
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
