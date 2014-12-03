using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PCG_DFFortressGenerator.Classes.Rooms;

namespace PCG_DFFortressGenerator.Classes
{
    class Map
    {
        /// <summary>
        /// The layers of the map.
        /// </summary>
        public TileLayer[] MapLayers { get; private set; }

        /// <summary>
        /// The size of the map on the x-axis
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The size of the map on the y-axis
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The size of the map on the z-axis
        /// </summary>
        public int Z { get; private set; }

        /// <summary>
        /// The z-level being shown currently (used by ToString)
        /// </summary>
        public int CurrentZLevel { get; set; }

        /// <summary>
        /// Initializes a map with the given dimensions.
        /// </summary>
        /// <param name="x">The height of the map.</param>
        /// <param name="y">The width of the map.</param>
        /// <param name="z">The depth of the map.</param>
        public Map(int x, int y, int z)
        {
            MapLayers = new TileLayer[z];
            X = x;
            Y = y;
            Z = z;
            CurrentZLevel = 0;
//            WhitespaceX = 257.6 / 20d;
//            WhitespaceY = 120.933333333333 / 20d;

            // Initializing every layer of the map.
            for (var zz = 0; zz < z; zz++)
                MapLayers[zz] = new TileLayer(x, y, zz);

//            TestMap();
        }

        public void SetLayers(TileLayer[] layers)
        {
            MapLayers = layers;
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
            return MapLayers[CurrentZLevel].ToString();
        }

        /// <summary>
        /// Creates a test map and does some basic pathfinding on it.
        /// </summary>
        public void TestMap()
        {
            MapLayers = new TileLayer[5];
            Z = 5;
            CurrentZLevel = 4;

            for (var zz = 0; zz < 5; zz++)
                MapLayers[zz] = new TileLayer(20, 20, zz);

            SetTile(9, 0, 4, Tile.TileType.Room, new Entrance());
            SetTile(10, 0, 4, Tile.TileType.Room, new Entrance());
            /*for (var y = 1; y < 11; y++)
            {
                SetTile(9, y, 4, Tile.TileType.Dug, null);
                SetTile(10, y, 4, Tile.TileType.Dug, null);
            }*/

            SetTile(9, 11, 4, Tile.TileType.Stairs, null);
            SetTile(10, 11, 4, Tile.TileType.Stairs, null);

            SetTile(9, 11, 3, Tile.TileType.Stairs, null);
            SetTile(10, 11, 3, Tile.TileType.Stairs, null);
            /*for (var y = 5; y < 11; y++)
            {
                SetTile(9, y, 3, Tile.TileType.Dug, null);
                SetTile(10, y, 3, Tile.TileType.Dug, null);
            }*/
            SetTile(9, 4, 4, Tile.TileType.Room, new Bedroom());
            SetTile(10, 4, 4, Tile.TileType.Room, new Bedroom());

            Console.WriteLine(@"----------------------------");
            Console.WriteLine(@"First level");
            Console.WriteLine(ToString());
            Console.WriteLine();
            CurrentZLevel = 3;
            Console.WriteLine(@"Second level");
            Console.WriteLine(ToString());
            Console.WriteLine(@"----------------------------");
            Console.WriteLine(@"----------------------------");

            //Window.tbMapDisplay.Text = this.ToString();
            //Console.WriteLine(MeasureString(ToString()).Height + " of " + Window.tbMapDisplay.Height);
            //Console.WriteLine(MeasureString(ToString()).Width + " of " + Window.tbMapDisplay.Width);
        }

//        public Size MeasureString(string candidate)
//        {
//            var formattedText = new FormattedText(
//                candidate,
//                CultureInfo.CurrentUICulture,
//                FlowDirection.LeftToRight,
//                new Typeface(Window.tbMapDisplay.FontFamily, Window.tbMapDisplay.FontStyle, Window.tbMapDisplay.FontWeight, Window.tbMapDisplay.FontStretch),
//                Window.tbMapDisplay.FontSize,
//                Brushes.Black);
//
//            return new Size(formattedText.Width, formattedText.Height);
//        }

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
            MapLayers[z].SetTile(x, y, tileStatus, room);
        }

        public Map Copy()
        {
            var newMap = new Map(X, Y, Z);
            var newLayers = new TileLayer[Z];

            for (var z = 0; z < Z; z++)
            {
                newLayers[z] = MapLayers[z].Copy();
            }

            newMap.SetLayers(newLayers);
            return newMap;
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

        public Position Copy()
        {
            return new Position(X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position) obj);
        }

        protected bool Equals(Position other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }
    }
}
