using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    internal class ChessPieceRook : ChessPieceSlider
    {
        internal ChessPieceRook()
            : base()
        {
            type = ChessPieceType.Rook;

            // 4 directions for a Rook
            rowDirs.Add(0);
            fileDirs.Add(1);

            rowDirs.Add(1);
            fileDirs.Add(0);

            rowDirs.Add(0);
            fileDirs.Add(-1);

            rowDirs.Add(-1);
            fileDirs.Add(0);
        }
    }
}
