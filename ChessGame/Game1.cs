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
            InitializeChessGame();
            InitializeUIBoard();

            base.Initialize();
        }

        private void InitializeChessGame()
        {
            gs = new ChessGameState();
            gs.SetNewGame();

            MiniMaxChessMoveGenerator miniMax = new MiniMaxChessMoveGenerator(4);

            currentPlayer = playerWhite = new ChessPlayer(ChessColor.White, miniMax, 0);
            playerWhite.MadeMove += new ChessMoveEventHandler(playerWhite_MadeMove);
            playerBlack = new ChessPlayer(ChessColor.Black, miniMax, 0);
            playerBlack.MadeMove += new ChessMoveEventHandler(playerBlack_MadeMove);
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

        void playerBlack_MadeMove(ChessPlayer player, ChessGameState state)
        {
            gs = state;
            currentPlayer = playerWhite;
            //currentPlayer.MakeMove(gs);
            updateUIBoard();
        }

        void playerWhite_MadeMove(ChessPlayer player, ChessGameState state)
        {
            gs = state;
            currentPlayer = playerBlack;
            //currentPlayer.MakeMove(gs);
            updateUIBoard();
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
            updateUIBoard();
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
        /// Updates the pieces displayed on the uiBoard
        /// </summary>
        private void updateUIBoard()
        {
            for (int row = 0; row < 8; ++row)
            {
                for (int file = 0; file < 8; ++file)
                {
                    // Update squares
                    ChessPiece piece = gs.pieceGrid[file, row];
                    ChessBoardSquare square = uiBoard[file, row];

                    if (piece == null)
                        square.ChessPiece = string.Empty;
                    else 
                    {
                        if (piece is ChessPiecePawn)
                            square.ChessPiece = "P";
                        else if (piece is ChessPieceKnight)
                            square.ChessPiece = "N";
                        else if (piece is ChessPieceBishop)
                            square.ChessPiece = "B";
                        else if (piece is ChessPieceRook)
                            square.ChessPiece = "R";
                        else if (piece is ChessPieceQueen)
                            square.ChessPiece = "Q";
                        else if (piece is ChessPieceKing)
                            square.ChessPiece = "K";

                        if (piece.color == ChessColor.Black)
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

            if (currentPlayer.MakingMove)
                spriteBatch.DrawString(font, "THINKING...", Vector2.Zero, Color.White);
                
            spriteBatch.End();
        }
    }
}
