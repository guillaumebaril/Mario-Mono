using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class TileSet
    {
        public List<TileDef> Tiles { get; private set; }

        public TileSet()
        {
            Tiles = new List<TileDef>();
        }

        public static TileSet LoadFromFile(string file)
        {
            var tileset = new TileSet();

            tileset.Tiles.Add(new TileDef()
            {
                ID = 0,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(960, 288, 32, 32)
            });

            tileset.Tiles.Add(new TileDef()
            {
                ID = 1,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(0, 0, 32, 32)
            });

            tileset.Tiles.Add(new TileDef()
            {
                ID = 2,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(32, 0, 32, 32)
            });

            tileset.Tiles.Add(new TileDef()
            {
                ID = 3,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(64, 0, 32, 32)
            });

            tileset.Tiles.Add(new TileDef()
            {
                ID = 4,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(96, 0, 32, 32)
            });
            tileset.Tiles.Add(new TileDef()
            {
                ID = 5,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(128, 0, 32, 32)
            });
            tileset.Tiles.Add(new TileDef()
            {
                ID = 6,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(160, 0, 32, 32)
            });
            tileset.Tiles.Add(new TileDef()
            {
                ID = 7,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(192, 0, 32, 32)
            });
            tileset.Tiles.Add(new TileDef()
            {
                ID = 8,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(224, 0, 32, 32)
            });

            tileset.Tiles.Add(new TileDef()
            {
                ID = 9,
                ColFlags = new bool[4] { true, true, true, true },
                Src = new Rectangle(256, 0, 32, 32)
            });

            return tileset;
        }
    }

    public struct TileDef
    {
        public int ID;
        public bool[] ColFlags;
        public Rectangle Src;
    }

    public class TileData
    {
        public Point Source;
    }
}
