using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChessLib
{
    public class ChessPieceBishop : ChessPieceSlider
    {
        public ChessPieceBishop()
            : base()
        {
            type = ChessPieceType.Bishop;

            // 4 directions for a Bishop
            rowDirs.Add(1);
            fileDirs.Add(1);

            rowDirs.Add(1);
            fileDirs.Add(-1);

            rowDirs.Add(-1);
            fileDirs.Add(1);

            rowDirs.Add(-1);
            fileDirs.Add(-1);
        }

        /*public override List<ChessMove> GetValidMoves(ChessGameState state)
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

            // 4 directions to move

            #region row + 1, file + 1

            rowDir = 1;
            fileDir = 1;
            curRow = row + rowDir;
            curFile = file + fileDir;
            
            while(curRow <= 7 && curFile <= 7)
            {
                curIndex = curRow * 8 + curFile;
                if (!allyPieces[curIndex])
                {
                    ChessMove move = new ChessMove(row, file, curRow, curFile);
                    move.Color = color;
                    move.PieceType = ChessPieceType.Bishop;
                    validMoves.Add(move);
                    if (enemyPieces[curIndex])
                        break;
                }
                else break;
                curRow += rowDir;
                curFile += fileDir;
            }

            #endregion

            #region row + 1, file - 1

            rowDir = 1;
            fileDir = -1;
            curRow = row + rowDir;
            curFile = file + fileDir;

            while (curRow <= 7 && curFile >= 0)
            {
                curIndex = curRow * 8 + curFile;
                if (!allyPieces[curIndex])
                {
                    ChessMove move = new ChessMove(row, file, curRow, curFile);
                    move.Color = color;
                    move.PieceType = ChessPieceType.Bishop;
                    validMoves.Add(move);
                    if (enemyPieces[curIndex])
                        break;
                }
                else break;
                curRow += rowDir;
                curFile += fileDir;
            }

            #endregion

            #region row - 1, file + 1

            rowDir = -1;
            fileDir = 1;
            curRow = row + rowDir;
            curFile = file + fileDir;

            while (curRow >= 0 && curFile <= 7)
            {
                curIndex = curRow * 8 + curFile;
                if (!allyPieces[curIndex])
                {
                    ChessMove move = new ChessMove(row, file, curRow, curFile);
                    move.Color = color;
                    move.PieceType = ChessPieceType.Bishop;
                    validMoves.Add(move);
                    if (enemyPieces[curIndex])
                        break;
                }
                else break;
                curRow += rowDir;
                curFile += fileDir;
            }

            #endregion

            #region row - 1, file - 1

            rowDir = -1;
            fileDir = -1;
            curRow = row + rowDir;
            curFile = file + fileDir;

            while (curRow >= 0 && curFile >= 0)
            {
                curIndex = curRow * 8 + curFile;
                if (!allyPieces[curIndex])
                {
                    ChessMove move = new ChessMove(row, file, curRow, curFile);
                    move.Color = color;
                    move.PieceType = ChessPieceType.Bishop;
                    validMoves.Add(move);
                    if (enemyPieces[curIndex])
                        break;
                }
                else break;
                curRow += rowDir;
                curFile += fileDir;
            }

            #endregion

            return validMoves;
        }*/
    }
}
