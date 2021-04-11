namespace ConsoleFileManager
{
    class TreeItem
    {
        public string Name { get; }
        public bool IsDirectory { get; }
        public int Rank { get; }

        public TreeItem(string name, bool isDirectory, int rank)
        {
            this.Name = name;
            this.IsDirectory = isDirectory;
            this.Rank = rank;
        }
    }
}
