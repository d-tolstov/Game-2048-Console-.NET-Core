using System;

namespace Game2048
{
    public class ConsoleIo
    {
        private const int FieldWidth = 10; // Ширина поля в символах
        /// <summary>
        /// Перевод числа в строку для отображения в консоли
        /// </summary>
        /// <param name="valueInt">Число</param>
        /// <returns>Строка</returns>
        public static string FieldValue2String(int valueInt)
        {
            var valueStr = $"{valueInt}";
            var whiteSpaceCount = FieldWidth - valueStr.Length;
            if (whiteSpaceCount < 0) 
                throw new Exception($"Число {valueInt} нельзя отобразить в интерфейсе!");
            var whiteSpaceLeftCount = whiteSpaceCount/2;
            var whiteSpaceRightCount = whiteSpaceCount - whiteSpaceLeftCount;
            return $"{new string(' ',whiteSpaceLeftCount)}{valueStr}{new string(' ', whiteSpaceRightCount)}";
        }
        /// <summary>
        /// Строка для отображения пустого ряда чисел
        /// </summary>
        /// <returns></returns>
        public static string CellPlaceHolderString()
        {
            return $"{new string(' ', FieldWidth)}";
        }

        /// <summary>
        /// Вывод двумерного массива в консоль (1-е измерение - строки, 2-е измерение - колонки)
        /// </summary>
        /// <param name="fields">Массив</param>
        /// <param name="highlightedRowNo">Номер строки выделяемой ячейки</param>
        /// <param name="highlightedColumnNo">Номер колонки выделяемой ячейки</param>
        /// <param name="baseBackGroundColor">Базовый фоновый цвет</param>
        /// <param name="highlightedColor">Фоновый цвет выделяемой ячейки</param>
        public static void ShowFields(int[,] fields, ConsoleColor baseBackGroundColor, ConsoleColor highlightedColor, int highlightedRowNo, int highlightedColumnNo)
        {
            var columnCount = fields.GetLength(1);

            Console.BackgroundColor = baseBackGroundColor;
            Console.ForegroundColor = ConsoleColor.Black;

            for (var i = 0; i < fields.GetLength(0); i++)
            {
                var row = new int[columnCount];
                for (var j = 0; j < columnCount; j++)
                {
                    row[j] = fields[i, j];
                }

                ShowRow(row, baseBackGroundColor, highlightedColor, i == highlightedRowNo ? highlightedColumnNo : -1);
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Вывод строки массива в консоль
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="baseBackGroundColor"></param>
        /// <param name="highlightedColor"></param>
        /// <param name="highlightedColumnNo"></param>
        public static void ShowRow(int[] row, ConsoleColor baseBackGroundColor, ConsoleColor highlightedColor, int highlightedColumnNo)
        {
            var columnCount = row.Length;

            Console.BackgroundColor = baseBackGroundColor;
            Console.Write('|');
            for (var i = 0; i < columnCount; i++)
            {
                Console.BackgroundColor = i == highlightedColumnNo ? highlightedColor : baseBackGroundColor;
                Console.Write(CellPlaceHolderString());
                Console.BackgroundColor = baseBackGroundColor;
                Console.Write('|');
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            Console.BackgroundColor = baseBackGroundColor;
            Console.Write('|');
            for (var i = 0; i < columnCount; i++)
            {
                Console.BackgroundColor = i == highlightedColumnNo ? highlightedColor : baseBackGroundColor;
                Console.Write(FieldValue2String(row[i]));
                Console.BackgroundColor = baseBackGroundColor;
                Console.Write('|');
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            Console.BackgroundColor = baseBackGroundColor;
            Console.Write('|');
            for (var i = 0; i < columnCount; i++)
            {
                Console.BackgroundColor = i == highlightedColumnNo ? highlightedColor : baseBackGroundColor;
                Console.Write(CellPlaceHolderString());
                Console.BackgroundColor = baseBackGroundColor;
                Console.Write('|');
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }
    }
}
