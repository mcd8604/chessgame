using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    internal abstract class IChessMoveGenerator
    {
        internal abstract bool GenerateMove(ChessGameState state);
    }
}
