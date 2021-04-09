using System;
using System.Collections.Generic;
using System.Text;

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
            // Очистка строк
            var commandLineTopPosition = TopPosition + 1;
            var linesToClean = 2;
            CleanLines(commandLineTopPosition, linesToClean);

            // установка курсора в начальную позицию комантной строки
            Console.SetCursorPosition(leftPosition, commandLineTopPosition);
            Console.Write("> ");
        }
    }
}
