using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessGame
{
    /// <summary>
    /// Handles mouse input and draws the chess piece
    /// </summary>
    class ChessBoardSquare : DrawableGameComponent
    {

        private SpriteBatch spriteBatch;
        public SpriteBatch MySpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        private SpriteFont font;
        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        private string chessPiece = string.Empty;
        public string ChessPiece
        {
            get { return chessPiece; }
            set { chessPiece = value; }
        }

        private Rectangle destination;
        public Rectangle Destination
        {
            get { return destination; }
            set { 
                destination = value;
                position = new Vector2(destination.Center.X, destination.Center.Y);
            }
        }

        private Vector2 position;

        private Color pieceColor;
        public Color PieceColor
        {
            get { return pieceColor; }
            set { pieceColor = value; }
        }

        private Texture2D backgroundTexture;
        public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
            set { backgroundTexture = value; }
        }

        public ChessBoardSquare(Game game): base(game) {}

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, destination, Color.White);
            spriteBatch.DrawString(font, chessPiece, position, pieceColor);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
