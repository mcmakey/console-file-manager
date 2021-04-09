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
            var leftOffset = 3;
            CleanLines(commandLineTopPosition, linesToClean, leftOffset);

            // установка курсора в начальную позицию комантной строки
            Console.SetCursorPosition(leftPosition, commandLineTopPosition);
            Console.Write("> ");
        }

        /// <summary>
        /// Отображение сообщений в командном окне (аргумент - массив строк)
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfoContent(string[] info)
        {
            var commandInfoTopPosition = TopPosition + 3;
            Console.SetCursorPosition(leftPosition, commandInfoTopPosition);

            for (int i = 0; i < info.Length; i++)
            {
                Console.SetCursorPosition(leftPosition, commandInfoTopPosition + i);
                Console.WriteLine(info[i]);
            }
        }

        /// <summary>
        /// Отображение сообщений в командном окне (аргумент - строка)
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfoContent(string info)
        {
            var commandInfoTopPosition = TopPosition + 3;
            Console.SetCursorPosition(leftPosition, commandInfoTopPosition);

            Console.WriteLine(info);
        }

        /// <summary>
        /// Стерерть сообщения в комантном окне
        /// </summary>
        public void CleanInfo()
        {
            var commandInfoTopPosition = TopPosition + 3;
            var linesToClean = TopPosition + Height - commandInfoTopPosition - 2; // TODO: 2
            CleanLines(commandInfoTopPosition, linesToClean, leftPosition);
        }
    }
}
