using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleFileManager
{
    class FilesTree
    {
        private DirectoryInfo Root { get; }
        public List<File> Files {get; set;}

        public FilesTree(DirectoryInfo root)
        {
            this.Root = root;
        }
    }
}
