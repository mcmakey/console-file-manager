namespace ConsoleFileManager
{
    /// <summary>
    /// Файл (файл или директория) rank - уовень вложенности от указанного корня.
    /// </summary>
    class FilesTreeItem
    {
        public string Name { get; }
        public bool IsDirectory { get; }
        public int Rank { get; }

        public FilesTreeItem(string name, bool isDirectory, int rank)
        {
            this.Name = name;
            this.IsDirectory = isDirectory;
            this.Rank = rank;
        }
    }
}
