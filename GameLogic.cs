

namespace chess
{
    internal static class GameLogic
    {
        private static Graphics graphics; //Переменная, необходимая для отрисовки графики
        private static int squareSize; //Размер клетки
        public static Graphics g
        { 
            get { return graphics; }
            set { graphics = value; }
        }
        public static int SquareSize
        {
            get { return squareSize; }
            set { squareSize = value; }
        }
        //Сравнить две позиции
        public static bool CompareTwoPositions(Square[,] pos1, Square[,] pos2)
        {
            //Размер двумерных массивов не может быть не равен 64
            if (pos1.Length != pos2.Length || pos1.Length != 64 || pos2.Length != 64) throw new Exception();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Если хотя бы одна клетка массива не равна соответствующей клетке другого массива, то вернуть false
                    if (pos1[i, j] != pos2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //Копирование массива клеток
        public static Square[,] CopyThePosition(Square[,] squares)
        {
            //Выделение памяти под новый массив
            Square[,] tmp = new Square[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tmp[i, j] = new Square(new Point(i * squareSize, j * squareSize), new Empty());
                }
            }
            //Установка фигур в новый массив, в соответствии с типом фигур, находящихся на соответствующих клетках в старом массиве
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (squares[i, j].Piece is Pawn)
                    {
                        tmp[i, j].Piece = new Pawn(g, squares[i, j].Piece.PieceColor);
                    }
                    if (squares[i, j].Piece is Rook)
                    {
                        tmp[i, j].Piece = new Rook(g, squares[i, j].Piece.PieceColor);
                    }
                    if (squares[i, j].Piece is Knight)
                    {
                        tmp[i, j].Piece = new Knight(g, squares[i, j].Piece.PieceColor);
                    }
                    if (squares[i, j].Piece is Bishop)
                    {
                        tmp[i, j].Piece = new Bishop(g, squares[i, j].Piece.PieceColor);
                    }
                    if (squares[i, j].Piece is King)
                    {
                        tmp[i, j].Piece = new King(g, squares[i, j].Piece.PieceColor);
                    }
                    if (squares[i, j].Piece is Queen)
                    {
                        tmp[i, j].Piece = new Queen(g, squares[i, j].Piece.PieceColor);
                    }
                    tmp[i, j].Piece.Moved = squares[i,j].Piece.Moved;
                    tmp[i, j].AnPassantable = squares[i, j].AnPassantable;
                    tmp[i, j].JustCastled = squares[i, j].JustCastled;
                    tmp[i, j].JustAnPassanted = squares[i, j].JustAnPassanted;
                }
                
            }
            return tmp;
        }
        //Поиск клетки, которая соответствует координатам
        public static Square SquareThatFitsCoords(int x, int y, Square[,] squares)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Если координаты не выходят за пределы клетки, вернуть эту клетку
                    if (x >= squares[i, j].AbsoluteCoords.X &&
                        x < squares[i, j].AbsoluteCoords.X + squareSize &&
                        y >= squares[i, j].AbsoluteCoords.Y &&
                        y < squares[i, j].AbsoluteCoords.Y + squareSize)
                    {
                        return squares[i, j];
                    }
                }
            }
            return new Square();
        }
        //Проверка, действителен ли ход
        private static bool IsMoveValid(Square from, Square to, bool whiteToMove, Square[,] squares)
        {
            Point oldCoords = from.AbsoluteCoords; //Координаты, откуда происходит движение
            Piece tmp = from.Piece; //Фигура, находящаяся в клетке, откуда предполагается движение
            //Ниже идёт преобразование абсолютных координат в индексы двумерного массива
            int x = to.AbsoluteCoords.X / squareSize; 
            int y = to.AbsoluteCoords.Y / squareSize;
            int old_x = oldCoords.X / squareSize;
            int old_y = oldCoords.Y / squareSize;
            //Создание копии позиции, в которой будет сделан тот или иной ход и
            //после этого будет произведена проверка, находится ли 
            //король игрока, сделавшего ход, под атакой
            Square[,] sqrs = CopyThePosition(squares);
            //Временная очистка клетки, необходимая для корректной работы Movable
            sqrs[old_x, old_y].Piece = new Empty();
            //Если на эту клетку возможно совершить ход
            if (tmp.Movable(oldCoords, to.AbsoluteCoords, sqrs))
            {
                //Если перемещаемая фиугра это пешка
                if (tmp is Pawn)
                {
                    //Если пешка совершила взятие на проходе, 
                    if (sqrs[x, y].JustAnPassanted)
                    {
                        //d - направление расположения пешки, снятой при взятии на проходе.
                        int d = sqrs[x, y].Piece.PieceColor == PieceColor.White ? 1 : -1;
                        //Если фигура, отстоящая на 1 клетку ниже или выше пешки
                        if (sqrs[x, y + d].Piece is Pawn &&
                            //является фигурой не того же цвета, что пешка и
                            sqrs[x, y + d].Piece.PieceColor != sqrs[x, y].Piece.PieceColor &&
                            //не является пустой фигурой
                            sqrs[x, y + d].Piece is not Empty)
                        {
                            //Снять эту фигуру с доски
                            sqrs[x, y + d].Piece = new Empty();
                        }
                    }
                }
                //Возврат ранее удалённой фигуры на исходное место
                sqrs[x, y].Piece = tmp;
                //Если король игрока, делающего ход, не под шахом
                if (!IsSquareHitByEnemy(sqrs, FindKing(sqrs, whiteToMove ? PieceColor.White : PieceColor.Black), !whiteToMove))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //Проверка, может ли игрок сделать ход
        public static bool CanPlayerMove(PieceColor playerColor, Square[,] squares, bool whiteToMove)
        {
            //Клетка, откуда делается ход
            Square from;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Если цвет фигуры на текущей клетке такой же, как playerColor,
                    //линейно пройтись по остальной доске и проверить, можно ли пойти на ту или иную клетку
                    if (squares[i, j].Piece.PieceColor == playerColor)
                    {
                        from = squares[i, j];
                        for (int m = 0; m < 8; m++)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                //Если можно пойти на текущую клетку, вернуть true
                                if (IsMoveValid(from, squares[m, k], whiteToMove, squares))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            //Если ни одна фигура игрока не может пойти ни на одну клетку, вернуть false
            return false;
        }
        //Статический метод, который передаёт координаты короля того или иного цвета на заданной позиции 
        public static Point FindKing(Square[,] squares, PieceColor color)
        {
            //Линейный поиск по двумерному массиву клеток
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Если на текущей клетке стоит король цвета color
                    if (squares[i, j].Piece is King && squares[i, j].Piece.PieceColor == color)
                    {
                        //Вернуть координаты этой клетки
                        return squares[i, j].AbsoluteCoords;
                    }
                }
            }
            //В каждой позиции должна быть пара королей
            throw new Exception("There is no king!");
        }
        //Статический метод, который проверяет, находится ли клетка по тем или иным координатам, под атакой противника, в заданной позиции
        public static bool IsSquareHitByEnemy(Square[,] squares, Point coords, bool isWhiteEnemy)
        {

            Piece checking_piece;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Удаление фигуры с текущей координаты необходимо для корректной работы метода Movable(Point from, Point to, Square[,] squares)
                    //Перед удалением сохраним фигуру в качестве временной переменной
                    checking_piece = squares[i, j].Piece;
                    squares[i, j].Piece = new Empty();
                    //Если фигура, которую мы проверяем, может пойти на coords
                    if (checking_piece.Movable(squares[i, j].AbsoluteCoords, coords, squares) &&
                        //И если фигура того же цвета, что фигуры противника
                        (checking_piece.PieceColor == PieceColor.White && isWhiteEnemy || checking_piece.PieceColor == PieceColor.Black && !isWhiteEnemy))
                    {
                        //То устанавливаем временно удалённую фигуру на место
                        squares[i, j].Piece = checking_piece;
                        //И возвращаем true
                        return true;
                    }
                    //После проверки вернём временно удалённую фигуру на место
                    squares[i, j].Piece = checking_piece;
                }
            }
            return false;
        }
        public static Square[,] SetAnPassantableAndReturn(Square[,] n_squares, int old_x, int old_y, int x, int y)
        {
            //Если игрок сходил пешкой на 2 клетки, то соперник может на следующем ходу провести взятие на проходе
            if (n_squares[x, y].Piece is Pawn && (
                //Если произошло движение белой пешки
                n_squares[x, y].Piece.PieceColor == PieceColor.White &&
                //на 2 клетки либо
                y == 4 && old_y == 6 ||
                //Если произошло движение чёрной пешки
                n_squares[x, y].Piece.PieceColor == PieceColor.Black &&
                //на 2 клетки
                y == 3 && old_y == 1))
            {
                n_squares[x, y].AnPassantable = true;
                //Если игроки сходили пешками на 2 клетки по очереди, anPassantable должен быть только у одной
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (n_squares[i, j].AnPassantable && i != x && j != y)
                        {
                            n_squares[i, j].AnPassantable = false;
                        }
                    }
                }
            }
            else
            {
                //Если один из игроков не пошёл пешкой на 2 клетки, то право взятия на проходе пропадает
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (n_squares[i, j].AnPassantable)
                        {
                            n_squares[i, j].AnPassantable = false;
                        }
                    }
                }
            }
            return n_squares;
        }
        //Если произошло взятие на проходе
        public static Square[,] SetJustAnPassantedAndReturn(Square[,] n_squares, int x, int y)
        {   
            //Если в клетке по заданным координатам произошло взятие на проходе
            if (n_squares[x, y].JustAnPassanted)
            {
                //d - направление расположения пешки, снятой при взятии на проходе.
                int d = n_squares[x, y].Piece.PieceColor == PieceColor.White ? 1 : -1;
                //Если фигура, отстоящая на 1 клетку ниже или выше пешки
                if (n_squares[x, y + d].Piece is Pawn &&
                    //является фигурой не того же цвета, что пешка и
                    n_squares[x, y + d].Piece.PieceColor != n_squares[x, y].Piece.PieceColor &&
                    //не является пустой фигурой
                    n_squares[x, y + d].Piece is not Empty)
                {
                    //Снять эту фигуру с доски
                    n_squares[x, y + d].Piece = new Empty();
                }
                n_squares[x, y].JustAnPassanted = false;
            }
            return n_squares;
        }
        //Если произошла рокировка
        public static Square[,] SetJustCastledAndReturn(Square[,] n_squares, int x, int y, Piece grabbedPiece)
        {
            if (n_squares[x, y].JustCastled && grabbedPiece is King)
            {
                //Если произошла рокировка в короткую сторону
                if (x == 6)
                {
                    //Перемещение ладьи в соответствии с правилами
                    n_squares[7, y].Piece = new Empty();
                    n_squares[5, y].Piece = new Rook(g, grabbedPiece.PieceColor);
                }
                //Если произошла рокировка в длинную сторону
                else if (x == 2)
                {
                    //Перемещение ладьи в соответствии с правилами
                    n_squares[0, y].Piece = new Empty();
                    n_squares[3, y].Piece = new Rook(g, grabbedPiece.PieceColor);
                }
                //На этой клетке только что произошла рокировка
                n_squares[x, y].JustCastled = false;
            }
            return n_squares;
        }
    }
}
