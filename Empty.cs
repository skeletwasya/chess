﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    internal class Empty : Piece
    {
        //С пустой фигурой нельзя взаимодействовать
        public override bool Movable(Point from, Point to, Square[,] squares)
        {
            return false;
        }
        //Пустая фигура не отображается на экране
        public override void Draw(Point coords)
        {

        }
        public Empty() : base() { pieceColor = PieceColor.Empty; }
    }
}
