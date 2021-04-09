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
        public const int leftPosition = 2;

        public int TopPosition { get; }
        public int Height { get; }

        public Frame(int topPosition, int height)
        {
            this.TopPosition = topPosition;
            this.Height = height;
        }

        /// <summary>
        /// Отрисовка окна
        /// </summary>
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

        /// <summary>
        /// Очитска строк
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="amountLines"></param>
        public void CleanLines(int startLine, int numberLinesToClear)
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
