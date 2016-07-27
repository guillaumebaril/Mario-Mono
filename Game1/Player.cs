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
    public class Player  
    {
        public Vector2 Pos = new Vector2(0, 0);
        public Vector2 Vel = new Vector2(0, 0);

        public SpriteInfo SprInfo { get; set;  }

        private Game1 _Game;

        public Player(Game game)
        {
            _Game = (Game1)game;
        }

        public void Update(GameTime gameTime)
        {
            Pos += Vel;
            Vel *= 0.95f;

            if (gameTime.ElapsedGameTime.TotalSeconds > 0.3)
            {
                SprInfo.Step();
            }
        }

        public void WalkLeft()
        {
            SprInfo.SetAction("walkleft");

            Vel += new Vector2(-0.3f, 0);
        }

        public void WalkRight()
        {
            SprInfo.SetAction("walkright");

            Vel += new Vector2(0.3f, 0);
        }

        public void WalkDown()
        {
            Vel += new Vector2(0, 0.3f);
        }

        public void WalkUp()
        {
            Vel += new Vector2(0, -0.3f);
        }
    }
}
