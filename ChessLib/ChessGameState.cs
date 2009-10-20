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

    public class ChessMove : ICloneable
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
#if DEBUG
        public ChessPiece DebugPiece;
#endif
        public bool Capture;
        public ChessPieceType CaptureType;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Color: " + Enum.GetName(typeof(ChessColor), Color));
            sb.AppendLine("Piece Type: " + Enum.GetName(typeof(ChessPieceType), PieceType));
            sb.AppendLine("Source File: " + srcFile);
            sb.AppendLine("Source Row: " + srcRow);
            sb.AppendLine("Destination File: " + destFile);
            sb.AppendLine("Destination Row: " + destRow);
            return sb.ToString();
        }
        #region ICloneable Members

        public object Clone()
        {
            return (ChessMove)this.MemberwiseClone();
        }

        #endregion
    }

    /*public class ChessPiece
    {
        public ChessPieceType type;
        public ChessColor color;
        public int row;
        public int file;
    }*/

    public class ChessGameState : ICloneable
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
        public bool IsInCheck;
        public bool CheckMate;
        public bool StaleMate;

        // Store a BitArray for each piece type and color.
        public BitArray wP, wN, wB, wR, wQ, wK, bP, bN, bB, bR, bQ, bK;

        public BitArray WhitePieces;
        public BitArray WhiteAttacks;
        public BitArray BlackPieces;
        public BitArray BlackAttacks;
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

        private static readonly bool[] DEFAULT_WHITE_ATTACKS = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            true, true, true, true, true, true, true, true, 
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false
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

        private static readonly bool[] DEFAULT_BLACK_ATTACKS = 
        {
            false, false, false, false, false, false, false, false, 
            false, false, false, false, false, false, false, false, 
            true, true, true, true, true, true, true, true,
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
            WhiteAttacks = new BitArray(DEFAULT_WHITE_ATTACKS);

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
            BlackAttacks = new BitArray(DEFAULT_BLACK_ATTACKS);

            AllPieces = new BitArray(64);
            AllPieces.Or(WhitePieces);
            AllPieces.Or(BlackPieces);

            for (int i = 0; i < 8; ++i)
            {
                ChessPiece whitePawn = new ChessPiecePawn();
                whitePawn.color = ChessColor.White;
                whitePawn.row = 6;
                whitePawn.file = i;
                whitePieceList.Add(whitePawn);
                pieceGrid[i, 6] = whitePawn;

                ChessPiece blackPawn = new ChessPiecePawn();
                blackPawn.color = ChessColor.Black;
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
                {
                    whiteKing = new ChessPieceKing();
                    whitePiece = whiteKing;
                }
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
                {
                    blackKing = new ChessPieceKing();
                    blackPiece = blackKing;
                }
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
            //if (move.Color == ChessColor.White && whiteKing.IsInCheck(this) || blackKing.IsInCheck(this))
            bool whiteInCheck = BlackAttacks[whiteKing.row * 8 + whiteKing.file];
            bool blackInCheck = WhiteAttacks[blackKing.row * 8 + blackKing.file];
            
            // Deny the move if it puts that color's king in check 
            if((move.Color == ChessColor.White && whiteInCheck) || 
                (move.Color == ChessColor.Black && blackInCheck))
            {
                PrintBitArray(WhiteAttacks);
                // undo the move
                MoveBackward();
                moves.RemoveAt(moves.Count - 1);
                //SetMove(moves.Count - 1);
                return false;
            }

            IsInCheck = whiteInCheck || blackInCheck;
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
                for (int i = curMoveIndex; i > moveIndex; ++i)
                    MoveBackward();
            else
                for (int i = curMoveIndex; i < moveIndex; ++i)
                    MoveForward();
        }

        public void MoveForward()
        {
            if (curMoveIndex < moves.Count - 1)
            {
                ChessMove move = moves[curMoveIndex + 1];

                int srcRow = move.srcRow;
                int srcFile = move.srcFile;

                int destRow = move.destRow;
                int destFile = move.destFile;

                int srcIndex = srcRow * 8 + srcFile;
                int destIndex = destRow * 8 + destFile;
                
#if DEBUG
                // Index out-of-bounds check
                if (srcRow < 0 || srcRow > 7 || srcFile < 0 || srcFile > 7 ||
                    destRow < 0 || destRow > 7 || destFile < 0 || destFile > 7)
                    throw new Exception("Move index out of bounds. Src Row: " +
                        srcRow + ", Src File: " + srcFile + ", Dest Row: " +
                        destRow + ", Dest File: " + destFile);

                //if (srcIndex < 0 || srcIndex > 63 || destIndex < 0 || destIndex > 63)
                //    throw new Exception("Move index out of bounds. Src: " + srcIndex + ", Dest: " + destIndex);
                
                // Check source index
                if (pieceGrid[srcFile, srcRow] == null)
                    throw new Exception("Invalid Chess Move.");
#endif
                if (move.Color == ChessColor.White)
                {
                    // Check destination index
#if DEBUG
                    if (WhitePieces[destIndex])
                        throw new Exception("Invalid Chess Move.");
#endif
                    // Update white bitboards
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

                    // Remove captured black piece

                    if (BlackPieces[destIndex])
                    {
#if DEBUG
                        if (bK[destIndex])
                            throw new Exception("Attempted to capture black King");
#endif
                        BlackPieces[destIndex] = false;
                        bP[destIndex] = false;
                        bN[destIndex] = false;
                        bB[destIndex] = false;
                        bR[destIndex] = false;
                        bQ[destIndex] = false;

                        ChessPiece capturedPiece = pieceGrid[destFile, destRow];
                        move.CaptureType = capturedPiece.type;
                        blackPieceList.Remove(capturedPiece);
                    }

                    CurMoveColor = ChessColor.Black;
                }
                else
                {
#if DEBUG
                    // Check destination index
                    if (BlackPieces[destIndex])
                        throw new Exception("Invalid Chess Move.");
#endif
                    // Update black bitboards

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

                    // Remove captured white piece

                    if (WhitePieces[destIndex])
                    {
#if DEBUG
                        if (wK[destIndex])
                            throw new Exception("Attempted to capture white King");
#endif 
                        move.Capture = true;

                        WhitePieces[destIndex] = false;
                        wP[destIndex] = false;
                        wN[destIndex] = false;
                        wB[destIndex] = false;
                        wR[destIndex] = false;
                        wQ[destIndex] = false;

                        ChessPiece capturedPiece = pieceGrid[destFile, destRow];
                        move.CaptureType = capturedPiece.type;
                        whitePieceList.Remove(capturedPiece);
                    }
                    CurMoveColor = ChessColor.White;
                }

                AllPieces[srcIndex] = false;
                AllPieces[destIndex] = true;

                ChessPiece piece = pieceGrid[srcFile, srcRow];

                // Promote piece

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
                
                // Update piece location

                piece.file = destFile;
                piece.row = destRow;

                // Update grid

                pieceGrid[destFile, destRow] = piece;
                pieceGrid[srcFile, srcRow] = null;

                updateAttackBoards();

                ++curMoveIndex;
            }
        }

        public void MoveBackward()
        {
            if (curMoveIndex > 0)
            {
                ChessMove move = moves[curMoveIndex];

                int srcRow = move.srcRow;
                int srcFile = move.srcFile;

                int destRow = move.destRow;
                int destFile = move.destFile;

                int srcIndex = srcRow * 8 + srcFile;
                int destIndex = destRow * 8 + destFile;

                ChessPiece piece = pieceGrid[destFile, destRow];
#if DEBUG
                if (move.DebugPiece != null && piece != move.DebugPiece)
                    if(!move.Promotion || piece.type != move.PromotionType)
                        throw new Exception("Invalid Piece");
#endif
                // Demote and update piece

                if (move.Promotion)
                {
                    ChessPiecePawn demotedPawn = new ChessPiecePawn();
                    demotedPawn.file = srcFile;
                    demotedPawn.row = srcRow;
                    demotedPawn.color = piece.color;

                    if (piece.color == ChessColor.Black)
                    {
                        blackPieceList.Remove(piece);
                        blackPieceList.Add(demotedPawn);
                    }
                    else
                    {
                        whitePieceList.Remove(piece);
                        whitePieceList.Add(demotedPawn);
                    }

                    piece = demotedPawn;
                }
                else
                {
                    piece.file = srcFile;
                    piece.row = srcRow;
                }

                // Update grid
#if DEBUG
                if (pieceGrid[srcFile, srcRow] != null)
                    throw new Exception("Piece should not exist in grid square");
#endif
                pieceGrid[srcFile, srcRow] = piece;

                if (move.Color == ChessColor.White)
                {
                    // Update white bitboards

                    if (move.PieceType == ChessPieceType.Pawn)
                    {
                        wP[srcIndex] = true;
                        if (!move.Promotion)
                            wP[destIndex] = false;
                        else
                        {
                            if (move.PromotionType == ChessPieceType.Knight)
                                wN[destIndex] = false;
                            else if (move.PromotionType == ChessPieceType.Bishop)
                                wB[destIndex] = false;
                            else if (move.PromotionType == ChessPieceType.Rook)
                                wR[destIndex] = false;
                            else if (move.PromotionType == ChessPieceType.Queen)
                                wQ[destIndex] = false;
                        }
                    }
                    else if (move.PieceType == ChessPieceType.Knight)
                    {
                        wN[srcIndex] = true;
                        wN[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.Bishop)
                    {
                        wB[srcIndex] = true;
                        wB[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.Rook)
                    {
                        wR[srcIndex] = true;
                        wR[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.Queen)
                    {
                        wQ[srcIndex] = true;
                        wQ[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.King)
                    {
                        wK[srcIndex] = true;
                        wK[destIndex] = false;
                    }

                    WhitePieces[srcIndex] = true;
                    WhitePieces[destIndex] = false;

                    // Restore captured black piece

                    if (move.Capture)
                    {
                        ChessPiece capturedPiece = null;
                        if (move.CaptureType == ChessPieceType.Pawn)
                        {
                            bP[destIndex] = true;
                            capturedPiece = new ChessPiecePawn();
                        }
                        if (move.CaptureType == ChessPieceType.Knight)
                        {
                            bN[destIndex] = true;
                            capturedPiece = new ChessPieceKnight();
                        }
                        else if (move.CaptureType == ChessPieceType.Bishop)
                        {
                            bB[destIndex] = true;
                            capturedPiece = new ChessPieceBishop();
                        }
                        else if (move.CaptureType == ChessPieceType.Rook)
                        {
                            bR[destIndex] = true;
                            capturedPiece = new ChessPieceRook();
                        }
                        else if (move.CaptureType == ChessPieceType.Queen)
                        {
                            bQ[destIndex] = true;
                            capturedPiece = new ChessPieceQueen();
                        }
                        capturedPiece.file = destFile;
                        capturedPiece.row = destRow;
                        capturedPiece.color = ChessColor.Black;
                        BlackPieces[destIndex] = true;
                        blackPieceList.Add(capturedPiece);
                        pieceGrid[destFile, destRow] = capturedPiece;
                    }
                    else 
                        pieceGrid[destFile, destRow] = null;
                }
                else
                {
                    // Update black bitboards

                    if (move.PieceType == ChessPieceType.Pawn)
                    {
                        bP[srcIndex] = true;
                        if (!move.Promotion)
                            bP[destIndex] = false;
                        else
                        {
                            if (move.PromotionType == ChessPieceType.Knight)
                                bN[destIndex] = false;
                            else if (move.PromotionType == ChessPieceType.Bishop)
                                bB[destIndex] = false;
                            else if (move.PromotionType == ChessPieceType.Rook)
                                bR[destIndex] = false;
                            else if (move.PromotionType == ChessPieceType.Queen)
                                bQ[destIndex] = false;
                        }
                    }
                    else if (move.PieceType == ChessPieceType.Knight)
                    {
                        bN[srcIndex] = true;
                        bN[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.Bishop)
                    {
                        bB[srcIndex] = true;
                        bB[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.Rook)
                    {
                        bR[srcIndex] = true;
                        bR[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.Queen)
                    {
                        bQ[srcIndex] = true;
                        bQ[destIndex] = false;
                    }
                    else if (move.PieceType == ChessPieceType.King)
                    {
                        bK[srcIndex] = true;
                        bK[destIndex] = false;
                    }

                    BlackPieces[srcIndex] = true;
                    BlackPieces[destIndex] = false;

                    // Restore captured white piece

                    if (move.Capture)
                    {
                        ChessPiece capturedPiece = null;
                        if (move.CaptureType == ChessPieceType.Pawn)
                        {
                            wP[destIndex] = true;
                            capturedPiece = new ChessPiecePawn();
                        }
                        if (move.CaptureType == ChessPieceType.Knight)
                        {
                            wN[destIndex] = true;
                            capturedPiece = new ChessPieceKnight();
                        }
                        else if (move.CaptureType == ChessPieceType.Bishop)
                        {
                            wB[destIndex] = true;
                            capturedPiece = new ChessPieceBishop();
                        }
                        else if (move.CaptureType == ChessPieceType.Rook)
                        {
                            wR[destIndex] = true;
                            capturedPiece = new ChessPieceRook();
                        }
                        else if (move.CaptureType == ChessPieceType.Queen)
                        {
                            wQ[destIndex] = true;
                            capturedPiece = new ChessPieceQueen();
                        }
                        capturedPiece.file = destFile;
                        capturedPiece.row = destRow;
                        capturedPiece.color = ChessColor.White;
                        WhitePieces[destIndex] = true;
                        whitePieceList.Add(capturedPiece);
                        pieceGrid[destFile, destRow] = capturedPiece;
                    }
                    else
                        pieceGrid[destFile, destRow] = null;
                }

                CurMoveColor = move.Color;

                AllPieces[srcIndex] = true;
                AllPieces[destIndex] = move.Capture;

                updateAttackBoards();

                --curMoveIndex;
            }
        }

        private void updateAttackBoards()
        {
            WhiteAttacks.SetAll(false);
            foreach (ChessPiece piece in whitePieceList)
            {
                List<ChessMove> moves = piece.GetValidMoves(this);
                foreach (ChessMove m in moves)
                    if (!(piece is ChessPiecePawn) || m.srcFile != m.destFile)
                        WhiteAttacks[m.destRow * 8 + m.destFile] = true;
            }
            BlackAttacks.SetAll(false);
            foreach (ChessPiece piece in blackPieceList)
            {
                List<ChessMove> moves = piece.GetValidMoves(this);
                foreach (ChessMove m in moves)
                    if (!(piece is ChessPiecePawn) || m.srcFile != m.destFile)
                        BlackAttacks[m.destRow * 8 + m.destFile] = true;
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

        #region ICloneable Members

        public object Clone()
        {
            ChessGameState state = new ChessGameState();

            state.curMoveIndex = this.curMoveIndex;
            state.CurMoveColor = this.CurMoveColor;
            state.IsInCheck = this.IsInCheck;
            state.CheckMate = this.CheckMate;
            state.StaleMate = this.StaleMate;

            // Clone BitArrays

            state.wP = (BitArray)this.wP.Clone();
            state.wK = (BitArray)this.wK.Clone();
            state.wB = (BitArray)this.wB.Clone();
            state.wR = (BitArray)this.wR.Clone();
            state.wQ = (BitArray)this.wQ.Clone();
            state.wK = (BitArray)this.wK.Clone();
            state.WhitePieces = (BitArray)this.WhitePieces.Clone();
            state.WhiteAttacks = (BitArray)this.WhiteAttacks.Clone();

            state.bP = (BitArray)this.bP.Clone();
            state.bK = (BitArray)this.bK.Clone();
            state.bB = (BitArray)this.bB.Clone();
            state.bR = (BitArray)this.bR.Clone();
            state.bQ = (BitArray)this.bQ.Clone();
            state.bK = (BitArray)this.bK.Clone();
            state.BlackPieces = (BitArray)this.BlackPieces.Clone();
            state.BlackAttacks = (BitArray)this.BlackAttacks.Clone();

            state.AllPieces = (BitArray)this.AllPieces.Clone();

            // Clone ChessPieces, lists and grid

            state.pieceGrid = new ChessPiece[8, 8];

            state.whitePieceList = new List<ChessPiece>();
            foreach (ChessPiece piece in this.whitePieceList)
            {
                ChessPiece pieceClone = (ChessPiece)piece.Clone();
                if (pieceClone is ChessPieceKing)
                    state.whiteKing = (ChessPieceKing)pieceClone;
                state.whitePieceList.Add(pieceClone);
                state.pieceGrid[pieceClone.file, pieceClone.row] = pieceClone;
            }
            state.blackPieceList = new List<ChessPiece>();
            foreach (ChessPiece piece in this.blackPieceList)
            {
                ChessPiece pieceClone = (ChessPiece)piece.Clone();
                if (pieceClone is ChessPieceKing)
                    state.blackKing = (ChessPieceKing)pieceClone;
                state.blackPieceList.Add(pieceClone);
                state.pieceGrid[pieceClone.file, pieceClone.row] = pieceClone;
            }

            // Clone move list

            foreach (ChessMove move in this.moves)
                state.moves.Add((ChessMove)move.Clone());

            return state;
        }

        #endregion
    }
}
