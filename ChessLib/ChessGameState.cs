using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public enum ChessColor
    {
        White,
        Black
    }

    public enum ChessPieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public class ChessMove
    {
        public ChessMove(int srcRow, int srcFile, int destRow, int destFile)
        {
            this.srcRow = srcRow;
            this.srcFile = srcFile;
            this.destRow = destRow;
            this.destFile = destFile;
        }
        public int srcRow, srcFile, destRow, destFile;
        public ChessColor Color;
        public ChessPieceType PieceType;
        public bool EnPassant;
        public bool Promotion;
        public ChessPieceType PromotionType;
    }

    /*public class ChessPiece
    {
        public ChessPieceType type;
        public ChessColor color;
        public int row;
        public int file;
    }*/

    public class ChessGameState
    {
        private int curMoveIndex = -1;
        public int CurMoveIndex
        {
            get { return curMoveIndex; }
        }

        public List<ChessMove> moves;
        public List<ChessPiece> whitePieceList;
        public List<ChessPiece> blackPieceList;
        public ChessPieceKing whiteKing;
        public ChessPieceKing blackKing;
        public ChessPiece[,] pieceGrid;
        public ChessColor CurMoveColor;

        // Store a BitArray for each piece type and color.
        public BitArray wP, wN, wB, wR, wQ, wK, bP, bN, bB, bR, bQ, bK;

        public BitArray WhitePieces;
        public BitArray BlackPieces;
        public BitArray AllPieces;

#region Default piece sets for new game

        private static readonly bool[] DEFAULT_WHITE_PAWNS = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            true, true, true, true, true, true, true, true, 
            false, false, false, false, false, false, false, false
        };

        private static readonly bool[] DEFAULT_WHITE_KNIGHTS = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, true, false, false, false, false, true, false
        };

        private static readonly bool[] DEFAULT_WHITE_BISHOPS = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, true, false, false, true, false, false
        };

        private static readonly bool[] DEFAULT_WHITE_ROOKS = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            true, false, false, false, false, false, false, true
        };

        private static readonly bool[] DEFAULT_WHITE_QUEEN = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, true, false, false, false, false
        };

        private static readonly bool[] DEFAULT_WHITE_KING = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, true, false, false, false
        };


        private static readonly bool[] DEFAULT_BLACK_PAWNS = 
        {
            false, false, false, false, false, false, false, false, 
            true, true, true, true, true, true, true, true, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
        };

        private static readonly bool[] DEFAULT_BLACK_KNIGHTS = 
        {
            false, true, false, false, false, false, true, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
        };

        private static readonly bool[] DEFAULT_BLACK_BISHOPS = 
        {
            false, false, true, false, false, true, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
        };

        private static readonly bool[] DEFAULT_BLACK_ROOKS = 
        {
            true, false, false, false, false, false, false, true, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
        };

        private static readonly bool[] DEFAULT_BLACK_QUEEN = 
        {
            false, false, false, true, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
        };

        private static readonly bool[] DEFAULT_BLACK_KING = 
        {
            false, false, false, false, true, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
        };

