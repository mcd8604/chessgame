using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public interface IChessMoveGenerator
    {
        bool GenerateMove(ChessGameState state);
    }
}
