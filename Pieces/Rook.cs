
namespace chess
{
    internal class Rook : Piece
    {
        public override bool Movable(Point from, Point to, Square[,] squares)
        {
            int to_x = to.X / Form1.SquareSize;
            int to_y = to.Y / Form1.SquareSize;
            int from_x = from.X / Form1.SquareSize;
            int from_y = from.Y / Form1.SquareSize;
            //Если координата расположена в соответствии с тем, как ходит ладья, и, если по этой координате нет союзной фигуры, то проверить, есть ли другие фигуры на пути
            //Правило, по которому ходит ладья
            if ((from_x == to_x || from_y == to_y) &&
                IsCoordValid(squares, from, to))
            {
                //Проверка на то, есть ли фигуры на пути ладьи, если есть, вернуть false, иначе true
                //Если происходит движение по вертикали вниз
                if (from_x < to_x)
                {
                    while (from_x != to_x)
                    {                        
                        to_x -= 1;
                        //Если в предшествующей клетке есть фигура, то вернуть false (аналогично для других случаев)
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }
                //Если происходит движение по вертикали вверх
                if (from_x > to_x)
                {
                    while (from_x != to_x)
                    {
                        to_x += 1;
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }
                //Если движение происходит по горизонтали вправо
                if (from_y < to_y)
                {
                    while (from_y != to_y)
                    {
                        to_y -= 1;
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }
                //Если движение происходит по горизонтали влево
                if (from_y > to_y)
                {
                    while (from_y != to_y)
                    {
                        to_y += 1;
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }           
        }
        //Рисование ладьи на определённых координатах
        public override void Draw(Point coords)
        {
            if (PieceColor == PieceColor.White)
            {
                Image image = Image.FromFile("./images/white_rook_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
            else if (PieceColor == PieceColor.Black)
            {
                Image image = Image.FromFile("./images/black_rook_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
        }
        public Rook(Graphics pieceGraphics, PieceColor pieceColor) : base(pieceGraphics, pieceColor) { }
    }
}
