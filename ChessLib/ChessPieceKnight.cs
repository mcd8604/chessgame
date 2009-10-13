using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class ChessPieceKnight : ChessPiece
    {
        public ChessPieceKnight()
        {
            type = ChessPieceType.Knight;
        }

        public override bool CanMove(int row, int file)
        {
            throw new NotImplementedException();
        }

        public override List<ChessMove> GetValidMoves(ChessGameState state)
        {
            List<ChessMove> validMoves = new List<ChessMove>();

            // 8 potential moves for a knight
            BitArray allyBoard;
            if (color == ChessColor.Black)
                allyBoard = state.BlackPieces;
            else
                allyBoard = state.WhitePieces;

            int curIndex = row * 8 + file;

            // file + 2, row + 1
            if (file < 6 && row < 7
                && !allyBoard[curIndex + 10])
            {
                ChessMove move = new ChessMove(row, file, row + 1, file + 2);
                validMoves.Add(move);
            }

            // file + 1, row + 2
            if (file < 7 && row < 6
                && !allyBoard[curIndex + 17])
            {
                ChessMove move = new ChessMove(row, file, row + 2, file + 1);
                validMoves.Add(move);
            }

            // file - 1, row + 2
            if (file > 0 && row < 6
                && !allyBoard[curIndex + 15])
            {
                ChessMove move = new ChessMove(row, file, row + 2, file - 1);
                validMoves.Add(move);
            }

            // file - 2, row + 1
            if (file > 1 && row < 7
                && !allyBoard[curIndex + 6])
            {
                ChessMove move = new ChessMove(row, file, row + 1, file - 2);
                validMoves.Add(move);
            }

            // file - 2, row - 1
            if (file > 1 && row > 0
                && !allyBoard[curIndex - 10])
            {
                ChessMove move = new ChessMove(row, file, row - 1, file - 2);
                validMoves.Add(move);
            }

            // file - 1, row - 2
            if (file > 0 && row > 1
                && !allyBoard[curIndex - 17])
            {
                ChessMove move = new ChessMove(row, file, row - 2, file - 1);
                validMoves.Add(move);
            }

            // file + 1, row - 2
            if (file < 7 && row > 1
                && !allyBoard[curIndex - 15])
            {
                ChessMove move = new ChessMove(row, file, row - 2, file + 1);
                validMoves.Add(move);
            }

            // file + 2, row - 1
            if (file < 6 && row > 0
                && !allyBoard[curIndex - 6])
            {
                ChessMove move = new ChessMove(row, file, row - 1, file + 2);
                validMoves.Add(move);
            }

            foreach (ChessMove move in validMoves)
                move.PieceType = ChessPieceType.Knight;

            return validMoves;
        }
    }
}
