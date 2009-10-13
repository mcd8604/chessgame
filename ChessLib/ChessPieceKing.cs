using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class ChessPieceKing : ChessPiece
    {
        public ChessPieceKing()
        {
            type = ChessPieceType.King;
        }

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

            bool rGT0 = row > 0;
            bool rLT7 = row < 7;
            bool fGT0 = file > 0;
            bool fLT7 = file < 7;

            if (rGT0)
            {
                if (fGT0 && !allyPieces[curIndex - 9])
                {
                    ChessMove move = new ChessMove(row, file, row - 1, file - 1);
                    move.Color = color;
                    move.PieceType = ChessPieceType.King;
                    validMoves.Add(move);
                }

                if (!allyPieces[curIndex - 8])
                {
                    ChessMove move = new ChessMove(row, file, row - 1, file);
                    move.Color = color;
                    move.PieceType = ChessPieceType.King;
                    validMoves.Add(move);
                }

                if (fLT7 && !allyPieces[curIndex - 7])
                {
                    ChessMove move = new ChessMove(row, file, row - 1, file + 1);
                    move.Color = color;
                    move.PieceType = ChessPieceType.King;
                    validMoves.Add(move);
                }
            }

            if(fGT0 && !allyPieces[curIndex - 1])
            {
                ChessMove move = new ChessMove(row, file, row, file - 1);
                move.Color = color;
                move.PieceType = ChessPieceType.King;
                validMoves.Add(move);
            }

            if (fLT7 && !allyPieces[curIndex + 1])
            {
                ChessMove move = new ChessMove(row, file, row, file + 1);
                move.Color = color;
                move.PieceType = ChessPieceType.King;
                validMoves.Add(move);
            }

            if (rLT7)
            {
                if (fGT0 && !allyPieces[curIndex + 7])
                {
                    ChessMove move = new ChessMove(row, file, row + 1, file - 1);
                    move.Color = color;
                    move.PieceType = ChessPieceType.King;
                    validMoves.Add(move);
                }

                if (!allyPieces[curIndex + 8])
                {
                    ChessMove move = new ChessMove(row, file, row + 1, file);
                    move.Color = color;
                    move.PieceType = ChessPieceType.King;
                    validMoves.Add(move);
                }

                if (fLT7 && !allyPieces[curIndex + 9])
                {
                    ChessMove move = new ChessMove(row, file, row + 1, file + 1);
                    move.Color = color;
                    move.PieceType = ChessPieceType.King;
                    validMoves.Add(move);
                }
            }
            
            return validMoves;
        }

        public bool IsInCheck(ChessGameState state)
        {            
            bool inCheck = false;

            int kingIndex = row * 8 + file;

            bool rGT0 = row > 0;
            bool rGT1 = row > 1;
            bool rLT6 = row < 6;
            bool rLT7 = row < 7;

            bool fGT0 = file > 0;
            bool fGT1 = file > 1;
            bool fLT6 = file < 6;
            bool fLT7 = file < 7;

            BitArray enemyKnights;
            BitArray enemyBishops;
            BitArray enemyRooks;
            BitArray enemyQueens;

            if(color == ChessColor.White)
            {
                // validate against black pawns
                if (rGT1 &&
                    (((fGT0 && state.bP[kingIndex - 9]) ||
                    (fLT7 && state.bP[kingIndex - 7]))))
                        return true;

                // get enemy bit boards
                enemyKnights = state.bN;
                enemyBishops = state.bB;
                enemyRooks = state.bR;
                enemyQueens = state.bQ;
            }
            else
            {
                // validate against white pawns
                if (rLT6 &&
                    ((fGT0 && state.wP[kingIndex + 7]) ||
                    (fLT7 && state.wP[kingIndex + 9])))
                        return true;

                // get enemy bit boards
                enemyKnights = state.wN;
                enemyBishops = state.wB;
                enemyRooks = state.wR;
                enemyQueens = state.wQ;
            }
            
            // validate against knights

            if ((rGT1 && fGT0 && enemyKnights[kingIndex - 17]) ||
                (rGT1 && fLT7 && enemyKnights[kingIndex - 15]) ||
                (rGT0 && fLT6 && enemyKnights[kingIndex - 6]) ||
                (rLT7 && fLT6 && enemyKnights[kingIndex + 10]) ||
                (rLT6 && fLT7 && enemyKnights[kingIndex + 17]) ||
                (rLT6 && fGT0 && enemyKnights[kingIndex + 15]) ||
                (rLT7 && fGT1 && enemyKnights[kingIndex + 6]) ||
                (rGT0 && fGT1 && enemyKnights[kingIndex - 10]))
                return true;

            // validate against bishops (and queens)

            // validate against rooks (and queens)

            return inCheck;
        }
    }
}
