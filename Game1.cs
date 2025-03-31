using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        SpriteFont instructionFont;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenState = ScreenState.TitleScreen;
            // TODO: Add your initialization logic here
            instructionFont = Content.Load<SpriteFont>("Fonts/InstructionFont");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (screenState == ScreenState.TitleScreen)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue); //Should switch to a background....
            }


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
