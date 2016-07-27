using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        Texture2D _Tiles = null;
        Texture2D _Empty = null;
        MapData _Map = null;
        Player _Player = null;
        SpriteFont _Font = null;
        List<Player> players = new List<Player>();

        Rectangle _CamBounds;

        Camera _Camera = null;

        MouseSelector _MouseSel = null;

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
            _Map = MapData.CreateRandomMapData(10000, 30);

            _Camera = new Camera(new Vector2(0, 0), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _CamBounds = new Rectangle(100, 100, _Camera.W - 200 , _Camera.H - 200);
            _MouseSel = new MouseSelector(this, _Camera);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _Font = Content.Load<SpriteFont>("Arial8");
            _Tiles = Content.Load<Texture2D>("tiles1");
            _Empty = Content.Load<Texture2D>("empty");

            var SprInfo = new SpriteInfo(Content.Load<Texture2D>("mario"));

            SprInfo.Add("walkleft", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
            SprInfo.Add("walkright", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.None);

            SprInfo.Add("runleft", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
            SprInfo.Add("runright", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.None);

            _Player = new Player(this);
            _Player.SprInfo = SprInfo;
            _Player.Position = new Vector2(300, 200);

            IsMouseVisible = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var ts = gameTime.ElapsedGameTime;
            var kbs = Keyboard.GetState();
            _MouseSel.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbs.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (kbs.IsKeyDown(Keys.Left))
            {
                _Player.WalkLeft();
            }

            if (kbs.IsKeyDown(Keys.Right))
            {
                _Player.WalkRight();
            }

            if (kbs.IsKeyDown(Keys.Space))
            {
                _Player.Jump();
            }

            var c = (float) ts.TotalSeconds * 300;

            if (_Player is IMoveableObject)
            {
                UpdateMoveableObject(gameTime, _Player);
            }

            _Player.Update(gameTime);

            int pvpx = _Camera.ToViewportX(_Player.Position.X);
            int pvpy = _Camera.ToViewportY(_Player.Position.Y);

            if (pvpx < _CamBounds.X)
            {
                _Camera.Pos.X = (float)Math.Floor(Math.Max(_Camera.Pos.X - (_CamBounds.X - pvpx), 0));
            }

            if (pvpx > _CamBounds.Right)
            {
                _Camera.Pos.X = (float)Math.Floor(_Camera.Pos.X + (pvpx - _CamBounds.Right));
            }

            if (pvpy < _CamBounds.Y)
            {
                _Camera.Pos.Y = Math.Max(_Camera.Pos.Y - (_CamBounds.Y - pvpy), 0);
            }

            if (pvpy > _CamBounds.Bottom)
            {
                _Camera.Pos.Y = _Camera.Pos.Y + (pvpy - _CamBounds.Bottom);
            }

            var ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                var x = _MouseSel.X / 32;
                var y = _MouseSel.Y / 32;

                _Map[x, y] = new TileData()
                {
                    Source = new Point(0, 0)
                };
            }
        }

        private void UpdateMoveableObject(GameTime gameTime, IMoveableObject obj)
        {
            obj.Position += obj.Velocity;

            obj.Velocity *= new Vector2(0.95f, 1);
            obj.Velocity += new Vector2(0, (float)(gameTime.ElapsedGameTime.TotalSeconds * 20));

            if (obj.Velocity.Y > 10) obj.Velocity = new Vector2(obj.Velocity.X, 10);

            var tx = (int)(obj.Position.X / 32);
            var ty = (int)((obj.Position.Y  + 56) / 32);

            if (_Map[tx, ty] != null)
            {
                obj.Position = new Vector2(obj.Position.X, ty * 32 - 56);
                obj.Velocity = new Vector2(obj.Velocity.X, 0);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            RenderMap();

            RenderMouseSelector();

            RenderPlayer(_Player);
            DrawFpsCounter(gameTime);

            spriteBatch.End();
        }

        private void RenderMouseSelector()
        {
            spriteBatch.Draw(_Empty, new Rectangle(_Camera.ToViewportX(_MouseSel.X), _Camera.ToViewportY(_MouseSel.Y), 32, 32), Color.White);
        }

        private void RenderMap()
        {
            int x1 = (int)Math.Floor(_Camera.Pos.X / 32);
            int y1 = (int)Math.Floor(_Camera.Pos.Y / 32);
            int x2 = Math.Min(x1 + 26, _Map.W);
            int y2 = Math.Min(y1 + 19, _Map.H);

            for (int x = x1; x < x2; x++)
            {
                for (int y = y1; y < y2; y++)
                {
                    int vpx = _Camera.ToViewportX(x * 32);
                    int vpy = _Camera.ToViewportY(y * 32);

                    if (_Map[x, y] != null)
                    {
                        var p = _Map[x, y].Source;

                        spriteBatch.Draw(_Tiles, new Rectangle(vpx, vpy, 32, 32), new Rectangle(p.X, p.Y, 32, 32), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_Empty, new Rectangle(vpx, vpy, 31, 31), CreateBackgroundColor(x, y));
                    }
                }
            }
        }

        private void DrawFpsCounter(GameTime gameTime)
        {
            var fps = (Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds));

            spriteBatch.DrawString(_Font, fps.ToString(), new Vector2(9, 9), Color.Black, 0, default(Vector2), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_Font, fps.ToString(), new Vector2(9, 11), Color.Black, 0, default(Vector2), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_Font, fps.ToString(), new Vector2(11, 9), Color.Black, 0, default(Vector2), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_Font, fps.ToString(), new Vector2(11, 11), Color.Black, 0, default(Vector2), 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_Font, fps.ToString(), new Vector2(10, 10), Color.White, 0, default(Vector2), 1, SpriteEffects.None, 0);
        }

        private static Color CreateBackgroundColor(int x, int y)
        {
            var r = (int)(Math.Abs(Math.Cos(x * 32 / 11)) * 150);
            var g = (int)(Math.Abs(Math.Sin(y * 32 / 11)) * 150 + 55);
            var color = new Color(r, g, 255);

            return color;
        }

        private void RenderPlayer(Player player)
        {
            var spr = player.SprInfo;

            var rect = spr.GetCurrentSrcRect();

            spriteBatch.Draw(spr.Texture, new Rectangle(_Camera.ToViewportX(player.Position.X), _Camera.ToViewportY(player.Position.Y), rect.Width * 2, rect.Height * 2), rect, Color.White, 0f, Vector2.Zero, spr.Effect, 0);
        }
    }
}
