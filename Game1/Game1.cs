using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Texture2D _Mario = null;
        MapData _Map = null;
        Player _Player = null;
        SpriteFont _Font = null;
        List<Player> players = new List<Player>();
        Random _Rand = new Random();
        GameInput _Input = null;

        TimedMessagesCollection _TimedMessages = null;

        int _TileSel = 0;

        TileSet _Tileset = null;
        bool _EditMode = false;

        Stopwatch _Sw = null;
        int _FC = 0;
        int _FPS = 0;

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
            _Map = MapData.CreateRandomMapData(123, 20);

            _Camera = new Camera(new Vector2(0, 0), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _CamBounds = new Rectangle((int)(_Camera.W / 3f), (int)(_Camera.H / 3f), (int)(_Camera.W / 3f), (int)(_Camera.H / 3f));
            _MouseSel = new MouseSelector(this, _Camera);
            _Sw = new Stopwatch();
            _Sw.Start();
            //TargetElapsedTime = new TimeSpan(0, 0, 0, 0, (int)Math.Floor(1000 / 120f));
            _Input = new GameInput();

            _TimedMessages = new TimedMessagesCollection();
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

            _Mario = Content.Load<Texture2D>("mario");

            var SprInfo = new SpriteInfo(_Mario);

            SprInfo.Add("walkleft", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
            SprInfo.Add("walkright", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.None);

            SprInfo.Add("runleft", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
            SprInfo.Add("runright", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.None);

            _Player = new Player(SprInfo);
            _Player.Position = new Vector2(300, 200);

            _Tileset = TileSet.LoadFromFile("todo.txt");

            IsMouseVisible = false;

            for (int i = 0; i < 0; i++)
            {
                SprInfo = new SpriteInfo(_Mario);

                SprInfo.Add("walkleft", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
                SprInfo.Add("walkright", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.None);

                SprInfo.Add("runleft", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
                SprInfo.Add("runright", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.None);

                players.Add(new Player(SprInfo)
                {
                    Position = new Vector2(_Rand.Next(1000) + 64, _Rand.Next(400) + 64)
                });
            }
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

            _MouseSel.Update(gameTime);

            _TimedMessages.Update(gameTime);
            _Input.Update(gameTime);

            if (_Input.OnQuitGame())
            {
                Exit();
            }

            if (_Input.EditMode_OnToggle())
            {
                _EditMode = !_EditMode;

                _TimedMessages.Show(new TimedMessage()
                {
                    Duration = new TimeSpan(0, 0, 0, 0, 1100),
                    Position = new Vector2(_Camera.W / 2, _Camera.H / 2),
                    Text =  (_EditMode ? "Entering edit mode" : "Quitting edit mode")
                });
            }

            if (_EditMode)
            {
                if (_Input.EditMode_OnPrevTile())
                {
                    _TileSel --;
                    if (_TileSel < 0)
                    {
                        _TileSel = _Tileset.Tiles.Count - 1;
                    }
                }
                if (_Input.EditMode_OnNextTile())
                {
                    _TileSel = (_TileSel + 1) % _Tileset.Tiles.Count;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.ToggleFullScreen();
            }

            if (_Input.OnWalkLeft())
            {
                _Player.WalkLeft(_Input.WalkValue);
            }

            if (_Input.OnWalkRight())
            {
                _Player.WalkRight(_Input.WalkValue);
            }

            if (_Input.OnJump())
            {
                _Player.Jump(_Input.JumpValue);
            }

            var c = (float) ts.TotalSeconds * 300;

            if (_Player is IMoveableObject)
            {
                UpdateMoveableObject(gameTime, _Player, true);
            }

            _Player.Update(gameTime);

            foreach (var player in players)
            {
                if (_Rand.Next(3) == 1)
                {
                    if (player.Position.X < _Player.Position.X)
                    {
                        if (player.CollisionRight != null || _Rand.Next(50) == 1) player.Jump((float)(_Rand.NextDouble() * .75 + .25));
                        
                        player.WalkRight(1f);
                    }
                    else
                    {
                        if (player.CollisionLeft != null || _Rand.Next(50) == 1) player.Jump((float)(_Rand.NextDouble() * .75 + .25));
                        player.WalkLeft(1f);
                    }
                }
                
                UpdateMoveableObject(gameTime, player, true);
            }

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

            if (_EditMode)
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    var x = _MouseSel.X / 32;
                    var y = _MouseSel.Y / 32;

                    if (_TileSel == 0)
                    {
                        _Map[x, y] = null;
                    }
                    else
                    {
                        _Map[x, y] = new TileData()
                        {
                            Source = new Point(_Tileset.Tiles[_TileSel].Src.X, _Tileset.Tiles[_TileSel].Src.Y)
                        };
                    }
                }
            }

            if (ms.RightButton == ButtonState.Pressed)
            {
                var SprInfo = new SpriteInfo(_Mario );

                SprInfo.Add("walkleft", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
                SprInfo.Add("walkright", new List<Rectangle>() { new Rectangle(60, 53, 19, 28), new Rectangle(78, 53, 19, 28) }, SpriteEffects.None);

                SprInfo.Add("runleft", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.FlipHorizontally);
                SprInfo.Add("runright", new List<Rectangle>() { new Rectangle(160, 53, 19, 28), new Rectangle(182, 53, 19, 28) }, SpriteEffects.None);

                players.Add(new Player(SprInfo)
                {
                    Position = new Vector2(_MouseSel.X, _MouseSel.Y )
                });
            }
        }

        private void UpdateMoveableObject(GameTime gameTime, IMoveableObject obj, bool checkCollision)
        {
            obj.Position += obj.Velocity;

            obj.Velocity *= new Vector2(0.95f, 1);
            obj.Velocity += new Vector2(0, (float)(gameTime.ElapsedGameTime.TotalSeconds * 20));

            if (obj.Velocity.Y > 10)
            {
                obj.Velocity = new Vector2(obj.Velocity.X, 10);
            }
            if (checkCollision)
            {
                obj.CollisionBottom = CheckBottomCollision(obj);
                obj.CollisionTop = CheckTopCollision(obj);
                obj.CollisionLeft = CheckLeftCollision(obj);
                obj.CollisionRight = CheckRightCollision(obj);
            }
        }

        private CollisionInfo CheckBottomCollision(IMoveableObject obj)
        {
            if (obj.Velocity.Y > 0)
            {
                int ty = (int)((obj.Position.Y + obj.CollisionBox.Bottom) / 32);
                int tx = (int)((obj.Position.X + obj.CollisionBox.X) / 32);

                var collisionTile = _Map[tx, ty];

                if (collisionTile == null)
                {
                    tx = (int)((obj.Position.X + obj.CollisionBox.Right) / 32);
                    collisionTile = _Map[tx, ty];
                }

                if (collisionTile != null)
                {
                    obj.Position = new Vector2(obj.Position.X, ty * 32 - obj.CollisionBox.Bottom);
                    obj.Velocity = new Vector2(obj.Velocity.X, 0);

                    return new CollisionInfo()
                    {
                        TileData = collisionTile, 
                        TX = tx,
                        TY = ty
                    };
                }
            }

            return null;
        }

        private CollisionInfo CheckTopCollision(IMoveableObject obj)
        {
            if (obj.Velocity.Y < 0)
            {
                int ty = (int)((obj.Position.Y + obj.CollisionBox.Y) / 32);
                int tx = (int)((obj.Position.X + obj.CollisionBox.X) / 32);

                var collisionTile = _Map[tx, ty];
                if (collisionTile == null)
                {
                    tx = (int)((obj.Position.X + obj.CollisionBox.Right) / 32);
                    collisionTile = _Map[tx, ty];
                }

                if (collisionTile != null)
                {
                    obj.Position = new Vector2(obj.Position.X, (ty + 1) * 32 - obj.CollisionBox.Y);
                    obj.Velocity = new Vector2(obj.Velocity.X, 0);

                    return new CollisionInfo()
                    {
                        TileData = collisionTile,
                        TX = tx,
                        TY = ty
                    };
                }
            }

            return null;
        }

        private CollisionInfo CheckLeftCollision(IMoveableObject obj)
        {
            if (obj.Velocity.X < 0)
            {
                const int offset = 8;

                int tx = (int)((obj.Position.X + obj.CollisionBox.X - offset) / 32);
                int ty = (int)((obj.Position.Y + obj.CollisionBox.Bottom - 3) / 32);

                var collisionTile = _Map[tx, ty];
                if (collisionTile == null)
                {
                    ty = (int)((obj.Position.Y + obj.CollisionBox.Y + 2) / 32);
                    collisionTile = _Map[tx, ty];
                }

                if (collisionTile == null)
                {
                    ty = (int)((obj.Position.Y + obj.CollisionBox.Y + obj.CollisionBox.Height / 2.0) / 32);
                    collisionTile = _Map[tx, ty];
                }

                if (collisionTile != null)
                {
                    obj.Position = new Vector2((tx + 1) * 32 - obj.CollisionBox.X + offset, obj.Position.Y);
                    obj.Velocity = new Vector2(0, obj.Velocity.Y);

                    return new CollisionInfo()
                    {
                        TileData = collisionTile,
                        TX = tx,
                        TY = ty
                    };
                }
            }

            return null;
        }

        private CollisionInfo CheckRightCollision(IMoveableObject obj)
        {
            if (obj.Velocity.X > 0)
            {
                const int offset = 8;

                int tx = (int)((obj.Position.X + obj.CollisionBox.Right + offset) / 32);
                int ty = (int)((obj.Position.Y + obj.CollisionBox.Bottom - 3) / 32);

                var collisionTile = _Map[tx, ty];
                if (collisionTile == null)
                {
                    ty = (int)((obj.Position.Y + obj.CollisionBox.Y + 2) / 32);
                    collisionTile = _Map[tx, ty];
                }

                if (collisionTile == null)
                {
                    ty = (int)((obj.Position.Y + obj.CollisionBox.Y + obj.CollisionBox.Height / 2.0) / 32);
                    collisionTile = _Map[tx, ty];
                }

                if (collisionTile != null)
                {
                    obj.Position = new Vector2(tx * 32 - obj.CollisionBox.Right - offset, obj.Position.Y);
                    obj.Velocity = new Vector2(0, obj.Velocity.Y);

                    return new CollisionInfo()
                    {
                        TileData = collisionTile,
                        TX = tx,
                        TY = ty
                    };
                }
            }

            return null;
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

            if (_EditMode)
            {
                RenderMouseSelector();
            }

            foreach (var player in players)
            {
                RenderPlayer(player);
            }

            RenderPlayer(_Player);

            if (_EditMode)
            {
                var t = _Tileset.Tiles[_TileSel];

                spriteBatch.Draw(_Tiles, new Rectangle(32, 32, 32, 32), t.Src, Color.White);
            }

            RenderTimedMessages();

            DrawFpsCounter(gameTime);

            spriteBatch.End();
        }

        private void RenderTimedMessages()
        {
            foreach (var message in _TimedMessages)
            {
                var pos = message.Position;

                if (!message.StaticPosition)
                {
                    pos = message.Position - _Camera.Pos;
                }

                spriteBatch.DrawString(_Font, message.Text, pos, message.Color);
            }
        }

        private void RenderMouseSelector()
        {
            var x = (int)(( _Camera.Pos.X + _Input.SelValue.X) / 32) * 32;
            var y = (int)((_Camera.Pos.Y + _Input.SelValue.Y) / 32) * 32;

            spriteBatch.Draw(_Empty, new Rectangle(_Camera.ToViewportX(x), _Camera.ToViewportY(y), 32, 32), Color.White);
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
                        if (_EditMode)
                        {
                            spriteBatch.Draw(_Empty, new Rectangle(vpx, vpy, 31, 31), CreateBackgroundColor(x, y));
                        }
                        else
                        {
                            spriteBatch.Draw(_Empty, new Rectangle(vpx, vpy, 32, 32), CreateBackgroundColor(x, y));
                        }
                    }
                }
            }
        }

        private void DrawFpsCounter(GameTime gameTime)
        {
            _FC++;
            if (_Sw.Elapsed.TotalSeconds >= 1.0)
            {
                _FPS = _FC;
                _FC = 0;
                _Sw.Restart();
            }

            var fps = (Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds))  + " " + _FPS.ToString();

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
