using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessLib
{
    public enum ChessColor
    {
        White,
        Black
    }

    /// <summary>
    /// Represents the entirety of a Chess game.
    /// Contains the ChessGameState and 2 ChessPlayers.
    /// </summary>
    public class ChessGameProxy
    {
        private ChessGameState state;
        private ChessPlayer playerWhite;
        private ChessPlayer playerBlack;
        private ChessPlayer currentPlayer;

        /// <summary>
        /// Raised when the ChessGame is updated.
        /// </summary>
        public event ChessGameUpdate Updated;

        /// <summary>
        /// Creates a new ChessGame 
        /// </summary>
        public ChessGameProxy() { }

        /// <summary>
        /// Starts a new game with ChessPlayers of the given PlayerTypes.
        /// </summary>
        /// <param name="playerWhite">PlayerType for White.</param>
        /// <param name="playerBlack">PlayerType for Black.</param>
        public void StartGame(PlayerType playerTypeWhite, PlayerType playerTypeBlack)
        {
            state = new ChessGameState();
            state.SetNewGame();

            IChessMoveGenerator moveGen = null;

            if (playerTypeWhite == PlayerType.Human)
                moveGen = new HumanChessMoveGenerator();
            else
                moveGen = new MiniMaxChessMoveGenerator(4);

            playerWhite = new ChessPlayer(ChessColor.White, moveGen, 0);
            currentPlayer = playerWhite;

            if (playerTypeBlack == PlayerType.Human)
                moveGen = new HumanChessMoveGenerator();
            else
                moveGen = new MiniMaxChessMoveGenerator(4);

            playerBlack = new ChessPlayer(ChessColor.Black, moveGen, 0);

            playerWhite.MadeMove += new ChessMoveEventHandler(playerWhite_MadeMove);
            playerBlack.MadeMove += new ChessMoveEventHandler(playerBlack_MadeMove);

            dispatchUpdate();
        }

        private void playerBlack_MadeMove(ChessPlayer player, ChessGameState newState)
        {
            state = newState;
            currentPlayer = playerWhite;
            currentPlayer.MakeMove(state);

            dispatchUpdate();
        }

        private void playerWhite_MadeMove(ChessPlayer player, ChessGameState newState)
        {
            state = newState;
            currentPlayer = playerBlack;
            currentPlayer.MakeMove(state);

            dispatchUpdate();
        }

        private void dispatchUpdate()
        {
            ChessPieceInfo?[,] pieceInfo = new ChessPieceInfo?[8, 8];

            foreach (ChessPiece p in state.whitePieceList)
            {
                ChessPieceInfo i =  new ChessPieceInfo();
                i.Color = p.color;
                i.Type = p.type;
                pieceInfo[p.file, p.row] = i;
            }

            foreach (ChessPiece p in state.blackPieceList)
            {
                ChessPieceInfo i = new ChessPieceInfo();
                i.Color = p.color;
                i.Type = p.type;
                pieceInfo[p.file, p.row] = i;
            }

            if (Updated != null)
                Updated.Invoke(pieceInfo);
        }
            
        public void MakeMove()
        {
            currentPlayer.MakeMove(state);
        }

        public void MoveBackward()
        {
            state.MoveBackward();
            dispatchUpdate();
        }

        public void MoveForward()
        {
            state.MoveForward();
            dispatchUpdate();
        }

    }
}
