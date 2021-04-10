namespace ConsoleFileManager
{
    class File
    {
        public string Name { get; }
        public bool IsDirectory { get; }
        public int Rank { get; }

        public File(string name, bool isDirectory, int rank)
        {
            this.Name = name;
            this.IsDirectory = isDirectory;
            this.Rank = rank;
        }
    }
}
