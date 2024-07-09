

namespace chess
{
    internal class Knight : Piece
    {
        public override bool Movable(Point from, Point to, Square[,] squares)
        {
            int to_x = to.X / Form1.SquareSize;
            int to_y = to.Y / Form1.SquareSize;
            int from_x = from.X / Form1.SquareSize;
            int from_y = from.Y / Form1.SquareSize;
            if ((Math.Abs(from_x - to_x) == 1 && Math.Abs(from_y - to_y) == 2 || //Правило, по которому ходит конь
                Math.Abs(from_x - to_x) == 2 && Math.Abs(from_y - to_y) == 1) &&
                IsCoordValid(squares, from, to))
            {
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
                Image image = Image.FromFile("./images/white_knight_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
            else if (PieceColor == PieceColor.Black)
            {
                Image image = Image.FromFile("./images/black_knight_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
        }
        public Knight(Graphics pieceGraphics, PieceColor pieceColor) : base(pieceGraphics, pieceColor) { }
    }
}
