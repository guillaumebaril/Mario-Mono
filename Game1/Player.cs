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
    public interface IEntity
    {
        Vector2 Position { get; set; }
    }

    public interface IMoveableObject : IEntity 
    {
        Vector2 Velocity { get; set; }
        Rectangle CollisionBox { get; set; }
    }

    public class Player : IMoveableObject 
    {
        public SpriteInfo SprInfo { get; set;  }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle CollisionBox { get; set; }

        public Player(SpriteInfo sprInfo)
        {
            SprInfo = sprInfo;
            CollisionBox = new Rectangle(11, 2, 17, 54);
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

            //TODO: Remove debug stuff
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                Velocity += new Vector2(-0.01f, 0);
            else
                Velocity += new Vector2(-0.3f, 0);
        }

        public void WalkRight()
        {
            SprInfo.SetAction("walkright");

            //TODO: Remove debug stuff
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                Velocity += new Vector2(0.01f, 0);
            else
                Velocity += new Vector2(0.3f, 0);

        }
    }
}
