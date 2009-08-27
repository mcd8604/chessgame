using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    abstract class AIChessMoveGenerator : IChessMoveGenerator
    {
        #region IChessMoveGenerator Members

        public bool GenerateMove(ChessGameState state)
        {
            ChessColor color;

            if(state.CurMoveColor == ChessColor.White)
                color = ChessColor.Black;
            else
                color = ChessColor.White;

            return state.AddMove(GenerateMove(color));
        }

        #endregion

        protected abstract ChessMove GenerateMove(ChessColor color);
    }
}
