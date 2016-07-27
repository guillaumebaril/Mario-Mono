using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class MouseSelector
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private Camera _Camera = null;
        
        public MouseSelector(Game game, Camera camera)
        {
            _Camera = camera;
        }

        public void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();

            X = (int)((ms.X + _Camera.Pos.X) / 32) * 32;
            Y = (int)((ms.Y + _Camera.Pos.Y) / 32) * 32;
        }
    }
}
