using System;
using System.Collections.Generic;

namespace PCG_DFFortressGenerator.Classes
{
    /// <summary>
    /// A class that represents a special area of the fortress (a room, a workshop or a stockpile).
    /// </summary>
    class Area
    {
        public int MinHeight { get; set; }

        public int MinWidth { get; set; }

        public int MaxHeight { get; set; }

        public int MaxWidth { get; set; }

//        public int MinSize { get; set; }
//
//        public int MaxSize { get; set; }

        public List<Tile> AreaTiles { get; private set; }

        // Make sure it is only one character due to the map being ASCII for now.
        public String AreaName { get; protected set; }

        public Area()
        {
            MinHeight = 4;
            MaxHeight = 4;
            MinWidth = 4;
            MaxWidth = 4;
//            MinSize = 0;
//            MaxSize = 10000;
            AreaName = "ø";

            AreaTiles = new List<Tile>();
        }

        public void AddTile(Tile tile)
        {
            AreaTiles.Add(tile);
        }

        public Area Copy()
        {
            // TODO: Copy self
            return null;
        }
    }
}
