using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChessLib
{
    public class ChessPlayer
    {
        private ChessColor color;
        private IChessMoveGenerator moveGen;
        private double timeLeft;

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
            if(!state.CheckMate)
                moveGen.GenerateMove(state);            
        }
    }
}
