﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    class InfoFrame : Frame
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="topPosition"></param>
        /// <param name="height"></param>
        public InfoFrame(int topPosition, int height) : base(topPosition, height)
        {

        }

        /// <summary>
        /// Отображение информации (аргумент - массив строк)
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfoContent(string[] info)
        {
            // Очистка окна от предыдущего сообщения
            CleanInfo();

            // Отображение нового сообщения 
            var infoTopPosition = TopPosition + 1; // TODO: getter/setter
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
            CleanInfo();

            // Отображение нового сообщения
            var infoTopPosition = TopPosition + 1; // TODO: getter/setter
            Console.SetCursorPosition(leftPosition, infoTopPosition);

            Console.WriteLine(info);
        }

        /// <summary>
        /// Очистка окна 
        /// </summary>
        public void CleanInfo()
        {
            var infoTopPosition = TopPosition + 1; // TODO: getter/setter
            var linesToClean = Height - 2; // 2  - две строки с границей (верхняя и нижняя)
            CleanLines(infoTopPosition, linesToClean);
        }
    }
}
