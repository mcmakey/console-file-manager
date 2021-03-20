using System;

namespace ConsoleFileManager
{
    class FileManager
    {
        public FileManager()
        {

        }

        public void Init()
        {
            Console.WriteLine("Console file manager!");
        }

        public void List()
        {
            Console.WriteLine("List");
        }

        public void CopyFile()
        {
            Console.WriteLine("CopyFile");
        }

        public void CopyDirectory()
        {
            Console.WriteLine("CopyDirectory");
        }

        public void FileInfo()
        {
            Console.WriteLine("FileInfo");
        }

        public void DirInfo()
        {
            Console.WriteLine("DirInfo");
        }
    }
}
