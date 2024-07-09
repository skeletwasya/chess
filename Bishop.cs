using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    internal class Bishop : Piece
    {
        public override bool Movable(Point from, Point to, Square[,] squares)
        {
            int to_x = to.X / Form1.SquareSize;
            int to_y = to.Y / Form1.SquareSize;
            int from_x = from.X / Form1.SquareSize;
            int from_y = from.Y / Form1.SquareSize;
            if ((Math.Abs(from_x - to_x) == Math.Abs(from_y - to_y)) && //Правило, по которому ходит слон
                IsCoordValid(squares, from, to))
            {
                //Проверка на то, есть ли фигуры на пути слона, если есть, вернуть false, иначе true
                //Если происходит движение по диагонали вверх влево
                if (from_x > to_x && from_y > to_y)
                {
                    //Если в предшествующей клетке есть фигура, то вернуть false (аналогично для других случаев)
                    while (from_x != to_x && from_y != to_y)
                    {
                        to_x += 1;
                        to_y += 1;
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }
                //Если происходит движение по диагонали вверх вправо
                if (from_x > to_x && from_y < to_y)
                {
                    while (from_x != to_x && from_y != to_y)
                    {
                        to_x += 1;
                        to_y -= 1;
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }
                //Если происходит движение по диагонали вниз вправо
                if (from_x < to_x && from_y < to_y)
                {
                    while (from_x != to_x && from_y != to_y)
                    {
                        to_x -= 1;
                        to_y -= 1;
                        if (!(squares[to_x, to_y].Piece is Empty))
                        {
                            return false;
                        }
                    }
                }
                //Если происходит движение по диагонали вниз влево
                if (from_x < to_x && from_y > to_y)
                {
                    while (from_x != to_x && from_y != to_y)
                    {
                        to_x -= 1;
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
        public override void Draw(Point coords)
        {
            if(PieceColor == PieceColor.White)
            {
                Image image = Image.FromFile("./images/white_bishop_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
            else if (PieceColor == PieceColor.Black)
            {
                Image image = Image.FromFile("./images/black_bishop_no_font.png");
                PieceGraphics.DrawImage(image, coords.X, coords.Y);
            }
        }
        public Bishop(Graphics pieceGraphics, PieceColor pieceColor) : base(pieceGraphics, pieceColor) { }
    }
}
