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

                if (state.pieceGrid[piece.file, piece.row] != piece)
                    throw new Exception("Invalid Chess Move.");
                
                if (piece.color != state.CurMoveColor)
                    throw new Exception("Invalid Piece Color");

                // Determine valid moves for the piece type
                List<ChessMove> validMoves = piece.GetValidMoves(state);               

                foreach (ChessMove move in validMoves)
                {
                    move.Color = piece.color;

                    // Pawn promotion
                    if (piece is ChessPiecePawn)
                    {
                        if (move.destRow == 0 || move.destRow == 7)
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

                // Pick a valid move at random
                if (validMoves.Count > 0)
                {
                    ChessMove m = validMoves[rand.Next(validMoves.Count)];
#if DEBUG
                    m.DebugPiece = piece;
#endif
                    valid = state.AddMove(m);
                }
            }

            return false;
        }

        #endregion
    }
}
