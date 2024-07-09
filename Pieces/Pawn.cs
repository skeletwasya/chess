
namespace chess
{
    internal class Pawn : Piece
    {   
        //Проверка, может ли пешка двигаться прямо
        private bool CanMoveForward(Square[,] squares, int from_x, int from_y, int to_x, int to_y)
        {
            //d - направление движения пешки, -1 вверх, 1 вниз
            int d = pieceColor.Equals(PieceColor.White) ? -1 : 1;
            //Если перемещение идёт по вертикали и если клетка, куда направляется пешка, пустая
            if (from_x == to_x && squares[to_x, to_y].Piece is Empty)
            {
                return
                    //Если пешка пошла на 1 клетку, когда она уже сходила ранее в партии или
                    (to_y - from_y == d && moved ||
                    //Если пешка пошла на 2 либо на 1 клетку, когда она ещё не ходила и
                    (to_y - from_y == 2 * d || to_y - from_y == d) && !moved &&
                    //Если клетка, которая стоит между клеткой, на которую идёт пешка и клеткой, от которой идёт пешка, пустая
                    squares[from_x, from_y + d].Piece is Empty);
            }
            else return false;
        }
        //Проверка, может ли пешка сходить по диагонали
        private bool CanMoveDiagonally(Square[,] squares, int from_x, int from_y, int to_x, int to_y)
        {
            //d - направление движения пешки, -1 вверх, 1 вниз
            int d = pieceColor.Equals(PieceColor.White) ? -1 : 1;
            //Если перемещение происходит по диагонали на 1 клетку
            if (Math.Abs(to_x - from_x) == 1 && to_y - from_y == d)
            {
                return
                    //Если клетка, куда направляется пешка, занята вражеской фигурой или
                    squares[to_x, to_y].Piece.PieceColor != PieceColor && squares[to_x, to_y].Piece.PieceColor != PieceColor.Empty ||
                    //Если под клеткой, куда сходила пешка, находится клетка, в которой есть пешка, готовая, чтобы её взяли на проходе
                    squares[to_x, to_y - d].AnPassantable;
            }
            else return false;
        }
        private bool PawnMovable(Square[,] squares, Point from, Point to)
        {
            int to_x = to.X / Form1.SquareSize;
            int to_y = to.Y / Form1.SquareSize;
            int from_x = from.X / Form1.SquareSize;
            int from_y = from.Y / Form1.SquareSize;
            //Если координаты, куда нужно попасть пешке, не выходят за пределы доски
            if(to_y > -1 && to_y < 8)
            {
                return
                CanMoveForward(squares, from_x, from_y, to_x, to_y) ||
                CanMoveDiagonally(squares, from_x, from_y, to_x, to_y);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }            
        }
        public override bool Movable(Point from, Point to, Square[,] squares)
        {
            //Преобразование координат в индексы
            int to_x = to.X / Form1.SquareSize;
            int to_y = to.Y / Form1.SquareSize;
            int from_x = from.X / Form1.SquareSize;
            int from_y = from.Y / Form1.SquareSize;
            //Вспомогательные булевы переменные
            bool fl1 = false, fl2 = false;
            if (PawnMovable(squares, from, to) && //Правило, по которому ходит пешка
                IsCoordValid(squares, from, to))
            {
                //Если произошло взятие
                if (pieceColor == PieceColor.White &&
                Math.Abs(to_x - from_x) == 1 && to_y - from_y == -1 ||
                pieceColor == PieceColor.Black &&
                Math.Abs(to_x - from_x) == 1 && to_y - from_y == 1 )
                {
                    if(from_x - 1 != -1)
                    {
                        if (squares[from_x - 1, from_y].AnPassantable)
                        {
                            fl1 = true;
                        }
                    }
                    if(from_x + 1 != 8)
                    {
                        if(squares[from_x + 1, from_y].AnPassantable)
                        {
                            fl2 = true;
                        }
                    }
                    if(fl1 || fl2)
                    {
                        //Установить состояние клетки, что только что на ней произошло взятие на проходе
                        squares[to_x, to_y].JustAnPassanted = true;
                    }                   
                }

                return true;
            }
            else
            {
                return false;
            }
                
        }
        public override void Draw(Point coords)
        {
            if (PieceColor == PieceColor.White)
            {
                Image image = Image.FromFile("./images/white_pawn_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
            else if (PieceColor == PieceColor.Black)
            {
                Image image = Image.FromFile("./images/black_pawn_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
        }

        public Pawn(Graphics pieceGraphics, PieceColor pieceColor) : base(pieceGraphics, pieceColor) {  }
    }
}
