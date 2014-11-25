using System;
using System.Text;
using PCG_DFFortressGenerator.Classes.Rooms;

namespace PCG_DFFortressGenerator.Classes
{
    class Map
    {
        /// <summary>
        /// The map in tiles
        /// </summary>
        public Tile[, ,] MapData { get; private set; }

        /// <summary>
        /// The z-level being shown currently (used by ToString)
        /// </summary>
        public int CurrentZLevel { get; private set; }

        /// <summary>
        /// Initializes a map with the given dimensions.
        /// </summary>
        /// <param name="x">The height of the map.</param>
        /// <param name="y">The width of the map.</param>
        /// <param name="z">The depth of the map.</param>
        public Map(int x, int y, int z)
        {
            MapData = new Tile[x, y, z];
            CurrentZLevel = 0;

            // Initializing every tile with a position.
            for (var xx = 0; xx < x; xx++)
                for (var yy = 0; yy < y; yy++)
                    for (var zz = 0; zz < z; zz++)
                        MapData[xx, yy, zz] = new Tile(new Position(xx, yy, zz));

            TestMap();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var map = new StringBuilder();

            for (var x = 0; x < MapData.GetLength(0); x++)
            {
                for (var y = 0; y < MapData.GetLength(1); y++)
                {
                    map.Append(MapData[x, y, CurrentZLevel]);
                }
                map.AppendLine();
            }

            return map.ToString();
        }

        /// <summary>
        /// Creates a test map and does some basic pathfinding on it.
        /// </summary>
        private void TestMap()
        {
            MapData = new Tile[20, 20, 5];
            CurrentZLevel = 4;

            // Initializing every tile with a position.
            for (var xx = 0; xx < 20; xx++)
                for (var yy = 0; yy < 20; yy++)
                    for (var zz = 0; zz < 5; zz++)
                        MapData[xx, yy, zz] = new Tile(new Position(xx, yy, zz));

            SetTile(9, 0, 4, Tile.TileType.Room, new Entrance());
            SetTile(10, 0, 4, Tile.TileType.Room, new Entrance());
            for (var y = 1; y < 11; y++)
            {
                SetTile(9, y, 4, Tile.TileType.Dug, null);
                SetTile(10, y, 4, Tile.TileType.Dug, null);
            }

            SetTile(9, 11, 4, Tile.TileType.StairDown, null);
            SetTile(10, 11, 4, Tile.TileType.StairDown, null);

            SetTile(9, 11, 3, Tile.TileType.StairUp, null);
            SetTile(10, 11, 3, Tile.TileType.StairUp, null);
            for (var y = 5; y < 11; y++)
            {
                SetTile(9, y, 3, Tile.TileType.Dug, null);
                SetTile(10, y, 3, Tile.TileType.Dug, null);
            }
            SetTile(9, 4, 3, Tile.TileType.Room, new Bedroom());
            SetTile(10, 4, 3, Tile.TileType.Room, new Bedroom());

            Console.WriteLine(@"----------------------------");
            Console.WriteLine(@"First level");
            Console.WriteLine(ToString());
            Console.WriteLine();
            CurrentZLevel = 3;
            Console.WriteLine(@"Second level");
            Console.WriteLine(ToString());
            Console.WriteLine(@"----------------------------");
            Pathfinding.DijsktraFindDistanceTo(this, MapData[9, 0, 4], MapData[10, 4, 3]);
        }

        /// <summary>
        /// Sets a tile to the given type and room, if applicable.
        /// </summary>
        /// <param name="x">The x-coordinate of the tile.</param>
        /// <param name="y">The y-coordinate of the tile.</param>
        /// <param name="z">The z-coordinate of the tile.</param>
        /// <param name="tileStatus">The status of the tile.</param>
        /// <param name="room">The roomtype of the tile.</param>
        private void SetTile(int x, int y, int z, Tile.TileType tileStatus, Area room)
        {
            MapData[x, y, z].TileStatus = tileStatus;
            MapData[x, y, z].AreaType = room;
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
