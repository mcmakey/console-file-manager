using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleFileManager
{
    class FilesTree
    {
        public DirectoryInfo Root { get; }

        public FilesTree(DirectoryInfo root)
        {
            this.Root = root;
        }

        public List<TreeItem> Items {

            get {
                var items = new List<TreeItem>();

                FillItems(Root, items);

                return items;
            }
        }

        private void FillItems(DirectoryInfo root, List<TreeItem> items, int rank = 1)
        {
            items.Add(new TreeItem("dir", true, 1));
            items.Add(new TreeItem("file-1", false, 2));
            items.Add(new TreeItem("file-2", false, 2));
        }
    }
}
