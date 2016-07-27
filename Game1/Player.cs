using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public interface IMoveableObject
    {
        Vector2 Position
        {
            get; set;
        }

        Vector2 Velocity
        {
            get; set;
        }
    }

    public class Player : IMoveableObject 
    {
        public SpriteInfo SprInfo { get; set;  }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        private Game1 _Game;

        public Player(Game game)
        {
            _Game = (Game1)game;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.3)
            {
                SprInfo.Step();
            }
        }

        public void Jump()
        {
            if (Velocity.Y == 0)
            {
                Velocity = new Vector2(Velocity.X, -10);
            }
        }

        public void WalkLeft()
        {
            SprInfo.SetAction("walkleft");

            Velocity += new Vector2(-0.3f, 0);
        }

        public void WalkRight()
        {
            SprInfo.SetAction("walkright");

            Velocity += new Vector2(0.3f, 0);
        }
    }
}
