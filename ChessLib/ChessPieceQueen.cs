using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public class ChessPieceQueen : ChessPieceSlider
    {
        public ChessPieceQueen()
            : base()
        {
            type = ChessPieceType.Queen;

            // 8 directions for a Queen
            rowDirs.Add(1);
            fileDirs.Add(1);

            rowDirs.Add(-1);
            fileDirs.Add(1);

            rowDirs.Add(1);
            fileDirs.Add(-1);

            rowDirs.Add(-1);
            fileDirs.Add(-1);

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
