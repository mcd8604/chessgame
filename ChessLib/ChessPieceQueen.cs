﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public class ChessPieceQueen : ChessPiece
    {
        public override bool CanMove(int row, int file)
        {
            throw new NotImplementedException();
        }

        public override List<ChessMove> GetValidMoves(ChessGameState state)
        {
            //throw new NotImplementedException();
            return new List<ChessMove>();
        }
    }
}
