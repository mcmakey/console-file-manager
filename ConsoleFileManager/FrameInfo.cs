using System;

namespace ConsoleFileManager
{
    class FrameInfo : Frame
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="topPosition"></param>
        /// <param name="height"></param>
        public FrameInfo(int topPosition, int height) : base(topPosition, height)
        {

        }

        /// <summary>
        /// Отображение информации (аргумент - массив строк)
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfoContent(string[] info)
        {
            // Очистка окна от предыдущего сообщения
            Clean();

            // Отображение нового сообщения 
            var infoTopPosition = TopPosition + 1;
            Console.SetCursorPosition(leftPosition, infoTopPosition);

            for (int i = 0; i < info.Length; i++)
            {
                Console.SetCursorPosition(leftPosition, infoTopPosition + i);
                Console.WriteLine(info[i]);
            }
        }

        /// <summary>
        /// Отображение информации (аргумент - строка)
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfoContent(string info)
        {
            // Очистка окна от предыдущего сообщения
            Clean();

            // Отображение нового сообщения
            var infoTopPosition = TopPosition + 1;
            Console.SetCursorPosition(leftPosition, infoTopPosition);

            Console.WriteLine(info);
        }
    }
}
