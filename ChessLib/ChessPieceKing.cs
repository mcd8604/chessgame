using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class ChessPieceKing : ChessPiece
    {
        public override bool CanMove(int row, int file)
        {
            throw new NotImplementedException();
        }

        public override List<ChessMove> GetValidMoves(ChessGameState state)
        {
            List<ChessMove> validMoves = new List<ChessMove>();

            BitArray allyPieces;

            if(color == ChessColor.White)
            {
                allyPieces = state.WhitePieces;
            }
            else
            {
                allyPieces = state.BlackPieces;
            }

            int curIndex = row * 8 + file;

            // 8 possible moves 
            // TODO: check for out of bounds

            if (allyPieces[curIndex - 9])
            {
                ChessMove move = new ChessMove(row, file, row - 1, file - 1);
                move.Color = color;
                move.PieceType = ChessPieceType.King;
            }
            return validMoves;
        }

        public bool IsInCheck(ChessGameState state)
        {            
            bool inCheck = false;

            int kingIndex = row * 8 + file;

            // validate against pawns
            
            // validate against knights

            // validate against bishops (and queens)

            // validate against rooks (and queens)

            return inCheck;
        }
    }
}
