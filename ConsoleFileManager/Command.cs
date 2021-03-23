namespace ConsoleFileManager
{
    class Command
    {
        public string Name { get; }
        public string[] Args { get; }
        public Command(string name, string[] args)
        {
            Name = name;
            Args = args;
        }
    }
}
