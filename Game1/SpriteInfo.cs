using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class SpriteInfo
    {
        private class Info
        {
            public List<Rectangle> Rects = null;
            public SpriteEffects Effect = SpriteEffects.None;

            public Info(List<Rectangle> rects, SpriteEffects effect)
            {
                Rects = rects;
                Effect = effect;
            }
        }

        public Texture2D Texture
        {
            get; private set;
        }

        private Dictionary<string, Info> _Info = null;
        private int _Step = 0;

        private List<Rectangle> _Rects = null;
        private string _Current = null;
        private Vector2 _Origin = new Vector2(0, 0);
        public SpriteEffects Effect
        {
            get; private set;
        }

        public SpriteInfo(Texture2D texture)
        {
            Texture = texture;
            _Info = new Dictionary<string, Info>();
            Effect = SpriteEffects.None;
        }

        public void Add(string name, List<Rectangle> info, SpriteEffects effect)
        {
            _Info.Add(name, new Info(info, effect));
            if (_Current == null)
            {
                SetAction(name);
            }
        }

        public Rectangle GetCurrentSrcRect()
        {
            return _Rects[_Step];
        }

        public void SetAction(string action)
        {
            _Rects = _Info[action].Rects;
            Effect = _Info[action].Effect;
            _Current = action;
        }

        public void Step()
        {
            _Step = ((_Step + 1) % _Rects.Count);
        }

        public void Render(SpriteBatch sb, Vector2 position)
        {
            var rect = _Rects[_Step];

            sb.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, rect.Width * 2, rect.Height * 2), rect, Color.White, 0f, _Origin, Effect, 0);
        }
    }
}
