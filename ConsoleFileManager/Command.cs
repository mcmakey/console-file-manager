namespace ConsoleFileManager
{
    /// <summary>
    /// Класс для команд вводимых пользователем в командной строке (имя команды и аргументы, если есть)
    /// </summary>
    class Command
    {
        public string Name { get; }
        public string Source { get; }
        public string Destination { get; }
        public int Page { get; }

        /// <summary>
        /// Конструктор для команд без аргументов (Exit, Help)
        /// </summary>
        /// <param name="name"></param>
        public Command(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Конструктор для команды List
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="page"></param>
        public Command(string name, string source, int page)
        {
            this.Name = name;
            this.Source = source;
            this.Page = page;
        }

        /// <summary>
        /// Конструктор для команд FileInfo, DirectoryInfo, Remove
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        public Command(string name, string source)
        {
            this.Name = name;
            this.Source = source;
        }

        /// <summary>
        /// Конструктор для команды Copy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public Command(string name, string source, string destination)
        {
            this.Name = name;
            this.Source = source;
            this.Destination = destination;
        }
    }
}
