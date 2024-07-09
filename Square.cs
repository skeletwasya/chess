using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    internal class Square
    {
        private bool anPassantable = false; //Возможно ли взятие на проходе на этой клетке
        private bool justAnPassanted = false; //Произошло ли только что взятие на проходе на этой клетке
        private bool justCastled = false; //Произошла ли рокировка на этой клетке
        private Piece piece; //Фигура, находящаяся в клетке
        private Point absoluteCoords; //Координаты клетки
        private string name = ""; //Имя клетки

        //Конструктор
        public Square(Point absoluteCoords, Piece piece)
        {
            int size = Form1.SquareSize;
            this.absoluteCoords = absoluteCoords;
            //Если происходит попытка создать клетку на первом или на восьмом ряду, в которой находится пешка(что невозможно)
            if(piece is Pawn && absoluteCoords.X == 0 || absoluteCoords.Y == size * 7)
            {
                this.piece = new Empty();
            }
            else
            {
                this.piece = piece;
            }           
            
            //Установка имени клетки, в соответствии с координатами
            switch(absoluteCoords.X/size)
            {
                //absoluteCoords.X/size - это формула получения номера вертикалей доски.
                //Math.Abs(absoluteCoords.Y/size - 8) - это формула получения номера горизонталей доски.
                //Если absoluteCoords.X/size = 0, то речь идёт о вертикали "a", если 1, то о вертикали "b" и т.д.
                case 0:
                    {
                        Name = "a" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 1:
                    {
                        Name = "b" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 2:
                    {
                        Name = "c" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 3:
                    {
                        Name = "d" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 4:
                    {
                        Name = "e" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 5:
                    {
                        Name = "f" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 6:
                    {
                        Name = "g" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
                case 7:
                    {
                        Name = "h" + Math.Abs(absoluteCoords.Y / size - 8);
                    }
                    break;
            }
            
        }
        //Конструктор по умолчанию
        public Square()
        {

        }

        //Ниже находятся свойства:
        public bool JustCastled
        {
            get { return justCastled; }
            set { justCastled = value; }
        }

        public bool JustAnPassanted
        {
            get { return justAnPassanted; }
            set { justAnPassanted = value; }
        }
        public Piece Piece
        {
            get { return piece; }
            set { piece = value; }
        }
        public Point AbsoluteCoords
        {
            get { return absoluteCoords; }
            set { absoluteCoords = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public bool AnPassantable
        {
            get { return anPassantable; }
            set { anPassantable = value; }
        }
        //Перегрузка оператора равно
        public static bool operator ==(Square s1, Square s2)
        {
            return s1.Piece.GetType().Name == s2.Piece.GetType().Name;  
        }
        //Перегрузка оператора не равно
        public static bool operator !=(Square s1, Square s2)
        {
            return s1.Piece.GetType().Name != s2.Piece.GetType().Name;
        }
        public override bool Equals(object? obj)
        {
            throw new NotImplementedException();
        }
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
