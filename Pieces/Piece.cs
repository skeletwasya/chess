

namespace chess
{
    abstract class Piece
    {
        protected bool moved = false; //Двигалась ли фигура
        protected PieceColor pieceColor; //Цвет фигуры
        protected Graphics pieceGraphics; //Поле, необходимое для отрисовки графики
        //Свойства ниже:
        public bool Moved
        {
            get { return moved; }
            set { moved = value; }
        }

        public Graphics PieceGraphics
        {
            set { pieceGraphics = value; }
            get { return pieceGraphics; }
        }

        public PieceColor PieceColor
        {
            get { return pieceColor; }
            set { pieceColor = value; }
        }
        //Проверка, можно ли пойти фигурой на определённую координату
        public abstract bool Movable(Point from, Point to, Square[,] squares);
        //Рисование фигуры по определённым координатам
        public abstract void Draw(Point coords);
        
        //Первичная проверка валидности хода
        protected bool IsCoordValid(Square[,] squares, Point from, Point to)
        {
            return squares[to.X / Form1.SquareSize, to.Y / Form1.SquareSize].Piece.PieceColor != this.PieceColor && //Если на текущей координате нет фигуры того же цвета
                   !(from.X == to.X && from.Y == to.Y); //Если новая координата не совпадает со старой                     
        }
        
        public Piece(Graphics pieceGraphics, PieceColor pieceColor)
        {
            this.pieceGraphics = pieceGraphics;
            this.pieceColor = pieceColor;
        }

        public Piece()
        {

        }
    }
}
