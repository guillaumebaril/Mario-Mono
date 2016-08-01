using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class KeyboardInput
    {
        private KeyboardState _Current;
        private KeyboardState _Previous;

        public KeyboardInput()
        {
        }

        public void Update(GameTime gameTime)
        {
            _Previous = _Current;
            _Current = Keyboard.GetState();
            
        }

        public bool IsKeyPressed(Keys key)
        {
            return _Previous[key] == KeyState.Up && _Current[key] == KeyState.Down;
        }

        public bool IsKeyDown(Keys key)
        {
            return _Current[key] == KeyState.Down;
        }

        public TimeSpan KeyDownDuration(Keys key)
        {
            return new TimeSpan();
        }


    }
}