#endregion

        public ChessGameState()
        {
            moves = new List<ChessMove>();
            createNewBoardState();
        }

        private void createNewBoardState()
        {
            whitePieceList = new List<ChessPiece>();
            blackPieceList = new List<ChessPiece>();
            pieceGrid = new ChessPiece[8, 8];

            wP = new BitArray(DEFAULT_WHITE_PAWNS);
            wN = new BitArray(DEFAULT_WHITE_KNIGHTS);
            wB = new BitArray(DEFAULT_WHITE_BISHOPS);
            wR = new BitArray(DEFAULT_WHITE_ROOKS);
            wQ = new BitArray(DEFAULT_WHITE_QUEEN);
            wK = new BitArray(DEFAULT_WHITE_KING);

            WhitePieces = new BitArray(64);
            WhitePieces.Or(wP);
            WhitePieces.Or(wN);
            WhitePieces.Or(wB);
            WhitePieces.Or(wR);
            WhitePieces.Or(wQ);
            WhitePieces.Or(wK);

            bP = new BitArray(DEFAULT_BLACK_PAWNS);
            bN = new BitArray(DEFAULT_BLACK_KNIGHTS);
            bB = new BitArray(DEFAULT_BLACK_BISHOPS);
            bR = new BitArray(DEFAULT_BLACK_ROOKS);
            bQ = new BitArray(DEFAULT_BLACK_QUEEN);
            bK = new BitArray(DEFAULT_BLACK_KING);

            BlackPieces = new BitArray(64);
            BlackPieces.Or(bP);
            BlackPieces.Or(bN);
            BlackPieces.Or(bB);
            BlackPieces.Or(bR);
            BlackPieces.Or(bQ);
            BlackPieces.Or(bK);

            AllPieces = new BitArray(64);
            AllPieces.Or(WhitePieces);
            AllPieces.Or(BlackPieces);

            for (int i = 0; i < 8; ++i)
            {
                ChessPiece whitePawn = new ChessPiecePawn();
                whitePawn.color = ChessColor.White;
                //whitePawn.type = ChessPieceType.Pawn;
                whitePawn.row = 6;
                whitePawn.file = i;
                whitePieceList.Add(whitePawn);
                pieceGrid[i, 6] = whitePawn;

                ChessPiece blackPawn = new ChessPiecePawn();
                blackPawn.color = ChessColor.Black;
                //blackPawn.type = ChessPieceType.Pawn;
                blackPawn.row = 1;
                blackPawn.file = i;
                blackPieceList.Add(blackPawn);
                pieceGrid[i, 1] = blackPawn;

                ChessPiece whitePiece;
                if (i == 0 || i == 7)
                    whitePiece = new ChessPieceRook();
                else if (i == 1 || i == 6)
                    whitePiece = new ChessPieceKnight();
                else if (i == 2 || i == 5)
                    whitePiece = new ChessPieceBishop();
                else if (i == 3)
                    whitePiece = new ChessPieceQueen();
                else //if (i == 4)
                    whitePiece = new ChessPieceKing();
                whitePiece.color = ChessColor.White;
                whitePiece.row = 7;
                whitePiece.file = i;
                whitePieceList.Add(whitePiece);
                pieceGrid[i, 7] = whitePiece;

                ChessPiece blackPiece;
                if (i == 0 || i == 7)
                    blackPiece = new ChessPieceRook();
                else if (i == 1 || i == 6)
                    blackPiece = new ChessPieceKnight();
                else if (i == 2 || i == 5)
                    blackPiece = new ChessPieceBishop();
                else if (i == 3)
                    blackPiece = new ChessPieceQueen();
                else //if (i == 4)
                    blackPiece = new ChessPieceKing();
                blackPiece.color = ChessColor.Black;
                blackPiece.row = 0;
                blackPiece.file = i;
                blackPieceList.Add(blackPiece);
                pieceGrid[i, 0] = blackPiece;
            }

            CurMoveColor = ChessColor.White;
        }

        public bool AddMove(ChessMove move)
        {
            moves.Add(move);

            SetMove(moves.Count - 1);

            // check validity
            // TODO: optimization test - order of operations
            if (move.Color == ChessColor.White && whiteKing.IsInCheck(this) || blackKing.IsInCheck(this))
            {
                moves.RemoveAt(moves.Count - 1);
                return false;
            }
#if DEBUG
            /*Console.WriteLine("--------------\n");
            Console.WriteLine("WHITE PAWNS:");
            PrintBitArray(wP);
            Console.WriteLine("BLACK PAWNS:");
            PrintBitArray(bP);*/
#endif
            return true;
        }

        /// <summary>
        /// Sets the board state to the move at moveIndex.
        /// </summary>
        /// <param name="moveIndex">The index of the move to set the board state.</param>
        public void SetMove(int moveIndex)
        {
            if (moveIndex <= curMoveIndex)
            {
                // TODO: process moves in reverse.
                // For now, just create a new board and reset the move index
                createNewBoardState();
                curMoveIndex = -1;
            }

            ChessMove move;
            int destIndex;
            int srcIndex;

            // Process each move up to the given move index,
            // starting from the current move.
            for (int i = curMoveIndex + 1; i <= moveIndex; ++i)
            {
                move = moves[i];
                    
                if (move.srcRow < 0 || move.srcRow > 7 || move.srcFile < 0 || move.srcFile > 7 ||
                    move.destRow < 0 || move.destRow > 7 || move.destFile < 0 || move.destFile > 7)
                    throw new Exception("Move index out of bounds. Src Row: " +
                        move.srcRow + ", Src File: " + move.srcFile + ", Dest Row: " +
                        move.destRow + ", Dest File: " + move.destFile);

                srcIndex = move.srcRow * 8 + move.srcFile;
                destIndex = move.destRow * 8 + move.destFile;

                //if (srcIndex < 0 || srcIndex > 63 || destIndex < 0 || destIndex > 63)
                //    throw new Exception("Move index out of bounds. Src: " + srcIndex + ", Dest: " + destIndex);

                // Check source index
                if (pieceGrid[move.srcFile, move.srcRow] == null)
                    throw new Exception("Invalid Chess Move.");

                if (move.Color == ChessColor.White)
                {
                    // Check destination index
                    if (WhitePieces[destIndex])
                        throw new Exception("Invalid Chess Move.");

                    // Update source boards
                    if (move.PieceType == ChessPieceType.Pawn)
                    {
                        wP[srcIndex] = false;
                        if (!move.Promotion)
                            wP[destIndex] = true;
                        else
                        {
                            if (move.PromotionType == ChessPieceType.Knight)
                                wN[destIndex] = true;
                            else if (move.PromotionType == ChessPieceType.Bishop)
                                wB[destIndex] = true;
                            else if (move.PromotionType == ChessPieceType.Rook)
                                wR[destIndex] = true;
                            else if (move.PromotionType == ChessPieceType.Queen)
                                wQ[destIndex] = true;
                        }
                    }
                    else if (move.PieceType == ChessPieceType.Knight)
                    {
                        wN[srcIndex] = false;
                        wN[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.Bishop)
                    {
                        wB[srcIndex] = false;
                        wB[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.Rook)
                    {
                        wR[srcIndex] = false;
                        wR[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.Queen)
                    {
                        wQ[srcIndex] = false;
                        wQ[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.King)
                    {
                        wK[srcIndex] = false;
                        wK[destIndex] = true;
                    }

                    WhitePieces[srcIndex] = false;
                    WhitePieces[destIndex] = true;

                    // updated for captured piece

                    if (BlackPieces[destIndex])
                    {
                        BlackPieces[destIndex] = false;
                        bP[destIndex] = false;
                        bN[destIndex] = false;
                        bB[destIndex] = false;
                        bR[destIndex] = false;
                        bQ[destIndex] = false;

                        blackPieceList.Remove(pieceGrid[move.destFile, move.destRow]);
                    }

                    CurMoveColor = ChessColor.Black;
                }
                else
                {
                    // Check destination index
                    if (BlackPieces[destIndex])
                        throw new Exception("Invalid Chess Move.");

                    // Update source boards
                    if (move.PieceType == ChessPieceType.Pawn)
                    {
                        bP[srcIndex] = false;
                        if (!move.Promotion)
                            bP[destIndex] = true;
                        else
                        {
                            if (move.PromotionType == ChessPieceType.Knight)
                                bN[destIndex] = true;
                            else if (move.PromotionType == ChessPieceType.Bishop)
                                bB[destIndex] = true;
                            else if (move.PromotionType == ChessPieceType.Rook)
                                bR[destIndex] = true;
                            else if (move.PromotionType == ChessPieceType.Queen)
                                bQ[destIndex] = true;
                        }
                    }
                    else if (move.PieceType == ChessPieceType.Knight)
                    {
                        bN[srcIndex] = false;
                        bN[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.Bishop)
                    {
                        bB[srcIndex] = false;
                        bB[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.Rook)
                    {
                        bR[srcIndex] = false;
                        bR[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.Queen)
                    {
                        bQ[srcIndex] = false;
                        bQ[destIndex] = true;
                    }
                    else if (move.PieceType == ChessPieceType.King)
                    {
                        bK[srcIndex] = false;
                        bK[destIndex] = true;
                    }

                    BlackPieces[srcIndex] = false;
                    BlackPieces[destIndex] = true;

                    // updated for captured piece

                    if (WhitePieces[destIndex])
                    {
                        WhitePieces[destIndex] = false;
                        wP[destIndex] = false;
                        wN[destIndex] = false;
                        wB[destIndex] = false;
                        wR[destIndex] = false;
                        wQ[destIndex] = false;

                        whitePieceList.Remove(pieceGrid[move.destFile, move.destRow]);
                    }
                    CurMoveColor = ChessColor.White;
                }

                AllPieces[srcIndex] = false;
                AllPieces[destIndex] = true;

                // Update piece and grid
                ChessPiece piece = pieceGrid[move.srcFile, move.srcRow];
                piece.file = move.destFile;
                piece.row = move.destRow;

                // Create promoted piece
                if (move.Promotion)
                {
                    ChessPiece promotedPiece = null;
                    switch (move.PromotionType)
                    {
                        case ChessPieceType.Knight:
                            promotedPiece = new ChessPieceKnight();
                            break;
                        case ChessPieceType.Bishop:
                            promotedPiece = new ChessPieceBishop();
                            break;
                        case ChessPieceType.Rook:
                            promotedPiece = new ChessPieceRook();
                            break;
                        case ChessPieceType.Queen:
                            promotedPiece = new ChessPieceQueen();
                            break;
                    }
                    promotedPiece.color = piece.color;
                    promotedPiece.file = piece.file;
                    promotedPiece.row = piece.row;

                    if (piece.color == ChessColor.Black)
                    {
                        blackPieceList.Remove(piece);
                        blackPieceList.Add(promotedPiece);
                    }
                    else if (piece.color == ChessColor.White)
                    {
                        whitePieceList.Remove(piece);
                        whitePieceList.Add(promotedPiece);
                    }

                    piece = promotedPiece;
                }

                pieceGrid[move.destFile, move.destRow] = piece;
                pieceGrid[move.srcFile, move.srcRow] = null;

                // TODO: determine check/mate and validity


                curMoveIndex = moveIndex;
            }
        }


#if DEBUG
        private void PrintBitArray(BitArray ba)
        {
            if (ba.Length != 64)
                throw new Exception("ChessGameState.PrintBitArray: BitArray.Length is not 64.");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 64; ++i)
            {
                sb.Append(ba[i] ? 1 : 0);
                sb.Append(" ");
                if ((i + 1) % 8 == 0)
                    sb.Append("\n");
            }
            sb.Append("\n");
            Console.WriteLine(sb.ToString());
        }
#endif

        // event StateChanged;
    }
}
