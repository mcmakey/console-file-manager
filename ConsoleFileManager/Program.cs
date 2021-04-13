using System.Configuration;

namespace ConsoleFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            AppInit();
        }

        static void AppInit()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var root = appSettings["root"];
            var file = appSettings["file"];
            FileManager fileManager = new FileManager(root, file);
            fileManager.Start();
        }
    }
}
