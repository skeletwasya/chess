using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    
    internal class King : Piece
    {
        
        //Проверка на то, может ли король рокироваться в короткую сторону
        private bool CanCastleShort(Square[,] squares)
        {
            //Если цвет короля чёрный, то ряд, в котором он может совершить рокировку восьмой, то есть, под индексом 7, если же цвет белый, то ряд первый, а, индекс 0.
            int y = PieceColor.Equals(PieceColor.White) ? 7 : 0;
            
            if (squares[5, y].Piece is Empty && squares[6, y].Piece is Empty && //Если между королём и ладьёй нет других фигур
                squares[7, y].Piece is Rook && !squares[7, y].Piece.Moved && !this.Moved && //Если ладья, находящаяся на своей начальной позиции, ещё не двигалась
                //Если клетки, через которые должен пройти король для рокировки, не под боем
                !GameLogic.IsSquareHitByEnemy(squares, new Point(Form1.SquareSize * 5, Form1.SquareSize * y), pieceColor == PieceColor.White ? false : true) &&
                !GameLogic.IsSquareHitByEnemy(squares, new Point(Form1.SquareSize * 6, Form1.SquareSize * y), pieceColor == PieceColor.White ? false : true) &&
                !GameLogic.IsSquareHitByEnemy(squares, new Point(Form1.SquareSize * 4, Form1.SquareSize * y), pieceColor == PieceColor.White ? false : true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Проверка на то, может ли король рокироваться в длинную сторону
        private bool CanCastleLong(Square[,] squares)
        {
            //Если цвет короля чёрный, то ряд, в котором он может совершить рокировку восьмой, то есть, под индексом 7, если же цвет белый, то ряд первый, а, индекс 0.
            int y = PieceColor.Equals(PieceColor.White) ? 7 : 0;
            if (squares[3, y].Piece is Empty && squares[2, y].Piece is Empty && squares[1, y].Piece is Empty && //Если между королём и ладьёй нет других фигур
                squares[0, y].Piece is Rook && !squares[0, y].Piece.Moved && !this.Moved && //Если ладья, находящаяся на своей начальной позиции, ещё не двигалась
                //Если клетки, через которые должен пройти король для рокировки, не под боем
                !GameLogic.IsSquareHitByEnemy(squares, new Point(Form1.SquareSize * 3, Form1.SquareSize * y), pieceColor == PieceColor.White ? false : true) &&
                !GameLogic.IsSquareHitByEnemy(squares, new Point(Form1.SquareSize * 2, Form1.SquareSize * y), pieceColor == PieceColor.White ? false : true) &&
                !GameLogic.IsSquareHitByEnemy(squares, new Point(Form1.SquareSize * 4, Form1.SquareSize * y), pieceColor == PieceColor.White ? false : true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Movable(Point from, Point to, Square[,] squares)
        {
            int to_x = to.X / Form1.SquareSize;
            int to_y = to.Y / Form1.SquareSize;
            int from_x = from.X / Form1.SquareSize;
            int from_y = from.Y / Form1.SquareSize;
            if ((from_x == to_x || from_y == to_y || Math.Abs(from_x - to_x) == Math.Abs(from_y - to_y)) && //Правило, по которому ходит король такое же, как у ферзя
                (Math.Abs(from_x - to_x) == 1 || Math.Abs(from_y - to_y) == 1) && //И если он ходит как обычно, то есть на 1 клетку
                IsCoordValid(squares, from, to) ||
                //Либо если движение происходит на 2 клетки, (то есть совершается рокировка) и если король может рокироваться в короткую либо длинную сторону
                (from_y == to_y && (to_x - from_x == 2 && CanCastleShort(squares) || to_x - from_x == -2 && CanCastleLong(squares) )))
            {
                if(CanCastleShort(squares) || CanCastleLong(squares))
                {
                    //Обозначим, что король сделал рокировку, для того, чтобы в дальнейшем корректно это отобразить на экране
                    squares[to_x, to_y].JustCastled = true;
                }
                moved = true;
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
                Image image = Image.FromFile("./images/white_king_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
            else if (PieceColor == PieceColor.Black)
            {
                Image image = Image.FromFile("./images/black_king_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
        }
        public King(Graphics pieceGraphics, PieceColor pieceColor) : base(pieceGraphics, pieceColor) { }
    }
}
