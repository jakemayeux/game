using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Game2
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        private Map map;
        private bool asdf;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player(GraphicsDevice);
            map = new Map(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //base.LoadContent();
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            //rectangle = new Texture2D(GraphicsDevice, 1, 1);
            //rectangle.SetData(new[] { Color.Red });

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            var gamePadState = GamePad.GetState(PlayerIndex.One);

            //var leftStick = gamePadState.ThumbSticks.Left;

            var dt = gameTime.ElapsedGameTime.TotalMilliseconds;

            player.Update(gamePadState, dt);

            var collisions = map.Collide(player.getPos());

            player.Collide(collisions);

            //if (leftStick.X < 0) {
            //    pos.X -= (float)dt;
            //} else if (leftStick.X > 0) {
            //    pos.X += (float)dt;
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            player.Draw(spriteBatch);
            map.Draw(spriteBatch);

            // Option One (if you have integer size and coordinates)
            //spriteBatch.Draw(rectangle, new Rectangle((int)pos.X, 20, 30, 30), Color.White);

            // Option Two (if you have floating-point coordinates)
            //spriteBatch.Draw(whiteRectangle, new Vector2(10f, 20f), null,
            //        Color.Chocolate, 0f, Vector2.Zero, new Vector2(80f, 30f),
            //        SpriteEffects.None, 0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
