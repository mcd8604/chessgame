#undef SHOW_TREE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if SHOW_TREE
using System.Windows.Forms;
#endif

namespace ChessLib
{
    /// <summary>
    /// Generates a ChessMove using an implementation of the MiniMax algorithm.
    /// An alpha-beta pruning algorithm is also applied.
    /// TODO: implement iterative deepening
    /// </summary>
    internal class MiniMaxChessMoveGenerator : IChessMoveGenerator
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

#if SHOW_TREE
        private TreeViewForm tree;
#endif
        internal MiniMaxChessMoveGenerator(int depth)
        {
            this.depth = depth;
#if SHOW_TREE
            tree = new TreeViewForm();
            tree.Show();
#endif
        }

        #region IChessMoveGenerator Members

        private int cutoffs;

        internal override bool GenerateMove(ChessGameState state)
        {
            curPlayerColor = state.CurMoveColor;
            cutoffs = 0;

#if SHOW_TREE
            // Set root TreeNode
            TreeNode root = new TreeNode();
            root.Text = "A: " + int.MinValue + ",   B:" + int.MaxValue;
            tree.AddNode(null, root);

            MiniMaxNode alpha = alphaBetaMax(state, int.MinValue, int.MaxValue, depth, root);
#else
            MiniMaxNode alpha = alphaBetaMax(state, int.MinValue, int.MaxValue, depth);
#endif
            Console.WriteLine(cutoffs);

            if (alpha == null) return false;

            // Perform the move
            List<ChessMove> moves = alpha.state.moves;
            state.AddMove(moves[state.moves.Count]);

            return true;
        }

        #endregion

#if SHOW_TREE
        private MiniMaxNode alphaBetaMax(ChessGameState state, int alpha, int beta, int depthLeft, TreeNode curNode)
#else
        private MiniMaxNode alphaBetaMax(ChessGameState state, int alpha, int beta, int depthLeft)
#endif
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
#if SHOW_TREE
                tree.SetNode(curNode, totalValue.ToString());
#endif
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
#if SHOWTREE
                            TreeNode childNode = new TreeNode("A:" + alpha + ",  B:" + beta);
                            tree.AddNode(curNode, childNode);
                            MiniMaxNode child = alphaBetaMin(stateClone, alpha, beta, nextDepth, childNode);
#else
                            MiniMaxNode child = alphaBetaMin(stateClone, alpha, beta, nextDepth);
#endif

                            if (child == null)
                                continue;
                            else
                            {
                                int score = child.value;
                                if (score > beta) // beta cutoff
                                {
                                    ++cutoffs;
#if SHOW_TREE
                                    tree.SetNode(curNode, curNode.Text + "Beta Cutoff");
#endif
                                    return child;
                                }
                                else if (score == alpha) // an equal alpha max
                                    nodeList.Add(child);
                                else if (score > alpha) // a new alpha max
                                {
                                    alpha = score;
#if SHOW_TREE
                                    tree.SetNode(curNode, "A:" + alpha + ",  B:" + beta);
#endif
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

#if SHOW_TREE
        private MiniMaxNode alphaBetaMin(ChessGameState state, int alpha, int beta, int depthLeft, TreeNode curNode)
#else
        private MiniMaxNode alphaBetaMin(ChessGameState state, int alpha, int beta, int depthLeft)
#endif
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
#if SHOW_TREE
                tree.SetNode(curNode, totalValue.ToString());
#endif
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
#if SHOW_TREE 
                            TreeNode childNode = new TreeNode("A:" + alpha + ",  B:" + beta);
                            tree.AddNode(curNode, childNode);
                            MiniMaxNode child = alphaBetaMax(stateClone, alpha, beta, nextDepth, childNode);
#else
                            MiniMaxNode child = alphaBetaMax(stateClone, alpha, beta, nextDepth);
#endif
                            if (child == null)
                                continue;
                            else
                            {
                                int score = child.value;
                                if (score < alpha) // alpha cutoff
                                {
                                    ++cutoffs;
#if SHOW_TREE
                                    tree.SetNode(curNode, curNode.Text + "Alpha Cutoff");
#endif
                                    return child;
                                }
                                else if (score == beta) // equal beta min
                                    nodeList.Add(child);
                                else if (score < beta) // new beta min
                                {
                                    beta = score;
#if SHOW_TREE
                                    tree.SetNode(curNode, "A:" + alpha + ",  B:" + beta);
#endif
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
