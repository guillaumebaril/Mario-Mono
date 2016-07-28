using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class TileData
    {
        public Point Source;
    }

    public class MapData
    {
        private TileData[,] _TileData = null;
        public int W { get; private set; }
        public int H { get; private set; }

        private MapData(int w, int h)
        {
            _TileData = new TileData[w, h];
            W = w;
            H = h;
        }

        public TileData this[int x, int y]
        {
            get { return _TileData[x, y]; }
            set { _TileData[x, y] = value; }
        }

        public static MapData FromFile(string filename)
        {
            return null;
        }

        public static MapData CreateRandomMapData(int w, int h)
        {
            var rand = new Random();

            var md = new MapData(w, h);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var pt = 
                        (x == 0 || y == 0 || x == w-1 || y == h-1 ? 
                        new Point(0, 0) :
                        new Point(0, 32));

                    if (x == 0 || y == 0 || x == w - 1 || y == h - 1)
                    {
                        md[x, y] = new TileData()
                        {
                            Source = pt
                        };
                    }
                }
            }

            return md;
        }
    }
}
