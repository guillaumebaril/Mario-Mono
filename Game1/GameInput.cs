using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class GameInput
    {
        private GamePadState _GamePadState;
        private KeyboardState _KeyboardState;
        private GamePadState _PrevGamePadState;
        private KeyboardState _PrevKeyboardState;

        private Vector2 _SelValue;

        public GameInput()
        {

        }

        public void Update(GameTime gameTime)
        {
            _PrevGamePadState = _GamePadState;
            _PrevKeyboardState = _KeyboardState;

            _GamePadState = GamePad.GetState(PlayerIndex.One);
            _KeyboardState = Keyboard.GetState();

            if (EditMode_OnSelMove())
            {
                _SelValue.X += _GamePadState.ThumbSticks.Right.X * (float)(320 * gameTime.ElapsedGameTime.TotalSeconds);
                _SelValue.Y -= _GamePadState.ThumbSticks.Right.Y * (float)(320 * gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public float WalkValue { get; private set; }
        public float JumpValue { get; private set; }

        public bool OnWalkLeft()
        {
            if (_GamePadState.ThumbSticks.Left.X < 0)
            {
                WalkValue = Math.Abs(_GamePadState.ThumbSticks.Left.X);
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.Left))
            {
                WalkValue = 1.0f;
                return true;
            }

            return false;
        }

        public bool OnWalkRight()
        {
            if (_GamePadState.ThumbSticks.Left.X > 0)
            {
                WalkValue = Math.Abs(_GamePadState.ThumbSticks.Left.X);
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.Right))
            {
                WalkValue = 1.0f;
                return true;
            }

            return false;
        }

        public bool OnJump()
        {
            if (_GamePadState.Buttons.A == ButtonState.Pressed && _PrevGamePadState.Buttons.A == ButtonState.Released)
            {
                JumpValue = 1.0f;
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.Up) && _PrevKeyboardState.IsKeyUp(Keys.Up))
            {
                JumpValue = 1.0f;
                return true;
            }

            return false;
        }

        public bool OnCrouch()
        {
            return false;
        }

        public bool EditMode_OnToggle()
        {
            if (_GamePadState.DPad.Up == ButtonState.Pressed && _PrevGamePadState.DPad.Up == ButtonState.Released)
            {
                _SelValue = new Vector2(200, 200);
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.G) && _PrevKeyboardState.IsKeyUp(Keys.G))
            {
                _SelValue = new Vector2(200, 200);
                return true;
            }

            return false;
        }

        public Vector2 SelValue { get { return _SelValue; } }

        public bool EditMode_OnSelMove()
        {
            if (_GamePadState.ThumbSticks.Right.X != 0 || _GamePadState.ThumbSticks.Right.Y != 0)
            {
                return true;
            }

            return false;
        }

        public bool EditMode_OnPrevTile()
        {
            if (_GamePadState.Buttons.LeftShoulder == ButtonState.Pressed && _PrevGamePadState.Buttons.LeftShoulder == ButtonState.Released)
            {
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.Q) && _PrevKeyboardState.IsKeyUp(Keys.Q))
            {
                return true;
            }

            return false;
        }

        public bool EditMode_OnNextTile()
        {
            if (_GamePadState.Buttons.RightShoulder == ButtonState.Pressed && _PrevGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.W) && _PrevKeyboardState.IsKeyUp(Keys.W))
            {
                return true;
            }

            return false;
        }

        public bool OnQuitGame()
        {
            if (_GamePadState.Buttons.Back == ButtonState.Pressed && _PrevGamePadState.Buttons.Back == ButtonState.Released)
            {
                return true;
            }

            if (_KeyboardState.IsKeyDown(Keys.Escape) && _PrevKeyboardState.IsKeyUp(Keys.Escape))
            {
                return true;
            }

            return false;
        }
    }
}
