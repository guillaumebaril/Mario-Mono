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
    public enum CollisionSide
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public class CollisionInfo
    {
        public int TX { get; set; }
        public int TY { get; set; }
        public TileData TileData { get; set; }
    }

    public interface IEntity
    {
        Vector2 Position { get; set; }
    }

    public interface IMoveableObject : IEntity 
    {
        Vector2 Velocity { get; set; }
        Rectangle CollisionBox { get; set; }

        CollisionInfo CollisionBottom { get; set; }
        CollisionInfo CollisionTop { get; set; }
        CollisionInfo CollisionLeft { get; set; }
        CollisionInfo CollisionRight { get; set; }
    }

    public class Player : IMoveableObject 
    {
        public SpriteInfo SprInfo { get; set;  }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle CollisionBox { get; set; }

        public CollisionInfo CollisionBottom { get; set; }
        public CollisionInfo CollisionTop { get; set; }
        public CollisionInfo CollisionLeft { get; set; }
        public CollisionInfo CollisionRight { get; set; }

        public Player(SpriteInfo sprInfo)
        {
            SprInfo = sprInfo;
            CollisionBox = new Rectangle(13, 4, 13, 52);
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.3)
            {
                SprInfo.Step();
            }
        }

        public void Jump(float value = 1f)
        {
            if (Velocity.Y == 0 && CollisionBottom != null)
            {
                Velocity = new Vector2(Velocity.X, -10 * value);
            }
        }

        public void WalkLeft(float value)
        {
            SprInfo.SetAction("walkleft");
            
            Velocity += new Vector2(-0.3f * value, 0);
        }

        public void WalkRight(float value)
        {
            SprInfo.SetAction("walkright");

            Velocity += new Vector2(0.3f * value, 0);
        }
    }
}
