namespace PCG_DFFortressGenerator.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PCG_DFFortressGenerator.Classes.Rooms;
    using PCG_DFFortressGenerator.Classes.Workshops;

    /// <summary>
    /// A map used in the generator.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class with the given dimensions. 
        /// </summary>
        /// <param name="x">The height of the map.</param>
        /// <param name="y">The width of the map.</param>
        /// <param name="z">The depth of the map.</param>
        public Map(int x, int y, int z)
        {
            Random = new Random();
            this.MapLayers = new TileLayer[z];
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.CurrentZLevel = 0;

            // Initializing every layer of the map.
            for (var zz = 0; zz < z; zz++)
                this.MapLayers[zz] = new TileLayer(x, y, zz);
        }

        /// <summary>
        /// Gets the random object.
        /// </summary>
        public static Random Random { get; private set; }

        /// <summary>
        /// Gets the layers of the map.
        /// </summary>
        public TileLayer[] MapLayers { get; private set; }

        /// <summary>
        /// Gets the size of the map on the x-axis
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the size of the map on the y-axis
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Gets the size of the map on the z-axis
        /// </summary>
        public int Z { get; private set; }

        /// <summary>
        /// Gets or sets the z-level being shown currently (used by ToString).
        /// </summary>
        public int CurrentZLevel { get; set; }

        public double Fitness { get; set; }

        /// <summary>
        /// Changes the layers of this map.
        /// </summary>
        /// <param name="layers"> The layers to change to. </param>
        public void SetLayers(TileLayer[] layers)
        {
            MapLayers = layers;
        }

        /// <summary>
        /// Gets a list of all the areas in the map. To figure out which level an area is on,
        /// check the Z-position of one of the tiles of the area.
        /// </summary>
        /// <returns>A list of all areas in the map.</returns>
        public List<Area> GetAllAreas()
        {
            var tempList = new List<Area>();

            tempList.Add(this.MapLayers[this.Z - 1].Entrance);

            for (var z = this.Z - 1; z >= 0; z--)
                tempList.AddRange(this.MapLayers[z].LayerAreas);

            return tempList;
        }

        /// <summary>
        /// Gets a list of all the areas in the map. To figure out which level an area is on,
        /// check the Z-position of one of the tiles of the area.
        /// </summary>
        /// <returns>A list of all areas in the map.</returns>
        public List<Tuple<Tuple<int, int>, Area>> GetAllAreasWithIndicies()
        {
            var tempList = new List<Tuple<Tuple<int, int>, Area>>();

            for (var z = Z - 1; z >= 0; z--)
                tempList.AddRange(MapLayers[z].GetAreasWithIndicies());

            return tempList;
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

            var entrance = new Entrance();
            entrance.AddTile(MapLayers[4].MapTiles[9, 0]);
            entrance.AddTile(MapLayers[4].MapTiles[10, 0]);
            MapLayers[4].MapTiles[9, 0].TileStatus = Tile.TileType.Room;
            MapLayers[4].MapTiles[10, 0].TileStatus = Tile.TileType.Room;
            MapLayers[4].MapTiles[9, 0].AreaType = entrance;
            MapLayers[4].MapTiles[10, 0].AreaType = entrance;
            MapLayers[4].Entrance = entrance;

            // SetTile(9, 0, 4, Tile.TileType.Room, new Entrance());
            // SetTile(10, 0, 4, Tile.TileType.Room, new Entrance());
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
            MapLayers[4].GenerateAndAddArea(9, 4, 10, 4, new Bedroom());

            // SetTile(9, 4, 4, Tile.TileType.Room, new Bedroom());
            // SetTile(10, 4, 4, Tile.TileType.Room, new Bedroom());
            Console.WriteLine(@"----------------------------");
            Console.WriteLine(@"First level");
            Console.WriteLine(ToString());
            Console.WriteLine();

            // CurrentZLevel = 3;
            Console.WriteLine(@"Replaced room");
            MapLayers[4].ReplaceArea(MapLayers[4].LayerAreas[0], new Kitchen());
            Console.WriteLine(ToString());
            Console.WriteLine(@"----------------------------");
            Console.WriteLine(MapLayers[4].LayerAreas.OfType<Kitchen>().Any());
            Console.WriteLine(@"----------------------------");

            // Window.tbMapDisplay.Text = this.ToString();
            // Console.WriteLine(MeasureString(ToString()).Height + " of " + Window.tbMapDisplay.Height);
            // Console.WriteLine(MeasureString(ToString()).Width + " of " + Window.tbMapDisplay.Width);
        }

        /// <summary>
        /// Creates a deep copy of the map.
        /// </summary>
        /// <returns>A deep copy of the map.</returns>
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

        /// <summary>
        /// Calculates and updates the distances between all areas.
        /// </summary>
        public void CalculateDistancesBetweenAreas()
        {
            var areas = this.GetAllAreas();
            var numberOfAreas = this.GetAllAreas().Count;
            for (var j = 0; j < areas.Count; j++)
            {
                var area = areas[j];
                for (var i = 0; i < numberOfAreas; i++)
                {
                    var target = areas[i];
                    if (i == j)
                    {
                        area.Distances[i] = 0;
                    }
                    else if (!target.Distances.ContainsKey(j))
                    {
                        var tiles = area.AreaTiles;
                        var start = tiles[Random.Next(tiles.Count)];
                        var targetTiles = target.AreaTiles;
                        var dist = Pathfinding.DijkstraFindDistanceTo(this, start, targetTiles);
                        area.Distances[i] = dist;
                        target.Distances[j] = dist;
                    }
                    else
                    {
                        area.Distances[i] = target.Distances[j];
                    }
                }
            }
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
            this.MapLayers[z].SetTile(x, y, tileStatus, room);
        }
    }
}
