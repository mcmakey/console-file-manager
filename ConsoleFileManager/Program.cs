using System.Configuration;

namespace ConsoleFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            AppInit();
        }

        /// <summary>
        /// Инициализация приложения
        /// </summary>
        static void AppInit()
        {
            // получение данных из конфига
            var appSettings = ConfigurationManager.AppSettings;
            var root = appSettings[AppConstants.ConfigKeys.LastRoot];
            var file = appSettings[AppConstants.ConfigKeys.LastFile];

            // создание экземпляра класса приложения и его запуск.
            FileManager fileManager = new FileManager(root, file);
            fileManager.Start();
        }
    }
}
