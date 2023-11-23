using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace coollisions
{
    public class Game1 : Game
    {
        //we are creating some variables: the texture, position, and speed of the ball
        Texture2D ballTexture;
        Vector2 ballPosition;
        Texture2D squareTexture;
        List<Vector2> squarePosition;
        float ballSpeed;
        float gravideeA;
        float gravideeV;


        //Both of these variables that are used for drawing to the screen, as you will see in a later tutorial.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //The main game constructor is used to initialize the starting variables. In this case, a new GraphicsDeviceManager is created, and the root directory containing the game's content files is set.
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        //The Initialize method is called after the constructor but before the main game loop (Update/Draw). This is where you can query any required services and load any non-graphic related content.
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //initializing ballPosition using the resolution of the screen
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, 0);
            squarePosition = new() { new(_graphics.PreferredBackBufferWidth, 0) };
            //initializing ball speed
            ballSpeed = 300f;
            gravideeA = 0.2f;

            base.Initialize();
        }

        //The LoadContent method is used to load your game content. It is called only once per game, within the Initialize method, before the main game loop starts.
        protected override void LoadContent()
        {
            //creates a new SpriteBatch, which can be used to draw textures (???????????????????)
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
        }

        //The Update method is called multiple times per second, and it is used to update your game state (checking for collisions, gathering input, playing audio, etc.).
        protected override void Update(GameTime gameTime)
        {
            //setting exit keys
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); //Content.Unload();

            // TODO: Add your update logic here
            //getting data from the keys and setting the ball position accordingly (moving the ball)
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up))
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Down))
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Left))
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Right))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //gravidee
            ballPosition.Y += gravideeV;// * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((ballPosition.Y + (ballTexture.Height / 2)) < _graphics.PreferredBackBufferHeight)
                gravideeV += gravideeA;
            else gravideeV = 0;

            //setting the boundaries on the ball to the size of the window
            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            else if (ballPosition.X < ballTexture.Width / 2)
                ballPosition.X = ballTexture.Width / 2;
            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            else if (ballPosition.Y < ballTexture.Height / 2)
                ballPosition.Y = ballTexture.Height / 2;

            //Debug.WriteLine($"{gravideeV}, {ballPosition.Y + (ballTexture.Height / 2)}, {_graphics.PreferredBackBufferHeight}");
            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    ballPosition.X = Mouse.GetState().X;
            //    ballPosition.Y = Mouse.GetState().Y;
            //    gravideeV = 0;
            //}

            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) &&
                (Convert.ToDouble(Mouse.GetState().X) >= Math.Floor(squarePosition[0].X - 80)) && (Convert.ToDouble(Mouse.GetState().X) <= Math.Ceiling(squarePosition[0].X)) &&
                (Convert.ToDouble(Mouse.GetState().Y) >= Math.Floor(squarePosition[0].Y)) && (Convert.ToDouble(Mouse.GetState().Y) <= Math.Ceiling(squarePosition[0].Y + 80)))
                squarePosition.Add(new(100, 200));

            base.Update(gameTime);
        }

        //Similar to the Update method, the Draw method is also called multiple times per second. This, as the name suggests, is responsible for drawing content to the screen.
        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            //drawing the spriteBatch (the ball)
            _spriteBatch.Draw(
                ballTexture, //texture
                ballPosition, //position
                null, //sourcerectangle
                Color.White, //color
                0f, //rotation
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), //origin
                Vector2.One, //scale
                SpriteEffects.None, //effects
                0f //layerdepth
            );

            //square
            Texture2D rect = new(_graphics.GraphicsDevice, 80, 80);
            Color[] data = new Color[80 * 80];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            rect.SetData(data);
            squareTexture = rect;
            foreach (Vector2 pos in squarePosition)
                _spriteBatch.Draw(squareTexture, pos, null, Color.Black, 0f, new(squareTexture.Width, 0), Vector2.One, SpriteEffects.None, 0f);

            //ends the drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
