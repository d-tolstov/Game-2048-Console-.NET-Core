using System;
using System.Collections.Generic;

namespace Game2048
{
    public class GameState
    {
        /// <summary>
        /// Число строк
        /// </summary>
        private const int FieldsRowCount = 4;
        /// <summary>
        /// Число столбцов
        /// </summary>
        private const int FieldsColumnCount = 4;
        /// <summary>
        /// Массив игрового поля
        /// </summary>
        public int[,] FieldsArray = new int[FieldsRowCount, FieldsColumnCount];
        /// <summary>
        /// Строка, в которой расположено новое значение
        /// </summary>
        private int _newValueRowNo = -1;
        /// <summary>
        /// Столбец, в котором расположено новое значение
        /// </summary>
        private int _newValueColumnNo = -1;
        /// <summary>
        /// Признак наступления успешного состояния в игровом поле
        /// </summary>
        public bool IsSuccess;
        /// <summary>
        /// Счёт игры
        /// </summary>
        public int Score;
        /// <summary>
        /// Вставляем в произвольную незаполненную ячейку указанное значение
        /// </summary>
        /// <param name="value">Значение для вставки</param>
        /// <returns>Удалось ли найти незаполненную ячейку</returns>
        public bool InsertValue2RandomEmptyField(int value)
        {
            var emptyFieldsDict = new Dictionary<int,Tuple<int,int>>();

            for (var i = 0; i < FieldsArray.GetLength(0); i++)
            {
                for (var j = 0; j < FieldsArray.GetLength(1); j++)
                {
                    if (FieldsArray[i, j] != 0) 
                        continue;
                    
                    emptyFieldsDict[emptyFieldsDict.Count + 1] = new Tuple<int, int>(i,j);
                }
            }

            if (emptyFieldsDict.Count == 0)
                return false;

            var rnd = new Random();
            var (item1, item2) = emptyFieldsDict[rnd.Next(1,emptyFieldsDict.Count)];

            FieldsArray[item1, item2] = value;

            _newValueRowNo = item1;
            _newValueColumnNo = item2;

            return true;
        }
        /// <summary>
        /// Свайпим ячейки игрового поля влево
        /// </summary>
        public bool SwipeFields2Left()
        {
            var ret = false;
            
            for (var i = 0; i < FieldsArray.GetLength(0); i++)
            {
                if (SwipeRow2Left(i)) ret = true;
            }

            return ret;
        }
        /// <summary>
        /// Свайпим в указанной строке ячейки влево, применяя к ячейкам с одинаковым номиналом соответствующие действия
        /// </summary>
        /// <param name="rowNo"></param>
        public bool SwipeRow2Left(int rowNo)
        {
            /* Условия свайпа :
             *  все нули смещаются вправо
             *  соседние два числа одинакового номинала схлопываются в одно число
             *  в последовательности из трёх чисел схлопываются только левые два
             * Если в результате свайпа сдвинулись какие-либо ячейки, то возвращаем true
             */
            var ret = false;

            var newRowList = new List<int>();
            var prevValue = 0;

            for (var j = 0; j < FieldsArray.GetLength(1); j++)
            {
                var fieldValue = FieldsArray[rowNo, j];

                if (fieldValue == 0)
                {
                    continue;
                }

                if (prevValue == fieldValue)
                {
                    var newValue = fieldValue * 2;
                    // Увеличиваем счёт
                    Score += newValue;
                    if (newValue == 2048)
                        IsSuccess = true;

                    newRowList.Add(newValue);
                    prevValue = 0;
                }
                else
                {
                    if (prevValue != 0)
                    {
                        newRowList.Add(prevValue);
                    }

                    prevValue = fieldValue;
                }
            }
            if (prevValue != 0)
                newRowList.Add(prevValue);

            // Заполняем начало строки новыми значениями
            var columnNo = 0;
            foreach (var newValue in newRowList)
            {
                if (FieldsArray[rowNo, columnNo] != newValue)
                {
                    FieldsArray[rowNo, columnNo] = newValue;
                    ret = true;
                }

                columnNo++;
            }

            // Заполняем нолями остаток строки
            for (var k = columnNo; k < FieldsArray.GetLength(1); k++)
            {
                if (FieldsArray[rowNo, k] == 0) 
                    continue;
                
                FieldsArray[rowNo, k] = 0;
                ret = true;
            }

            return ret;
        }
        /// <summary>
        /// Выводим игровое поле на консоли
        /// </summary>
        public void Show()
        {
            ConsoleIo.ShowFields(FieldsArray, _newValueRowNo, _newValueColumnNo);
            Console.WriteLine($"Score : {Score}");
        }
        /// <summary>
        /// Поворот игрового поля вправо на 90 градусов
        /// </summary>
        public void RotateFieldsRight()
        {
            var ret = new int[FieldsRowCount, FieldsColumnCount];

            for (var i = 0; i < FieldsRowCount; ++i)
            {
                for (var j = 0; j < FieldsColumnCount; ++j)
                {
                    ret[i, j] = FieldsArray[FieldsColumnCount - j - 1, i];
                }
            }

            FieldsArray = ret;
        }
        /// <summary>
        /// Возвращает копию текущего класса
        /// </summary>
        /// <returns></returns>
        public GameState Copy()
        {
            return new GameState(){ _newValueColumnNo = _newValueColumnNo, _newValueRowNo = _newValueRowNo, FieldsArray = FieldsArray, Score = Score};
        }
    }
}
