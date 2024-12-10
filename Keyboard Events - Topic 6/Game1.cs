using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Keyboard_Events___Topic_6
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D pacRTexture, pacLTexture, pacUTexture, pacDTexture, pacSTexture, currentPacTexture;
        Rectangle window, pacLocation;
        Vector2 pacSpeed;
        KeyboardState keyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            window = new Rectangle(0, 0, 800, 600);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();


            pacLocation = new Rectangle(10, 10, 75, 75);
            currentPacTexture = pacSTexture;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            pacRTexture = Content.Load<Texture2D>("PacRight");
            pacLTexture = Content.Load<Texture2D>("PacLeft");
            pacUTexture = Content.Load<Texture2D>("PacUp");
            pacDTexture = Content.Load<Texture2D>("PacDown");
            pacSTexture = Content.Load<Texture2D>("PacSleep");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();
            pacSpeed = new Vector2();

            pacSpeed = Vector2.Zero;

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

            if (pacLocation.Bottom < 0)
                pacLocation = new Rectangle(pacLocation.X, (window.Height + pacLocation.Height), 75, 75);
            if (pacLocation.Top > window.Height)
                pacLocation = new Rectangle(pacLocation.X, -pacLocation.Height, 75, 75);
            if (pacLocation.Left > window.Width)
                pacLocation = new Rectangle(-pacLocation.Width, pacLocation.Y, 75, 75);
            if (pacLocation.Right < 0)
                pacLocation = new Rectangle((window.Width + pacLocation.Width), pacLocation.Y, 75, 75);



            pacLocation.Offset(pacSpeed);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            _spriteBatch.Draw(currentPacTexture, pacLocation, Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
