using System;
using System.Collections.Generic;

namespace Game2048
{
    internal class GameController
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public GameState Fields = new GameState();
        /// <summary>
        /// Стек ходов (для возвращения назад)
        /// </summary>
        private Stack<GameState> _moveStack = new Stack<GameState>();
        /// <summary>
        /// Множество неуспешных движений. Если число неуспешных движений достигнет 4, то это конец игры.
        /// </summary>
        private SortedSet<string> _unSuccessfulMoveSet = new SortedSet<string>();
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
                        if (!CheckMove(MoveLeft))
                            continue;
                        break;
                    case UserKeyPressedEnum.Right:
                        if (!CheckMove(MoveRight))
                            continue;
                        break;
                    case UserKeyPressedEnum.Up:
                        if (!CheckMove(MoveUp))
                            continue;
                        break;
                    case UserKeyPressedEnum.Down:
                        if (!CheckMove(MoveDown))
                            continue;
                        break;
                    default:
                        continue;
                }

                if ( !Fields.IsSuccess 
                  && FillNextRamdomField())
                {
                    continue;
                }

                Console.WriteLine(Fields.IsSuccess
                    ? $"You win the game! Totally - {MoveNo() - 1} moves."
                    : "There is no empty cell and no available move. You lost the game!");

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
                }
            }
        }
        /// <summary>
        /// Интерпретация ответа пользователя
        /// </summary>
        public enum UserKeyPressedEnum
        {
            Escape, Restart, Undo, Left, Right, Up, Down
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
            Fields = new GameState();
            _moveStack = new Stack<GameState>();
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
        /// Разрешить ли пользователю продолжить попытки нажатия на стрелки
        /// </summary>
        /// <param name="moveFunction"></param>
        /// <returns></returns>
        protected bool CheckMove(Func<bool> moveFunction)
        {
            var ret = moveFunction();
            if (ret)
            {
                _unSuccessfulMoveSet = new SortedSet<string>();
            }
            else
            {
                _unSuccessfulMoveSet.Add(moveFunction.Method.Name);
                if (_unSuccessfulMoveSet.Count >= 4)
                    ret = true;
            }

            return ret;
        }
        /// <summary>
        /// Ход по стрелке влево
        /// </summary>
        protected bool MoveLeft()
        {
            return SwipeFieldsToLeft();
        }
        /// <summary>
        /// Ход по стрелке вправо
        /// </summary>
        protected bool MoveRight()
        {
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            var ret = SwipeFieldsToLeft();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            return ret;
        }
        /// <summary>
        /// Ход по стрелке вверх
        /// </summary>
        protected bool MoveUp()
        {
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            var ret = SwipeFieldsToLeft();
            Fields.RotateFieldsRight();
            return ret;
        }
        /// <summary>
        /// Ход по стрелке вниз
        /// </summary>
        protected bool MoveDown()
        {
            Fields.RotateFieldsRight();
            var ret = SwipeFieldsToLeft();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            Fields.RotateFieldsRight();
            return ret;
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
        protected bool SwipeFieldsToLeft()
        {
            return Fields.SwipeFields2Left();
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
            var rndNumber = rnd.Next(1, 11);
            // 90% - 2, 10% - 4
            return rndNumber == 10 ? 4 : 2;
        }
    }
}