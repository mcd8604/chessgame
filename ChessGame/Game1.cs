using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using ChessLib;

namespace ChessGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ChessGameState gs;
        ChessPlayer playerWhite;
        ChessPlayer playerBlack;
        ChessPlayer currentPlayer;
        Texture2D whiteSquare;
        Texture2D blackSquare;

        const int boardDimension = 800;
        const int squareSize = 100;

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gs = new ChessGameState();
            currentPlayer = playerWhite = new ChessPlayer(ChessColor.White, new MiniMaxChessMoveGenerator(4), 0);
            playerWhite.MadeMove += new ChessMoveEventHandler(playerWhite_MadeMove);
            playerBlack = new ChessPlayer(ChessColor.Black, new MiniMaxChessMoveGenerator(4), 0);
            playerBlack.MadeMove += new ChessMoveEventHandler(playerBlack_MadeMove);
        }

        void playerBlack_MadeMove(ChessPlayer player, ChessGameState state)
        {
            gs = state;
            currentPlayer = playerWhite;
        }

        void playerWhite_MadeMove(ChessPlayer player, ChessGameState state)
        {
            gs = state;
            currentPlayer = playerBlack;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferHeight = boardDimension;
            graphics.PreferredBackBufferWidth = boardDimension;
            graphics.ApplyChanges();

            // Create square textures
            Color[] whiteData = { Color.Silver };
            whiteSquare = new Texture2D(GraphicsDevice, 1, 1);
            whiteSquare.SetData<Color>(whiteData);

            Color[] blackData = { Color.SteelBlue };
            blackSquare = new Texture2D(GraphicsDevice, 1, 1);
            blackSquare.SetData<Color>(blackData);

            font = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        KeyboardState lastState = Keyboard.GetState();

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState curState = Keyboard.GetState();

            if (lastState.IsKeyDown(Keys.Space) && curState.IsKeyUp(Keys.Space))
            {
                currentPlayer.MakeMove(gs);
            }
            if (lastState.IsKeyDown(Keys.Left) && curState.IsKeyUp(Keys.Left))
                gs.MoveBackward();

            if (lastState.IsKeyDown(Keys.Right) && curState.IsKeyUp(Keys.Right))
                gs.MoveForward();

            lastState = curState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //if(!gs.CheckMate && !gs.StaleMate)
                //p.MakeMove(gs);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw Board
            Vector2 position;
            Rectangle dest;
            spriteBatch.Begin();
            for(int row = 0; row < 8; ++row)
            {
                for(int file = 0; file < 8; ++file)
                {
                    dest = new Rectangle(squareSize * file, squareSize * row, squareSize, squareSize);
                    if ((row % 2 == 0 && file % 2 == 0) ||
                        row % 2 == 1 && file % 2 == 1)
                    {
                        spriteBatch.Draw(whiteSquare, dest, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(blackSquare, dest, Color.White);
                    }

                    // Draw Piece
                    ChessPiece piece = gs.pieceGrid[file, row];
                    if(piece != null)
                    {
                        string pieceString = string.Empty;
                        if (piece is ChessPiecePawn)
                        {
                            pieceString = "P";
                        }
                        else if (piece is ChessPieceKnight)
                        {
                            pieceString = "N";
                        }
                        else if (piece is ChessPieceBishop)
                        {
                            pieceString = "B";
                        }
                        else if (piece is ChessPieceRook)
                        {
                            pieceString = "R";
                        }
                        else if (piece is ChessPieceQueen)
                        {
                            pieceString = "Q";
                        }
                        else if (piece is ChessPieceKing)
                        {
                            pieceString = "K";
                        }

                        Color color;
                        if(piece.color == ChessColor.Black)
                            color = Color.Black;
                        else
                            color = Color.White;

                        position = new Vector2(dest.Center.X, dest.Center.Y);
                        spriteBatch.DrawString(font, pieceString, position, color); 
                    }
                }
            }

            if (currentPlayer.MakingMove)
                spriteBatch.DrawString(font, "THINKING...", Vector2.Zero, Color.White);
                
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
