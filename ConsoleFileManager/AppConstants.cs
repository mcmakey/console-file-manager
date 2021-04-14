namespace ConsoleFileManager
{
    /// <summary>
    /// Константы приложения
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Наименования команд
        /// </summary>
        public static class Commands
        {
            public const string Help = "help";
            public const string Exit = "ex";
            public const string List = "ls";
            public const string ListPageArgument = "-p";
            public const string DirectoryInfo = "dir";
            public const string FileInfo = "file";
            public const string Copy = "cp";
            public const string Remove = "rm";
        }

        /// <summary>
        /// Имена ключей конфига
        /// </summary>
        public static class ConfigKeys
        {
            public const string LastRoot = "root";
            public const string LastFile = "file";
        }
    }
}
