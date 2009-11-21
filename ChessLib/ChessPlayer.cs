using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace ChessLib
{
    public enum PlayerType
    {
        Human,
        Computer
    }

    internal class ChessPlayer
    {
        private ChessColor color;
        private IChessMoveGenerator moveGen;
        private double timeLeft;
        private Thread moveThread;
        private ChessGameState state;
        private bool makingMove;
        /// <summary>
        /// True if waiting for the player to make a move, false if not.
        /// </summary>
        public bool MakingMove { get { return makingMove; } }

        public event ChessMoveEventHandler MadeMove;

        public ChessPlayer(ChessColor color, IChessMoveGenerator moveGenerator, double initTime)
        {
            this.color = color;
            moveGen = moveGenerator;
            timeLeft = initTime;
        }

        public void Update(GameTime time)
        {
            timeLeft -= time.ElapsedGameTime.TotalSeconds;
        }

        public void MakeMove(ChessGameState state)
        {
            if (!makingMove)
            {
                this.state = state;
                moveThread = new Thread(new ThreadStart(doMakeMove));
                moveThread.Start();
                makingMove = true;
            }
        }

        private void doMakeMove()
        {
            if (!state.CheckMate && !state.StaleMate)
            {
                ChessGameState stateClone = (ChessGameState)state.Clone();

                // Ensure stateClone is on the most recent move
                while (stateClone.CurMoveIndex < stateClone.moves.Count - 1)
                    stateClone.MoveForward();

                moveGen.GenerateMove(stateClone);
                MadeMove(this, stateClone);
            }
            makingMove = false;
        }
    }
}
