using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    internal class ChessPiecePawn : ChessPiece
    {
        internal ChessPiecePawn()
        {
            type = ChessPieceType.Pawn;
        }

        internal override bool CanMove(int row, int file)
        {
            throw new NotImplementedException();
        }

        internal override List<ChessMove> GetValidMoves(ChessGameState state)
        {
            List<ChessMove> validMoves = new List<ChessMove>();

            int dir;
            BitArray enemyPieces;
            if (color == ChessColor.Black)
            {
                dir = 1;
                enemyPieces = state.WhitePieces;
            }
            else
            {
                dir = -1;
                enemyPieces = state.BlackPieces;
            }

            int curIndex = row * 8 + file;

            // 4 potential moves for a pawn

            // 1 - forward square
            if (!state.AllPieces[curIndex + (dir * 8)])
            {
                ChessMove fwdMove = new ChessMove(row, file, row + dir, file);
                validMoves.Add(fwdMove);

                // 2 - twice forward square

                // if Black pawn in initial square or
                // if White pawn in initial square
                if (((dir > 0 && row == 1) ||
                    (dir < 0 && row == 6)) &&
                    !state.AllPieces[curIndex + (dir * 16)])
                {
                    ChessMove fwdTwoMove = new ChessMove(row, file, row + dir * 2, file);
                    fwdTwoMove.EnPassant = true;
                    validMoves.Add(fwdTwoMove);
                }
            }

            ChessMove lastMove = null;
            if (state.moves.Count > 0)
                lastMove = state.moves[state.moves.Count - 1];

            // 3 - right attack square
            // (prevent out of bounds)
            if ((file != 0 || dir < 0) &&
                (file != 7 || dir > 0) &&
                (enemyPieces[curIndex + (dir * 7)] ||
                // EnPassant
                (lastMove != null && 
                lastMove.EnPassant && 
                lastMove.destRow == row && 
                lastMove.destFile == file - dir)))
            {
                ChessMove rightMove = new ChessMove(row, file, row + dir, file - dir);
                validMoves.Add(rightMove);
            }

            // 4 - left attack square
            // (prevent out of bounds)
            if ((file != 0 || dir > 0) &&
                (file != 7 || dir < 0) &&
                (enemyPieces[curIndex + (dir * 9)] ||
                // EnPassant
                (lastMove != null && 
                lastMove.EnPassant && 
                lastMove.destRow == row && 
                lastMove.destFile == file + dir)))
            {
                ChessMove leftMove = new ChessMove(row, file, row + dir, file + dir);
                validMoves.Add(leftMove);
            }

            List<ChessMove> promotionMoves = new List<ChessMove>();
            foreach (ChessMove move in validMoves)
            {
                move.PieceType = ChessPieceType.Pawn;
                move.Color = this.color;
                
                // Create promotion moves
                if (move.destRow == 0 || move.destRow == 7)
                {
                    move.Promotion = true;
                    move.PromotionType = ChessPieceType.Knight;

                    ChessMove toBishop = (ChessMove)move.Clone();
                    toBishop.PromotionType = ChessPieceType.Bishop;
                    promotionMoves.Add(toBishop);

                    ChessMove toRook = (ChessMove)move.Clone();
                    toRook.PromotionType = ChessPieceType.Rook;
                    promotionMoves.Add(toRook);

                    ChessMove toQueen = (ChessMove)move.Clone();
                    toBishop.PromotionType = ChessPieceType.Queen;
                    promotionMoves.Add(toQueen);
                }
            }
            validMoves.AddRange(promotionMoves);

            return validMoves;
        }
    }
}
