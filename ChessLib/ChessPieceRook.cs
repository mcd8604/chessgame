using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public class ChessPieceRook : ChessPieceSlider
    {
        public ChessPieceRook()
            : base()
        {
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
