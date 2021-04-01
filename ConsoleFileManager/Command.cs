namespace ConsoleFileManager
{
    /// <summary>
    /// Класс для команд вводимых пользователем в командной строке (имя команды и аргументы, если есть)
    /// </summary>
    class Command
    {
        public string Name { get; }
        public string[] Arguments { get; }

        public Command(string name, string[] args)
        {
            this.Name = name;
            this.Arguments = args;
        }

        public Command(string name)
        {
            this.Name = name;
        }
    }
}
