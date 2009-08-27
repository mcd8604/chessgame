using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    internal abstract class IChessPiece
    {
        protected ChessColor color;

        internal IChessPiece(ChessColor color)
        {
            this.color = color;
        }

        internal abstract bool CanMove(int row, int file);
    }
}
