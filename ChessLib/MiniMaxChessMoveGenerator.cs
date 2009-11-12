using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    /// <summary>
    /// Generates a ChessMove using an implementation of the MiniMax algorithm.
    /// An alpha-beta pruning algorithm is also applied.
    /// TODO: implement iterative deepening
    /// </summary>
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

        private int cutoffs;

        public bool GenerateMove(ChessGameState state)
        {
            curPlayerColor = state.CurMoveColor;
            cutoffs = 0;
            MiniMaxNode node = alphaBetaMax(state, int.MinValue, int.MaxValue, depth);
            Console.WriteLine(cutoffs);

            if(node == null) return false;

            // Perform the move
            List<ChessMove> moves = node.state.moves;
            state.AddMove(moves[state.moves.Count]);

            return true;
        }

        #endregion

        private MiniMaxNode alphaBetaMax(ChessGameState state, int alpha, int beta, int depthLeft)
        {
            int nextDepth = depthLeft - 1;
            List<ChessMove> moves = state.moves;
            List<MiniMaxNode> nodeList = new List<MiniMaxNode>();
            if (depthLeft == 0)
            {
                // Evaluate current state based on captured piece values
                int totalValue = 0;
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
                            totalValue += moveValue;
                        else
                            totalValue -= moveValue;
                    }
                }

                return new MiniMaxNode(state, totalValue);
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
                            if (stateClone.CheckMate)
                            {
                                if (move.Color == curPlayerColor)
                                    return new MiniMaxNode(stateClone, int.MaxValue);
                                else
                                    return new MiniMaxNode(stateClone, int.MinValue);
                            }
                            else if (stateClone.StaleMate)
                                return new MiniMaxNode(stateClone, int.MinValue);

                            MiniMaxNode child = alphaBetaMin(stateClone, alpha, beta, nextDepth);

                            if (child == null)
                                continue;
                            else
                            {
                                int score = child.value;
                                if (score >= beta) // beta cutoff
                                {
                                    ++cutoffs;
                                    return child;
                                }
                                else if (score == alpha) // an equal alpha max
                                    nodeList.Add(child);
                                else if (score > alpha) // a new alpha max
                                {
                                    alpha = score;
                                    nodeList.Clear();
                                    nodeList.Add(child);
                                }
                            }
                        }
                    }
                }
            }
            return nodeList.Count > 0 ? nodeList[rand.Next(nodeList.Count)] : null;
        }

        private MiniMaxNode alphaBetaMin(ChessGameState state, int alpha, int beta, int depthLeft)
        {
            int nextDepth = depthLeft - 1;
            List<ChessMove> moves = state.moves;
            List<MiniMaxNode> nodeList = new List<MiniMaxNode>();
            if (depthLeft == 0)
            {
                // Evaluate current state based on captured piece values
                int totalValue = 0;
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
                            totalValue += moveValue;
                        else
                            totalValue -= moveValue;
                    }
                }

                return new MiniMaxNode(state, -totalValue);
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
                            if (stateClone.CheckMate)
                            {
                                if (move.Color == curPlayerColor)
                                    return new MiniMaxNode(stateClone, int.MaxValue);
                                else
                                    return new MiniMaxNode(stateClone, int.MinValue);
                            }
                            else if (stateClone.StaleMate)
                                return new MiniMaxNode(stateClone, int.MinValue);

                            MiniMaxNode child = alphaBetaMax(stateClone, alpha, beta, nextDepth);

                            if (child == null)
                                continue;
                            else
                            {
                                int score = child.value;
                                if (score <= alpha) // alpha cutoff
                                {
                                    ++cutoffs;
                                    return child;
                                }
                                else if (score == beta) // equal beta min
                                    nodeList.Add(child);
                                else if (score < beta) // new beta min
                                {
                                    beta = score;
                                    nodeList.Clear();
                                    nodeList.Add(child);
                                }
                            }
                        }
                    }
                }
            }
            return nodeList.Count > 0 ? nodeList[rand.Next(nodeList.Count)] : null;
        }
    }
}
