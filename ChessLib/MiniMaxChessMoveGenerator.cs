using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public class MiniMaxChessMoveGenerator : IChessMoveGenerator
    {
        private class MiniMaxNode
        {
            internal ChessGameState state;
            internal int value;
            internal MiniMaxNode(ChessGameState state, int value)
            {
                this.state = state;
                this.value = value;
            }
        }

        private Random rand = new Random();

        private int depth;
        private ChessColor curPlayerColor;

        public MiniMaxChessMoveGenerator(int depth)
        {
            this.depth = depth;
        }

        #region IChessMoveGenerator Members

        public bool GenerateMove(ChessGameState state)
        {
            curPlayerColor = state.CurMoveColor;
            MiniMaxNode node = doMiniMax(state, 0, depth);

            if(node == null) return false;

            // Perform the move
            List<ChessMove> moves = node.state.moves;
            state.AddMove(moves[moves.Count - depth]);

            return true;
        }

        #endregion

        private MiniMaxNode doMiniMax(ChessGameState state, int value, int curDepth)
        {
            List<ChessMove> moves = state.moves;
            MiniMaxNode best = null;
            int nextDepth = curDepth - 1;
            List<MiniMaxNode> bestList = new List<MiniMaxNode>();
            if (state.CheckMate)
            {
                ChessMove lastMove = moves[moves.Count - 1];
                if (lastMove.Color == curPlayerColor)
                    return new MiniMaxNode(state, int.MaxValue);
                else
                    return new MiniMaxNode(state, int.MinValue);
            }
            else if (state.StaleMate)
                return new MiniMaxNode(state, int.MinValue);
            else if (curDepth == 0)
            {
                // Evaluate current state based on captured piece values
                for (int i = moves.Count - this.depth; i < moves.Count; ++i)
                {
                    ChessMove move = moves[i];
                    int moveValue = 0;

                    if (move.Capture)
                    {
                        switch (move.CaptureType)
                        {
                            case ChessPieceType.Pawn:
                                moveValue = 1;
                                break;
                            case ChessPieceType.Knight:
                            case ChessPieceType.Bishop:
                                moveValue = 3;
                                break;
                            case ChessPieceType.Rook:
                                moveValue = 5;
                                break;
                            case ChessPieceType.Queen:
                                moveValue = 9;
                                break;
                        }
                        if (move.Color == curPlayerColor)
                            value += moveValue;
                        else
                            value -= moveValue;
                    }
                }

                return new MiniMaxNode(state, value);
            }
            else
            {
                // Get current color's pieces
                List<ChessPiece> pieces;
                if (state.CurMoveColor == ChessColor.White)
                    pieces = state.whitePieceList;
                else
                    pieces = state.blackPieceList;

                // Get valid moves, then perform MiniMax on 
                // each child to determine the best node
                foreach (ChessPiece piece in pieces)
                {
                    List<ChessMove> validMoves = piece.GetValidMoves(state);
                    foreach (ChessMove move in validMoves)
                    {
                        ChessGameState stateClone = state.Clone() as ChessGameState;
                        if (stateClone.AddMove(move))
                        {
                            MiniMaxNode child = doMiniMax(stateClone, value, nextDepth);

                            if (child == null)
                                continue;
                            if (best == null)
                            {
                                bestList.Add(child);
                                best = child;
                            }
                            else
                            {
                                if (child.value > best.value)
                                {
                                    bestList.Clear();
                                    bestList.Add(child);
                                    best = child;
                                }
                                else if (child.value == best.value)
                                {
                                    bestList.Add(child);
                                }
                            }
                        }
                    }
                }
            }
            return bestList.Count > 0 ? bestList[rand.Next(bestList.Count)] : null;
        }
    }
}
