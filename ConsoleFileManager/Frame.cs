using System;
using System.Text;

namespace ConsoleFileManager
{
    class Frame
    {
        private const string lineX = "─";
        private const string lineY = "│";
        private const string corner = "+";
        private const char space = ' ';
        private const int leftPosition = 2;

        private int TopPosition { get; }
        private int Height { get; }

        public Frame(int topPosition, int height)
        {
            this.TopPosition = topPosition;
            this.Height = height;
        }

        public void Dispaly()
        {
            StringBuilder firstLastLine = new StringBuilder();
            StringBuilder line = new StringBuilder();

            var lines = this.Height;
            var columns = Console.LargestWindowWidth;

            for (int i = 0; i < columns; i++)
            {
                if (i == 0 || i == columns - 1)
                {
                    firstLastLine.Append(corner);
                    line.Append(lineY);
                }
                else
                {
                    firstLastLine.Append(lineX);
                    line.Append(space);
                }
            }

            Console.SetCursorPosition(0, TopPosition);

            for (int i = 0; i < lines; i++)
            {
                if (i == 0 || i == lines - 1)
                {
                    Console.WriteLine(firstLastLine.ToString());
                }
                else
                {
                    Console.WriteLine(line.ToString());
                }
            }
        }

        // методы окна командной строки

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

        //

        /// <summary>
        /// Очитска строк
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="amountLines"></param>
        /// <param name="leftOffset"></param>
        private void CleanLines(int startLine, int numberLinesToClear, int leftOffset = 0)
        {
            for (int i = 0; i < numberLinesToClear; i++)
            {
                var currentLine = startLine + i;
                Console.SetCursorPosition(leftPosition, currentLine);
                Console.Write(new String(space, Console.BufferWidth - leftPosition * 2));
            }
        }

    }
}
