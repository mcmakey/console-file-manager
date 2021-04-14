namespace ConsoleFileManager
{
    /// <summary>
    /// Пункт дерева файлов (каталог или файл)
    /// </summary>
    class FilesTreeItem
    {
        public string Name { get; }
        public bool IsDirectory { get; }

        // Уровень вложенности (от корневого каталога дерева)
        public int Rank { get; }

        public FilesTreeItem(string name, bool isDirectory, int rank)
        {
            this.Name = name;
            this.IsDirectory = isDirectory;
            this.Rank = rank;
        }
    }
}
