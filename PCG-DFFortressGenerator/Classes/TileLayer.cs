using System.Text;

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

        /// <summary>
        /// The height at which the layer is in the map.
        /// </summary>
        public int Height { get; private set; }

        public TileLayer(int x, int y, int z)
        {
            MapTiles = new Tile[x, y];
            Height = z;
            
            for (var xx = 0; xx < x; xx++)
                for (var yy = 0; yy < y; yy++)
                    MapTiles[xx, yy] = new Tile(new Position(xx, yy, Height));
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
