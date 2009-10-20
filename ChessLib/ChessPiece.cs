using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public abstract class ChessPiece : ICloneable
    {
        public ChessPieceType type;
        public ChessColor color;
        public int row;
        public int file;

        public ChessPiece() { }

        public ChessPiece(ChessColor color)
        {
            this.color = color;
        }

        public abstract bool CanMove(int row, int file);

        public abstract List<ChessMove> GetValidMoves(ChessGameState state);

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
