using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class ChessPieceBishop : ChessPiece
    {
        public override bool CanMove(int row, int file)
        {
            throw new NotImplementedException();
        }

        public override List<ChessMove> GetValidMoves(ChessGameState state)
        {
            List<ChessMove> validMoves = new List<ChessMove>();

            BitArray allyPieces;
            if (color == ChessColor.Black)
            {
                allyPieces = state.BlackPieces;
            }
            else
            {
                allyPieces = state.WhitePieces;
            }

            int curIndex = row * 8 + file;



            return validMoves;
        }
    }
}
