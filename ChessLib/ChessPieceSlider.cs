using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class ChessPieceSlider : ChessPiece
    {
        protected List<int> rowDirs;
        protected List<int> fileDirs;

        public ChessPieceSlider()
            : base()
        {
            rowDirs = new List<int>();
            fileDirs = new List<int>();
        }

        public override bool CanMove(int row, int file)
        {
            throw new NotImplementedException();
        }

        public override List<ChessMove> GetValidMoves(ChessGameState state)
        {
            List<ChessMove> validMoves = new List<ChessMove>();

            BitArray allyPieces;
            BitArray enemyPieces;
            if (color == ChessColor.Black)
            {
                allyPieces = state.BlackPieces;
                enemyPieces = state.WhitePieces;
            }
            else
            {
                allyPieces = state.WhitePieces;
                enemyPieces = state.BlackPieces;
            }

            int curIndex = row * 8 + file;

            int curRow, curFile, rowDir, fileDir;

            for(int i = 0; i < rowDirs.Count; ++i)
            {
                rowDir = rowDirs[i];
                fileDir = fileDirs[i];
                curRow = row + rowDir;
                curFile = file + fileDir;

                while (curRow >= 0 && curRow <= 7 && curFile >= 0 && curFile <= 7)
                {
                    curIndex = curRow * 8 + curFile;
                    if (!allyPieces[curIndex])
                    {
                        ChessMove move = new ChessMove(row, file, curRow, curFile);
                        move.Color = color;
                        move.PieceType = type;
                        validMoves.Add(move);
                        if (enemyPieces[curIndex])
                            break;
                    }
                    else break;
                    curRow += rowDir;
                    curFile += fileDir;
                }
            }

            return validMoves;
        }
    }
}
