﻿namespace ConsoleFileManager
{
    /// <summary>
    /// Класс для команд вводимых пользователем в командной строке (имя команды и аргументы, если есть)
    /// </summary>
    class Command
    {
        public string name { get; }
        public string[] args { get; }

        public Command(string name, string[] args)
        {
            this.name = name;
            this.args = args;
        }

        public Command(string name)
        {
            this.name = name;
        }
    }
}
