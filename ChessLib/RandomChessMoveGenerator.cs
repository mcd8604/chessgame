using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class RandomChessMoveGenerator : IChessMoveGenerator
    {
        #region IChessMoveGenerator Members

        Random rand = new Random();

        public bool GenerateMove(ChessGameState state)
        {
            List<ChessPiece> pieceList;

            if (state.CurMoveColor == ChessColor.White)
            {
                pieceList = state.whitePieceList;
            }
            else
            {
                pieceList = state.blackPieceList;
            }

            bool valid = false;
            while (!valid)
            {
                // Select a piece at random
                ChessPiece piece = pieceList[rand.Next(pieceList.Count)];

                // Determine valid moves for the piece type
                List<ChessMove> validMoves = new List<ChessMove>();
                if (piece.type == ChessPieceType.Pawn)
                 {
                    int dir;
                    BitArray enemyPieces;
                    if (piece.color == ChessColor.Black)
                    {
                        dir = 1;
                        enemyPieces = state.WhitePieces;
                    }
                    else
                    {
                        dir = -1;
                        enemyPieces = state.BlackPieces;
                    }

                    int curIndex = piece.row * 8 + piece.file;

                    // 4 potential moves for a pawn

                    // 1 - forward square
                    if (!state.AllPieces[curIndex + (dir * 8)])
                    {
                        ChessMove fwdMove = new ChessMove();
                        fwdMove.srcRow = piece.row;
                        fwdMove.srcFile = piece.file;
                        fwdMove.destFile = piece.file;
                        fwdMove.destRow = piece.row + dir;
                        fwdMove.Color = piece.color;
                        validMoves.Add(fwdMove);

                        // TODO: pawn promotion

                        // 2 - twice forward square

                        // TODO: twice forward square, en passant rule
                    }

                    // 3 - right attack square
                    // (prevent out of bounds)
                    if ((piece.file != 0 || dir < 0) && 
                        (piece.file != 7 || dir > 0) &&
                        enemyPieces[curIndex + (dir * 7)])
                    {
                        ChessMove rightMove = new ChessMove();
                        rightMove.srcRow = piece.row;
                        rightMove.srcFile = piece.file;
                        rightMove.destFile = piece.file - dir;
                        rightMove.destRow = piece.row + dir;
                        rightMove.Piece = ChessPieceType.Pawn;
                        rightMove.Color = piece.color;
                        validMoves.Add(rightMove);
                    }

                    // 4 - left attack square
                    // (prevent out of bounds)
                    if ((piece.file != 0 || dir > 0) &&
                        (piece.file != 7 || dir < 0) && 
                        enemyPieces[curIndex + (dir * 9)])
                    {
                        ChessMove leftMove = new ChessMove();
                        leftMove.srcRow = piece.row;
                        leftMove.srcFile = piece.file;
                        leftMove.destFile = piece.file + dir;
                        leftMove.destRow = piece.row + dir;
                        leftMove.Piece = ChessPieceType.Pawn;
                        leftMove.Color = piece.color;
                        validMoves.Add(leftMove);
                    }
                }
                else if (piece.type == ChessPieceType.Knight)
                {
                    // 8 potential moves for a knight
                }
                else if (piece.type == ChessPieceType.Bishop)
                {

                }
                else if (piece.type == ChessPieceType.Rook)
                {
                    
                }
                else if (piece.type == ChessPieceType.Queen)
                {

                }
                else if (piece.type == ChessPieceType.King)
                {
                    // 8 potential moves for a king
                }

                // Pick a valid move at random
                if (validMoves.Count > 0)
                {
                    valid = state.AddMove(validMoves[rand.Next(validMoves.Count)]);
                }
            }

            return false;
        }

        #endregion
    }
}
