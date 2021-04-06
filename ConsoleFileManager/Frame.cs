using System;
using System.Text;

namespace ConsoleFileManager
{
    class Frame
    {
        private const string lineX = "─";
        private const string lineY = "│";
        private const string corner = "+";
        private const string space = " ";
        
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
    }
}
