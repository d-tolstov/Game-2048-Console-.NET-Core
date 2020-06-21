using System;
using System.Collections.Generic;

namespace Game2048
{
    internal class GameController
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public GameFields Fields = new GameFields();
        /// <summary>
        /// Стек ходов (для возвращения назад)
        /// </summary>
        private Stack<GameFields> _moveStack = new Stack<GameFields>();
        /// <summary>
        /// Старт игры (основная функция класса)
        /// </summary>
        public void Start()
        {
            InitializeGame();

            while (true)
            {
                Fields.Show();
                Console.WriteLine($"{MoveNo()}th move");

                switch (UserWantsToNextMove())
                {
                    case UserKeyPressedEnum.Escape:
                        return;
                    case UserKeyPressedEnum.Restart:
                        InitializeGame();
                        Console.WriteLine("Restart.");
                        continue;
                    case UserKeyPressedEnum.Undo:
                        Console.WriteLine(Undo() ? "Undo previous move." : "Unable to cancel previous move.");
                        continue;
                    case UserKeyPressedEnum.Left:
                        MoveLeft();
                        break;
                    case UserKeyPressedEnum.Right:
                        MoveRight();
                        break;
                    case UserKeyPressedEnum.Up:
                        MoveUp();
                        break;
                    case UserKeyPressedEnum.Down:
                        MoveDown();
                        break;
                    default:
                        continue;
                }

                if ( !Fields.IsSuccess && FillNextRamdomField())
                {
                    continue;
                }

                if (Fields.IsSuccess)
                {
                    Console.WriteLine($"You win the game! Totally - {MoveNo() - 1} moves.");
                }
                else
                {
                    Console.WriteLine("There is no empty cell. You loose the game!");
                }

                var userAnswer = UserWantToRestart();
                if (userAnswer == UserKeyPressedEnum.Escape)
                    break;
                switch (userAnswer)
                {
                    case UserKeyPressedEnum.Restart:
                        InitializeGame();
                        Console.WriteLine("Restart.");
                        break;
                    case UserKeyPressedEnum.Undo:
                        Console.WriteLine(Undo() ? "Undo previous move." : "Unable to cancel previous move.");
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Интерпретация ответа пользователя
        /// </summary>
        public enum UserKeyPressedEnum
        {
            None, Escape, Restart, Undo, Left, Right, Up, Down
        }
        /// <summary>
        /// Запрашиваем у пользователя его мысли на тему следующего хода
        /// </summary>
        /// <returns></returns>
        protected UserKeyPressedEnum UserWantsToNextMove()
        {
            Console.WriteLine("Press key: Escape - exit, R - restart, U - undo, Arrows - swipe left/right/down/up");
            while (true)
            {
                var keyPressed = Console.ReadKey(false).Key;
                Console.WriteLine();
                switch (keyPressed)
                {
                    case ConsoleKey.Escape:
                        return UserKeyPressedEnum.Escape;
                    case ConsoleKey.R:
                        return UserKeyPressedEnum.Restart;
                    case ConsoleKey.U:
                        return UserKeyPressedEnum.Undo;
                    case ConsoleKey.LeftArrow:
                        return UserKeyPressedEnum.Left;
                    case ConsoleKey.RightArrow:
                        return UserKeyPressedEnum.Right;
                    case ConsoleKey.UpArrow:
                        return UserKeyPressedEnum.Up;
                    case ConsoleKey.DownArrow:
                        return UserKeyPressedEnum.Down;
                    default:
                        Console.WriteLine("Unsupported key pressed!");
                        continue;
                }
            }
        }
        /// <summary>
        /// Запрашиваем у пользователя разрешение на рестарт игры или отмену предыдущего хода
        /// </summary>
        /// <returns></returns>
        protected UserKeyPressedEnum UserWantToRestart()
        {
            Console.WriteLine("Press key: Escape - exit, R - restart, U - undo");
            while (true)
            {
                var keyPressed = Console.ReadKey(false).Key;
                Console.WriteLine();
                switch (keyPressed)
                {
                    case ConsoleKey.Escape:
                        return UserKeyPressedEnum.Escape;
                    case ConsoleKey.R:
                        return UserKeyPressedEnum.Restart;
                    case ConsoleKey.U:
                        return UserKeyPressedEnum.Undo;
                    default:
                        Console.WriteLine("Unsupported key pressed!");
                        continue;
                }
            }
        }
        /// <summary>
        /// Инициализация параметров игры
        /// </summary>
        protected void InitializeGame()
        {
            Fields = new GameFields();
            _moveStack = new Stack<GameFields>();
            FillNextRamdomField();
        }
        /// <summary>
        /// Отмена предыдущего хода
        /// </summary>
        /// <returns>Возможна ли отмена?</returns>
        protected bool Undo()
        {
            var ret = false;

            switch (_moveStack.Count)
            {
                case 0:
                case 1:
                    break;
                default:
                    _moveStack.Pop();
                    Fields = _moveStack.Peek();
                    ret = true;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// Ход по стрелке влево
        /// </summary>
        protected void MoveLeft()
        {
            SwipeFieldsToLeft();
        }
        /// <summary>
        /// Ход по стрелке вправо
        /// </summary>
        protected void MoveRight()
        {
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            SwipeFieldsToLeft();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
        }
        /// <summary>
        /// Ход по стрелке вверх
        /// </summary>
        protected void MoveUp()
        {
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            SwipeFieldsToLeft();
            Fields.RotateFieldsRight();
        }
        /// <summary>
        /// Ход по стрелке вниз
        /// </summary>
        protected void MoveDown()
        {
            Fields.RotateFieldsRight();
            SwipeFieldsToLeft();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
        }
        /// <summary>
        /// Номер следующего хода
        /// </summary>
        /// <returns></returns>
        protected int MoveNo()
        {
            return _moveStack.Count;
        }
        /// <summary>
        /// Свайпим игровое поле влево
        /// </summary>
        protected void SwipeFieldsToLeft()
        {
            Fields.SwipeFields2Left();
        }
        /// <summary>
        /// Заполняем произвольную пустую ячейку случайным значением и записываем текущий ход в стек
        /// </summary>
        /// <returns></returns>
        protected bool FillNextRamdomField()
        {
            if (!Fields.InsertValue2RandomEmptyField(RandomFieldValue()))
                return false;
            
            _moveStack.Push(Fields.Copy());
            
            return true;
        }
        /// <summary>
        /// Случайное значение (90% - 2, 10% - 4)
        /// </summary>
        /// <returns></returns>
        protected int RandomFieldValue()
        {
            var rnd = new Random();
            var rndNumber = rnd.Next(1, 10);
            // 90% - 2, 10% - 4
            return rndNumber == 10 ? 4 : 2;
        }
    }
}