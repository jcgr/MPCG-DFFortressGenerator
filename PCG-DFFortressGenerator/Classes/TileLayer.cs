using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace PCG_DFFortressGenerator.Classes
{
    /// <summary>
    /// A class that represents one layer of the map.
    /// </summary>
    class TileLayer
    {
        /// <summary>
        /// The tiles that make up the layer.
        /// </summary>
        public Tile[,] MapTiles { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        /// <summary>
        /// The height at which the layer is in the map.
        /// </summary>
        public int ZLevel { get; private set; }

        public List<Area> LayerAreas { get; private set; }

        public TileLayer(int x, int y, int z)
        {
            MapTiles = new Tile[x, y];
            ZLevel = z;
            X = x;
            Y = x;
            LayerAreas = new List<Area>();
            
            for (var xx = 0; xx < x; xx++)
                for (var yy = 0; yy < y; yy++)
                    MapTiles[xx, yy] = new Tile(new Position(xx, yy, ZLevel));
        }

        /// <summary>
        /// Sets the given tile of this layer to have the chosen properties.
        /// </summary>
        /// <param name="x">The x-coordinate of the tile.</param>
        /// <param name="y">The y-coordinate of the tile.</param>
        /// <param name="tileStatus">The status of the tile.</param>
        /// <param name="room">The roomtype of the tile.</param>
        public void SetTile(int x, int y, Tile.TileType tileStatus, Area room)
        {
            MapTiles[x, y].TileStatus = tileStatus;
            MapTiles[x, y].AreaType = room;
        }

        public void AddArea(Area area)
        {
            LayerAreas.Add(area);
        }

        public TileLayer Copy()
        {
            // TODO: Copy tiles
            return null;
        }

        public override string ToString()
        {
            var map = new StringBuilder();

            for (var x = 0; x < MapTiles.GetLength(0); x++)
            {
                for (var y = 0; y < MapTiles.GetLength(1); y++)
                {
                    map.Append(MapTiles[x, y]);
                }
                map.AppendLine();
            }

            return map.ToString();
        }
    }
}
