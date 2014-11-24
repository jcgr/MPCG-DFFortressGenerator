using System.Text;

namespace PCG_DFFortressGenerator.Classes
{
    class Map
    {
        // Representation of map
        public Tile[, ,] MapData { get; private set; }

        // The z-level being shown
        public int CurrentZLevel { get; private set; }

        public Map(int x, int y, int z)
        {
            MapData = new Tile[x, y, z];
            CurrentZLevel = 0;

            // Initializing every tile with a position.
            for (var xx = 0; xx < x; xx++)
                for (var yy = 0; yy < y; yy++)
                    for (var zz = 0; zz < z; zz++)
                        MapData[xx, yy, zz] = new Tile(new Position(xx, yy, zz));
        }

        public override string ToString()
        {
            var map = new StringBuilder();

            for (var x = 0; x < MapData.GetLength(0); x++)
            {
                for (var y = 0; y < MapData.GetLength(1); y++)
                {
                    map.Append(MapData[x, y, CurrentZLevel].ToString());
                }
                map.AppendLine();
            }

            return map.ToString();
        }
    }

    /// <summary>
    /// A class that represents the position of something.
    /// </summary>
    class Position
    {
        /// <summary>
        /// The x-coordinate of the position
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The y-coordinate of the position
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The z-coordinate of the position
        /// </summary>
        public int Z { get; private set; }

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
