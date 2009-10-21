using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace ChessLib
{
    public class ChessPlayer
    {
        private ChessColor color;
        private IChessMoveGenerator moveGen;
        private double timeLeft;
        private Thread moveThread;
        private ChessGameState state; 

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
            this.state = state;
            moveThread = new Thread(new ThreadStart(doMakeMove));
            moveThread.Start();
        }

        private void doMakeMove()
        {
            if (!state.CheckMate)
                moveGen.GenerateMove(state);
        }
    }
}
