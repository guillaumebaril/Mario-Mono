using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Game1
{
    public class TimedMessage
    {
        public string Text { get; set; }
        public TimeSpan Duration { get; set; }
        public Vector2 Position { get; set; }
        public bool StaticPosition { get; set; }
        public DateTime? StartAt { get; set; }
        public Color Color { get; set; }

        public TimedMessage()
        {
            StartAt = null;
            StaticPosition = true;
            Color = Color.Black;
        }
    }

    public class TimedMessagesCollection : IEnumerable<TimedMessage>
    {
        private LinkedList<TimedMessage> _Texts = null;

        public TimedMessagesCollection()
        {
            _Texts = new LinkedList<TimedMessage>();
        }

        public IEnumerator<TimedMessage> GetEnumerator()
        {
            return _Texts.GetEnumerator();
        }

        public void Show(TimedMessage timedText)
        {
            if (timedText.StartAt == null)
            {
                timedText.StartAt = DateTime.Now;
            }

            _Texts.AddLast(timedText);
        }

        public void Update(GameTime gameTime)
        {
            var elem = _Texts.First;
            while (elem != null)
            {
                var next = elem.Next;
                if (DateTime.Now >= elem.Value.StartAt + elem.Value.Duration)
                {
                    _Texts.Remove(elem);
                }
                elem = next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
