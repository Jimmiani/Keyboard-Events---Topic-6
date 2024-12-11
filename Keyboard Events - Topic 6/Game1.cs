using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Keyboard_Events___Topic_6
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Random generator;
        Texture2D pacRTexture, pacLTexture, pacUTexture, pacDTexture, pacSTexture, currentPacTexture, exitTexture, barrierTexture, coinTexture;
        Rectangle window, pacLocation, barrierRect1, barrierRect2, exitRect;
        List<Rectangle> coins;
        List<Rectangle> barriers;
        Vector2 pacSpeed;
        KeyboardState keyboardState, prevKeyboardState;
        MouseState mouseState;
        bool spedUp, slowedDown, isBig;
        int pacSize;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            window = new Rectangle(0, 0, 800, 480);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            coins = new List<Rectangle>();
            barriers = new List<Rectangle>();
            
            base.Initialize();


            pacSize = 60;
            pacLocation = new Rectangle(10, 10, pacSize, pacSize);
            currentPacTexture = pacSTexture;
            spedUp = false;
            slowedDown = true;
            isBig = false;
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));
            
            coins.Add(new Rectangle(250, 10, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 400, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 400, coinTexture.Width, coinTexture.Height));
            exitRect = new Rectangle(700, 380, 100, 100);
            generator = new Random();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Pac-Man
            pacRTexture = Content.Load<Texture2D>("PacRight");
            pacLTexture = Content.Load<Texture2D>("PacLeft");
            pacUTexture = Content.Load<Texture2D>("PacUp");
            pacDTexture = Content.Load<Texture2D>("PacDown");
            pacSTexture = Content.Load<Texture2D>("PacSleep");

            // Others
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            coinTexture = Content.Load<Texture2D>("coin");
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            mouseState = Mouse.GetState();
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            pacSpeed = new Vector2();
           

            // Speed and Textures
            if (pacSpeed.X == 0 && pacSpeed.Y == 0)
                currentPacTexture = pacSTexture;

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacSpeed.Y -= 2;
                currentPacTexture = pacUTexture;
            }
                
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacSpeed.Y += 2;
                currentPacTexture = pacDTexture;
            }
                
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacSpeed.X += 2;
                currentPacTexture = pacRTexture;
            }
                
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacSpeed.X -= 2;
                currentPacTexture = pacLTexture;
            }


            // Go through walls
            if (pacLocation.Bottom < 0)
                pacLocation = new Rectangle(pacLocation.X, window.Height, pacSize, pacSize);
            if (pacLocation.Top > window.Height)
                pacLocation = new Rectangle(pacLocation.X, -pacLocation.Height, pacSize, pacSize);
            if (pacLocation.Left > window.Width)
                pacLocation = new Rectangle(-pacLocation.Width, pacLocation.Y, pacSize, pacSize);
            if (pacLocation.Right < 0)
                pacLocation = new Rectangle(window.Width, pacLocation.Y, pacSize, pacSize);

            // Mouse teleport
            if (mouseState.LeftButton == ButtonState.Pressed)
                pacLocation = new Rectangle(mouseState.X - (pacSize / 2), mouseState.Y - (pacSize / 2), pacSize, pacSize);

            
            // Get faster
            if (keyboardState.IsKeyDown(Keys.OemPlus))
            {
                spedUp = true;
                slowedDown = false;
            }
            if (spedUp)
            {
                pacSpeed *= 8;
            }

            // Slow down
            if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                slowedDown = true;
                spedUp = false;
            }
            if (slowedDown)
            {
                pacSpeed *= 1;
            }


            // Big
            if ((keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Up) && !isBig)) 
            {
                pacLocation.Width *= 3;
                pacLocation.Height *= 3;
                pacSize *= 3;
                isBig = true;
            }
            

            // Random Teleport
            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
            {
                pacLocation = new Rectangle(generator.Next(window.Width - pacSize), generator.Next(window.Height - pacSize), pacSize, pacSize);
            }

            pacLocation.Offset(pacSpeed);


            // Topic 8

            for (int i = 0; i < coins.Count; i++)
            {
                if (pacLocation.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                }
            }

            if (exitRect.Contains(pacLocation))
            {
                Exit();
            }

            foreach (Rectangle barrier in barriers)
            {
                if (pacLocation.Intersects(barrier))
                    pacLocation.Offset(-pacSpeed);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(barrierTexture, barrierRect1, Color.White);
            _spriteBatch.Draw(barrierTexture, barrierRect2, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacLocation, Color.White);
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
