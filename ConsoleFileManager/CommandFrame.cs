using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    class CommandFrame : Frame
    {

        public CommandFrame(int topPosition, int height) : base(topPosition, height)
        {
            
        }
        public void ShowTitle(string title)
        {
            Console.SetCursorPosition(leftPosition, TopPosition);
            Console.WriteLine(title);
        }
        public void CommandLineReady()
        {
            // Очистка строк
            var commandLineTopPosition = TopPosition + 1;
            var linesToClean = 2;
            var leftOffset = 3;
            CleanLines(commandLineTopPosition, linesToClean, leftOffset);

            // установка курсора в начальную позицию комантной строки
            Console.SetCursorPosition(leftPosition, commandLineTopPosition);
        }

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

        public void ShowInfoContent(string info)
        {
            var commandInfoTopPosition = TopPosition + 3;
            Console.SetCursorPosition(leftPosition, commandInfoTopPosition);

            Console.WriteLine(info);
        }

        public void CleanInfo()
        {
            var commandInfoTopPosition = TopPosition + 3;
            var linesToClean = TopPosition + Height - commandInfoTopPosition - 2; // TODO: 2
            CleanLines(commandInfoTopPosition, linesToClean, leftPosition);
        }
    }
}
