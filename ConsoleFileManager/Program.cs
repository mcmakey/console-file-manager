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
            var root = appSettings[AppConstants.ConfigKeys.LastRoot];
            var file = appSettings[AppConstants.ConfigKeys.LastFile];

            FileManager fileManager = new FileManager(root, file);
            fileManager.Start();
        }
    }
}
