using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;

namespace Monogame_2___Assignment
{
    enum ScreenState
    {
        TitleScreen,
        MainScreen,
        EndScreen
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        ScreenState screenState;
        SpriteFont instructionFont, scoreFont;
        MouseState mouseState, previousMouseState;
        KeyboardState keyboardState;
        List<Texture2D> mushroomLoad, mushroomTextures, badgerTextures;
        List<Rectangle> badgerRects;
        Random generator;
        float seconds, respawnTime;
        int score, clicks;


        Rectangle window;
        Texture2D titleBackgroundTexture, fieldBackgroundTexture, badgerTexture, endBackgroundTexture;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenState = ScreenState.TitleScreen;
            generator = new Random();

            score = 0;
            clicks = 0;

            mushroomLoad = new List<Texture2D>();
            badgerRects = new List<Rectangle>();
            mushroomTextures = new List<Texture2D>();
            badgerTextures = new List<Texture2D>();

            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            for (int i = 0; i < 1; i++)
            {
                badgerRects.Add(new Rectangle(generator.Next(window.Width - 100), generator.Next(250, 350), 100, 100)); //Need to check for collision with other rectangles...
                
            }
            respawnTime = 2.5f;
            seconds = 0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            instructionFont = Content.Load<SpriteFont>("Fonts/InstructionFont");
            scoreFont = Content.Load<SpriteFont>("Fonts/ScoreFont");
            titleBackgroundTexture = Content.Load<Texture2D>("Images/greenBackground");
            fieldBackgroundTexture = Content.Load<Texture2D>("Images/fieldBackground");
            badgerTexture = Content.Load<Texture2D>("Images/badger");
            endBackgroundTexture = Content.Load<Texture2D>("Images/grayBackground");


            for (int i = 0; i < badgerRects.Count; i++)
            {
                badgerTextures.Add(badgerTexture);
            }

            for (int i = 1; i <= 8; i++)
                mushroomLoad.Add(Content.Load<Texture2D>("Images/mushroom" + i));

            for (int i = 0; i < badgerRects.Count; i++)
                mushroomTextures.Add(mushroomLoad[generator.Next(mushroomLoad.Count)]);

        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screenState == ScreenState.TitleScreen)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screenState = ScreenState.MainScreen;
                }

            }
            else if (screenState == ScreenState.MainScreen)  // Need ending condition...
            {
                if (seconds > respawnTime)
                {
                    mushroomTextures.Add(mushroomLoad[generator.Next(mushroomLoad.Count)]);
                    badgerTextures.Add(badgerTexture);
                    badgerRects.Add(new Rectangle(generator.Next(window.Width - 100), generator.Next(250, 350), 100, 100));
                    seconds = 0f; // Restarts timer
                }
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    //Mouse Check - Badger Rects
                    for (int i = 0; i < badgerRects.Count; i++)
                    {
                        if (badgerRects[i].Contains(mouseState.Position))
                        {
                            clicks += 1;

                            //Changes to mushroom
                            if (clicks % 2 != 0)
                            {
                                badgerTextures[i] = mushroomTextures[i];
                                score += 10;
                            }
                            //Removes
                            else if (clicks % 2 == 0)
                            {
                                score += 20;
                                badgerTextures.RemoveAt(i);
                                badgerRects.RemoveAt(i);
                                mushroomTextures.RemoveAt(i);
                                i--;

                            }

                        }

                    }
                }

                if (score >= 100)
                {
                    screenState = ScreenState.EndScreen;
                }
            }
            else if (screenState == ScreenState.EndScreen)
            {
                score = 0;


                
            }

            previousMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if (screenState == ScreenState.TitleScreen)
            {
                _spriteBatch.Draw(titleBackgroundTexture, window, Color.White);
                _spriteBatch.DrawString(instructionFont, "Need to put actual instructions \nhere... Testing...", new Vector2(100, 50), Color.DarkBlue);
                _spriteBatch.DrawString(instructionFont, "Press ENTER to continue", new Vector2(150, 400), Color.Brown);
                _spriteBatch.DrawString(scoreFont, "By Zoey Hamm", new Vector2(300, 450), Color.Indigo);
            }
            else if (screenState == ScreenState.MainScreen)
            {
                _spriteBatch.Draw(fieldBackgroundTexture, window, Color.White);

                for (int i = 0; i < badgerRects.Count; i++)
                {
                    _spriteBatch.Draw(badgerTextures[i], badgerRects[i], Color.White); 
                }
                _spriteBatch.DrawString(scoreFont, $"Score: {score}", new Vector2(650, 10), Color.Crimson);

            }

            else if (screenState == ScreenState.EndScreen)
            {
                _spriteBatch.Draw(endBackgroundTexture, window, Color.White);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
