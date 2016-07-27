using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Camera 
    {
        public Vector2 Pos;

        public int W
        {
            get; private set;
        }

        public int H
        {
            get; private set;
        }

        public Camera(Vector2 position, int w, int h)
        {
            Pos = position;
            W = w;
            H = h;
        }

        public int ToViewportX(int x)
        {
            return ToViewportX((float)x);
        }

        public int ToViewportX(float x)
        {
            return (int)(x - Pos.X); 
        }

        public int ToViewportY(int y)
        {
            return ToViewportY((float)y);
        }

        public int ToViewportY(float y)
        {
            return (int)(y - Pos.Y);
        }

        public void Pan(Vector2 vect)
        {
        }
    }
}
