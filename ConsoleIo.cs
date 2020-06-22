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
        /// <param name="repeatedChar">Символ для заполнения пустоты</param>
        /// <returns>Строка</returns>
        public static string FieldValue2String(int valueInt, char repeatedChar = ' ')
        {
            var valueStr = $"{valueInt}";
            var whiteSpaceCount = FieldWidth - valueStr.Length;
            if (whiteSpaceCount < 0) 
                throw new Exception($"Число {valueInt} нельзя отобразить в интерфейсе!");
            var whiteSpaceLeftCount = whiteSpaceCount/2;
            var whiteSpaceRightCount = whiteSpaceCount - whiteSpaceLeftCount;
            return $"{new string(repeatedChar,whiteSpaceLeftCount)}{valueStr}{new string(repeatedChar, whiteSpaceRightCount)}|";
        }
        /// <summary>
        /// Строка для отображения пустого ряда чисел
        /// </summary>
        /// <param name="columnCount">Число колонок</param>
        /// <param name="repeatedChar">Символ для заполнения пустоты</param>
        /// <returns></returns>
        public static string RowPlaceHolderString(int columnCount, char repeatedChar = ' ')
        {
            var ret = "|";
            for (var i = 1; i <= columnCount; i++)
            {
                ret += $"{new string(repeatedChar, FieldWidth)}|";
            }
            return ret;
        }
        /// <summary>
        /// Вывод двумерного массива в консоль (1-е измерение - строки, 2-е измерение - колонки)
        /// </summary>
        /// <param name="fields">Массив</param>
        /// <param name="underScoredRowNo">Номер строки выделяемой ячейки</param>
        /// <param name="underScoredColumnNo">Номер колонки выделяемой ячейки</param>
        public static void ShowFields(int[,] fields, int underScoredRowNo, int underScoredColumnNo)
        {
            var columnCount = fields.GetLength(1);

            for (var i = 0; i < fields.GetLength(0); i++)
            {
                Console.WriteLine(RowPlaceHolderString(columnCount));

                var rowString = "|";
                for (var j = 0; j < columnCount; j++)
                {
                    rowString += FieldValue2String(fields[i,j], i==underScoredRowNo && j==underScoredColumnNo ? '_' : ' ');
                }
                Console.WriteLine(rowString);

                Console.WriteLine(RowPlaceHolderString(columnCount));
            }
        }
    }
}
