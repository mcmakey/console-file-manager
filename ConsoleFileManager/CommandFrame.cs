using System;

namespace ConsoleFileManager
{
    /// <summary>
    /// Класс командного окна
    /// </summary>
    class CommandFrame : Frame
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="topPosition"></param>
        /// <param name="height"></param>
        public CommandFrame(int topPosition, int height) : base(topPosition, height)
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
            // Очистка окна от предыдущей команды // TODO: fix delete bottom border frame
            Clean();

            // установка курсора в начальную позицию комантной строки
            var commandLineTopPosition = TopPosition;
            Console.SetCursorPosition(leftPosition, commandLineTopPosition);
            Console.Write("Введите команду('help - список команд')");
            Console.SetCursorPosition(leftPosition, commandLineTopPosition + 1);
            Console.Write("> ");
        }
    }
}
