using System;

namespace ConsoleFileManager
{
    /// <summary>
    /// Класс командного окна
    /// </summary>
    class FrameCommand : Frame
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="topPosition"></param>
        /// <param name="height"></param>
        public FrameCommand(int topPosition, int height) : base(topPosition, height)
        {
            
        }

        /// <summary>
        /// показ заголовка в командном окне
        /// </summary>
        /// <param name="title"></param>
        public void ShowTitle(string title)
        {
            Console.SetCursorPosition(leftPosition, TopPosition);
            Console.WriteLine(title);
        }

        /// <summary>
        /// Подготовка комантной строки к вводу новой команды
        /// </summary>
        public void CommandLineReady()
        {
            // Очистка окна от предыдущей команды
            Clean();

            // установка курсора в начальную позицию комантной строки
            var commandLineContentTopPosition = TopPosition + 1;
            Console.SetCursorPosition(leftPosition, commandLineContentTopPosition);
            Console.WriteLine("Введите команду('help - список команд')");
            Console.SetCursorPosition(leftPosition, commandLineContentTopPosition + 1);
            Console.Write("> ");
        }
    }
}
