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

        ChessGameProxy chessGame;

        const int BOARD_DIMENSION = 800;
        const int SQUARE_SIZE = 100;

        ChessBoardSquare[,] uiBoard;

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            chessGame = new ChessGameProxy();
            chessGame.Updated += new ChessGameUpdate(chessGame_Updated);
            InitializeUIBoard();
            chessGame.StartGame(PlayerType.Computer, PlayerType.Computer);

            base.Initialize();
        }

        void chessGame_Updated(ChessPieceInfo?[,] pieceInfo)
        {
            updateUIBoard(pieceInfo);
        }

        private void InitializeUIBoard()
        {
            uiBoard = new ChessBoardSquare[8, 8];

            for (int file = 0; file < 8; file++)
            {
                for (int row = 0; row < 8; row++)
                {
                    ChessBoardSquare square = new ChessBoardSquare(this);
                    uiBoard[file, row] = square;
                    Components.Add(square);
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadUIBoard();
        }

        private void LoadUIBoard()
        {
            graphics.PreferredBackBufferHeight = BOARD_DIMENSION;
            graphics.PreferredBackBufferWidth = BOARD_DIMENSION;
            graphics.ApplyChanges();

            // Create square textures
            Color[] whiteData = { Color.Silver };
            Texture2D whiteSquare = new Texture2D(GraphicsDevice, 1, 1);
            whiteSquare.SetData<Color>(whiteData);

            Color[] blackData = { Color.SteelBlue };
            Texture2D blackSquare = new Texture2D(GraphicsDevice, 1, 1);
            blackSquare.SetData<Color>(blackData);

            font = Content.Load<SpriteFont>("font");

            for (int row = 0; row < 8; ++row)
            {
                for (int file = 0; file < 8; ++file)
                {
                    ChessBoardSquare square = uiBoard[file, row];
                    square.MySpriteBatch = spriteBatch;
                    square.Font = font;
                    square.Destination = new Rectangle(SQUARE_SIZE * file, SQUARE_SIZE * row, SQUARE_SIZE, SQUARE_SIZE);
                    if ((row % 2 == 0 && file % 2 == 0) ||
                        row % 2 == 1 && file % 2 == 1)
                        square.BackgroundTexture = whiteSquare;
                    else
                        square.BackgroundTexture = blackSquare;
                }
            }
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
                chessGame.MakeMove();

            if (lastState.IsKeyDown(Keys.Left) && curState.IsKeyUp(Keys.Left))
                chessGame.MoveBackward();

            if (lastState.IsKeyDown(Keys.Right) && curState.IsKeyUp(Keys.Right))
                chessGame.MoveForward();

            lastState = curState;

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the pieces displayed on the uiBoard
        /// </summary>
        private void updateUIBoard(ChessPieceInfo?[,] pieceGrid)
        {
            for (int row = 0; row < 8; ++row)
            {
                for (int file = 0; file < 8; ++file)
                {
                    // Update squares
                    ChessPieceInfo? piece = pieceGrid[file, row];
                    ChessBoardSquare square = uiBoard[file, row];

                    if (!piece.HasValue)
                        square.ChessPiece = string.Empty;
                    else 
                    {
                        ChessPieceInfo i = piece.Value;
                        if (i.Type == ChessPieceType.Pawn)
                            square.ChessPiece = "P";
                        else if (i.Type == ChessPieceType.Knight)
                            square.ChessPiece = "N";
                        else if (i.Type == ChessPieceType.Bishop)
                            square.ChessPiece = "B";
                        else if (i.Type == ChessPieceType.Rook)
                            square.ChessPiece = "R";
                        else if (i.Type == ChessPieceType.Queen)
                            square.ChessPiece = "Q";
                        else if (i.Type == ChessPieceType.King)
                            square.ChessPiece = "K";

                        if (i.Color == ChessColor.Black)
                            square.PieceColor = Color.Black;
                        else
                            square.PieceColor = Color.White;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            spriteBatch.Begin();

            //if (game.MakingMove)
            //    spriteBatch.DrawString(font, "THINKING...", Vector2.Zero, Color.White);
                
            spriteBatch.End();
        }
    }
}
