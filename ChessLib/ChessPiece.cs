using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public struct ChessPieceInfo
    {
        public ChessPieceType Type;
        public ChessColor Color;
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

    public abstract class ChessPiece : ICloneable
    {
        internal ChessPieceType type;
        internal ChessColor color;
        internal int row;
        internal int file;

        internal BitArray[,] attackBoards;

        internal ChessPiece() { }

        internal ChessPiece(ChessColor color)
        {
            this.color = color;
        }

        internal abstract bool CanMove(int row, int file);

        internal abstract List<ChessMove> GetValidMoves(ChessGameState state);

        internal BitArray GetAttackBoard()
        {
            return attackBoards[row, file];
        }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
