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
                        ChessMove fwdMove = new ChessMove(piece.row, piece.file, piece.row + dir, piece.file);
                        validMoves.Add(fwdMove);

                        // 2 - twice forward square

                        // if Black pawn in initial square or
                        // if White pawn in initial square
                        if (((dir > 0 && piece.row == 1) || 
                            (dir < 0 && piece.row == 6)) &&                             
                            !state.AllPieces[curIndex + (dir * 16)])
                        {
                            ChessMove fwdTwoMove = new ChessMove(piece.row, piece.file, piece.row + dir * 2, piece.file);
                            fwdTwoMove.EnPassant = true;
                            validMoves.Add(fwdTwoMove);
                        }
                    }

                    bool eP = state.moves.Count > 0 ? state.moves[state.moves.Count - 1].EnPassant : false;

                    // 3 - right attack square
                    // (prevent out of bounds)
                    if ((piece.file != 0 || dir < 0) && 
                        (piece.file != 7 || dir > 0) &&
                        (enemyPieces[curIndex + (dir * 7)] ||
                        (eP && enemyPieces[curIndex - dir])))
                    {
                        ChessMove rightMove = new ChessMove(piece.row, piece.file, piece.row + dir, piece.file - dir);
                        validMoves.Add(rightMove);
                    }

                    // 4 - left attack square
                    // (prevent out of bounds)
                    if ((piece.file != 0 || dir > 0) &&
                        (piece.file != 7 || dir < 0) && 
                        (enemyPieces[curIndex + (dir * 9)] ||
                        (eP && enemyPieces[curIndex + dir])))
                    {
                        ChessMove leftMove = new ChessMove(piece.row, piece.file, piece.row + dir, piece.file + dir);
                        validMoves.Add(leftMove);
                    }

                    foreach(ChessMove move in validMoves)
                    {
                        move.PieceType = ChessPieceType.Pawn;
                        move.Color = piece.color;

                        // Pawn promotion
                        if (move.destRow == 0 ||  move.destRow == 7) 
                        {
                            move.Promotion = true;

                            // Pick a random piece type for promotion
                            switch (rand.Next(4))
                            {
                                case 0:
                                    move.PromotionType = ChessPieceType.Knight;
                                    break;
                                case 1:
                                    move.PromotionType = ChessPieceType.Bishop;
                                    break;
                                case 2:
                                    move.PromotionType = ChessPieceType.Rook;
                                    break;
                                case 3:
                                    move.PromotionType = ChessPieceType.Queen;
                                    break;
                            }
                        }
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
