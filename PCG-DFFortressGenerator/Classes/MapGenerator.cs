namespace PCG_DFFortressGenerator.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PCG_DFFortressGenerator.Classes.Rooms;

    /// <summary>
    /// Used to generate map layouts for Dwarf Fortress.
    /// </summary>
    public class MapGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapGenerator"/> class.
        /// </summary>
        /// <param name="map"> The map. </param>
        /// <param name="chosenAreas"> The chosen areas. </param>
        /// <param name="numberOfDwarves"> The number of dwarves. </param>
        public MapGenerator(Map map, List<Area> chosenAreas, int numberOfDwarves)
        {
            this.Map = map;
            this.Random = new Random();
            this.ChosenAreas = chosenAreas;
            this.NumberOfDwarves = numberOfDwarves;
            this.CurrentNumberOfRooms = this.CalculateNumberOfRooms();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGenerator"/> class.
        /// </summary>
        /// <param name="x"> The x-size. </param>
        /// <param name="y"> The y-size. </param>
        /// <param name="z"> The z-size. </param>
        /// <param name="requiredNumberOfRooms"> The required number of rooms for the map. </param>
        public MapGenerator(int x, int y, int z, int requiredNumberOfRooms)
        {
            this.Map = new Map(x, y, z);
            this.Random = new Random();
            this.RequiredNumberOfRooms = requiredNumberOfRooms;
            this.CurrentNumberOfRooms = this.RequiredNumberOfRooms;
        }

        /// <summary>
        /// Gets the map that has been generated.
        /// </summary>
        public Map Map { get; private set; }

        /// <summary>
        /// Gets or sets the number of dwarves the fortress should contain.
        /// </summary>
        private int NumberOfDwarves { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms that have to be generated.
        /// </summary>
        private int CurrentNumberOfRooms { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms that have to be generated.
        /// </summary>
        private int RequiredNumberOfRooms { get; set; }

        /// <summary>
        /// Gets or sets the random generator.
        /// </summary>
        private Random Random { get; set; }

        /// <summary>
        /// Gets or sets the chosen areas for the fortress.
        /// </summary>
        private List<Area> ChosenAreas { get; set; }

        /// <summary>
        /// Generates a laytout for the map that belongs to the map generator.
        /// </summary>
        public void GenerateMap()
        {
            GenerateEntrance(Map.MapLayers[Map.Z - 1]);
            for (var z = Map.Z - 1; z >= 0; z--)
            {
                GenerateRooms(Map, z);
                if (this.CurrentNumberOfRooms <= 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Generates a new map layout
        /// </summary>
        /// <returns> The newly generated <see cref="Map"/>. </returns>
        public Map GenerateNewMap()
        {
            this.Map = new Map(this.Map.X, this.Map.Y, this.Map.Z);
            this.CurrentNumberOfRooms = RequiredNumberOfRooms;

            GenerateEntrance(Map.MapLayers[Map.Z - 1]);
            for (var z = Map.Z - 1; z >= 0; z--)
            {
                if (!GenerateRooms(Map, z))
                {
                    break;
                }

                if (this.CurrentNumberOfRooms <= 0)
                {
                    break;
                }
            }

            return this.Map;
        }

        /// <summary>
        /// Checks if the tiles between the given positions are "open" (not dug or part of a wall) on the given layer.
        /// </summary>
        /// <param name="startX">The start x-coordinate.</param>
        /// <param name="startY">The start y-coordinate.</param>
        /// <param name="endX">The end x-coordinate.</param>
        /// <param name="endY">The end y-coordinate.</param>
        /// <param name="tileLayer">The layer</param>
        /// <returns>True if all tiles are open; false otherwise.</returns>
        private static bool TilesOpenBetween(int startX, int startY, int endX, int endY, TileLayer tileLayer)
        {
            var differenceX = endX - startX;
            var differenceY = endY - startY;
            var changeX = differenceX < 0 ? -1 : 1;
            var changeY = differenceY < 0 ? -1 : 1;

            for (var x = 0; Math.Abs(x) <= Math.Abs(differenceX); x += changeX)
                for (var y = 0; Math.Abs(y) <= Math.Abs(differenceY); y += changeY)
                    if (tileLayer.MapTiles[x + startX, y + startY].TileStatus != Tile.TileType.NotDug
                        && tileLayer.MapTiles[x + startX, y + startY].TileStatus != Tile.TileType.RoomWall)
                        return false;

            return true;
        }

        /// <summary>
        /// Calculates the number of rooms required.
        /// </summary>
        /// <returns>The number of rooms required.</returns>
        private int CalculateNumberOfRooms()
        {
            var rooms = 0;

            if (ChosenAreas.OfType<Bedroom>().Any())
            {
                rooms += (int)Math.Ceiling(NumberOfDwarves / 8d);
            }

            if (ChosenAreas.OfType<DiningRoom>().Any())
            {
                rooms += (int)Math.Ceiling(NumberOfDwarves / 6d);
            }

            rooms += NumberOfDwarves / 2;

            // Console.WriteLine(NumberOfDwarves + " rooms: " + rooms);
            return rooms;
        }

        /// <summary>
        /// Generates an entrance to the fortress.
        /// </summary>
        /// <param name="layer"> The layer to generate on. </param>
        private void GenerateEntrance(TileLayer layer)
        {
            var tile1 = Random.Next(layer.X - 2) + 1;
            var tile2 = -1;

            if (tile1 == 1)
                tile2 = 2;
            if (tile1 == layer.X - 2)
                tile2 = layer.X - 3;

            if (tile2 == -1)
            {
                if (Random.Next(10) % 2 == 0)
                    tile2 = tile1 + 1;
                else
                    tile2 = tile1 - 1;
            }

            // var entrance = layer.GenerateAndAddArea(tile2, 0, tile1, 0, new Entrance());
            var entrance = new Entrance();
            entrance.AddTile(layer.MapTiles[tile2, 0]);
            entrance.AddTile(layer.MapTiles[tile1, 0]);
            layer.MapTiles[tile2, 0].TileStatus = Tile.TileType.Room;
            layer.MapTiles[tile1, 0].TileStatus = Tile.TileType.Room;
            layer.MapTiles[tile2, 0].AreaType = entrance;
            layer.MapTiles[tile1, 0].AreaType = entrance;
            layer.Entrance = entrance;
        }

        /// <summary>
        /// Generates rooms in the fortress.
        /// </summary>
        /// <param name="map"> The map to generate on. </param>
        /// <param name="layer"> The layer to generate on. </param>
        /// <returns>True if generation finished successfully, fales if not.</returns>
        private bool GenerateRooms(Map map, int layer)
        {
            var done = false;
            var openPositions = new List<Position>();
            var tileLayer = map.MapLayers[layer];

            foreach (var mapTile in tileLayer.MapTiles)
                if (mapTile.TileStatus == Tile.TileType.NotDug)
                    openPositions.Add(mapTile.Position);

            while (!done)
            {
                GenerateRoomAndPath(tileLayer, openPositions);
                done = IsLayerFinished(tileLayer, openPositions);
            }

            // Console.WriteLine("After layer " + layer + " there are " + CurrentNumberOfRooms + " remaining");
            if (this.CurrentNumberOfRooms <= 0)
                return true;

            return GenerateStairsToNextLevel(map, layer, openPositions);
        }

        /// <summary>
        /// Generates a room on the given layer along with a path to the entrance on the layer.
        /// </summary>
        /// <param name="tileLayer">The layer to generate a room for.</param>
        /// <param name="openPositions">The positions on the layer that have not yet been tried.</param>
        private void GenerateRoomAndPath(TileLayer tileLayer, List<Position> openPositions)
        {
            var generated = false;
            var originalLayout = tileLayer.Copy();

            while (!generated)
            {
                // Grab random start position
                var startPosition = openPositions[Random.Next(openPositions.Count)];
                var possibleCombinations = new List<Tuple<int, int>>
                {
                    new Tuple<int, int>(-1, 1),
                    new Tuple<int, int>(-1, -1),
                    new Tuple<int, int>(1, 1),
                    new Tuple<int, int>(1, -1)
                };

                for (int i = 0; i < 4; i++)
                {
                    // Choose random combination (up/down and left/right)
                    var combinationIndex = Random.Next(possibleCombinations.Count);
                    var combination = possibleCombinations[combinationIndex];
                    possibleCombinations.RemoveAt(combinationIndex);

                    // Create positions to check for being open.
                    var checkX = startPosition.X + (combination.Item1 * 5);
                    var checkY = startPosition.Y + (combination.Item2 * 5);

                    if (!tileLayer.WithinLayer(checkX, checkY)) continue;
                    if (!TilesOpenBetween(startPosition.X, startPosition.Y, checkX, checkY, tileLayer)) continue;

                    // Create area
                    var tempArea = tileLayer.GenerateAndAddArea(
                        startPosition.X + combination.Item1,
                        startPosition.Y + combination.Item2,
                        checkX - combination.Item1,
                        checkY - combination.Item2,
                        new Area());

                    var wallTiles = new List<Tile>();

                    // Build walls around room
                    for (int xx = 0; Math.Abs(xx) <= Math.Abs(combination.Item1 * 5); xx++)
                    {
                        tileLayer.SetTile(startPosition.X + (combination.Item1 * xx), startPosition.Y, Tile.TileType.RoomWall, null);
                        tileLayer.SetTile(startPosition.X + (combination.Item1 * xx), startPosition.Y + (combination.Item2 * 5), Tile.TileType.RoomWall, null);
                                
                        if (xx == 0 || Math.Abs(xx) == Math.Abs(combination.Item1 * 5)) continue;
                        wallTiles.Add(tileLayer.MapTiles[startPosition.X + (combination.Item1 * xx), startPosition.Y]);
                        wallTiles.Add(tileLayer.MapTiles[startPosition.X + (combination.Item1 * xx), startPosition.Y + (combination.Item2 * 5)]);
                    }

                    for (int yy = 0; Math.Abs(yy) <= Math.Abs(combination.Item2 * 5); yy++)
                    {
                        tileLayer.SetTile(startPosition.X, startPosition.Y + (combination.Item2 * yy), Tile.TileType.RoomWall, null);
                        tileLayer.SetTile(startPosition.X + (combination.Item1 * 5), startPosition.Y + (combination.Item2 * yy), Tile.TileType.RoomWall, null);

                        if (yy == 0 || Math.Abs(yy) == Math.Abs(combination.Item1 * 5)) continue;
                        wallTiles.Add(tileLayer.MapTiles[startPosition.X, startPosition.Y + (combination.Item2 * yy)]);
                        wallTiles.Add(tileLayer.MapTiles[startPosition.X + (combination.Item1 * 5), startPosition.Y + (combination.Item2 * yy)]);
                    }

                    // Attempt to generate path
                    var pathGenerated = false;
                    while (!pathGenerated)
                    {
                        // If a path cannot be generated, set it back to the original layout.
                        if (wallTiles.Count <= 0)
                        {
                            tileLayer = originalLayout;
                            return;
                        }

                        var randomWallTileIndex = Random.Next(wallTiles.Count);
                        var chosenWallTile = wallTiles[randomWallTileIndex];
                        wallTiles.RemoveAt(randomWallTileIndex);

                        if (tileLayer.Entrance == null)
                            Console.WriteLine("Much error, such wow, many paths " + this.CurrentNumberOfRooms);
                        var path = Pathfinding.DijkstraFindPathToOpenArea(
                            Map,
                            chosenWallTile,
                            tileLayer.Entrance.AreaTiles[Random.Next(tileLayer.Entrance.AreaTiles.Count)]);

                        if (path == null) continue;
                                
                        foreach (var pathTile in path)
                        {
                            if (pathTile.TileStatus == Tile.TileType.Stairs) continue;
                            if (pathTile.TileStatus == Tile.TileType.Room) continue;
                            tileLayer.SetTile(pathTile.Position.X, pathTile.Position.Y, Tile.TileType.Dug, null);
                            pathGenerated = true;
                        }
                    }

                    foreach (var areaTile in tempArea.AreaTiles)
                        openPositions.Remove(areaTile.Position);

                    this.CurrentNumberOfRooms--;
                    generated = true;

                    break;
                }

                if (openPositions.Contains(startPosition))
                    openPositions.Remove(startPosition);

                if (possibleCombinations.Count <= 0)
                    generated = true;
            }
        }

        /// <summary>
        /// Generates stairs on the chosen layer that leads to the next layer.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="layer">The layer to generate stairs on (will make stairs to layer - 1).</param>
        /// <param name="openPositions">The positions not yet checked.</param>
        private bool GenerateStairsToNextLevel(Map map, int layer, List<Position> openPositions)
        {
            var tileLayer = map.MapLayers[layer];
            var originalLayout = tileLayer.Copy();
            var generated = false;
            openPositions =
                openPositions.Where(x => tileLayer.MapTiles[x.X, x.Y].TileStatus == Tile.TileType.NotDug).ToList();

            while (!generated)
            {
                // Grab random start position
                var startPosition = openPositions[Random.Next(openPositions.Count)];
                var possibleCombinations = new List<Tuple<int, int>>
                {
                    new Tuple<int, int>(-1, 0),
                    new Tuple<int, int>(0, -1),
                    new Tuple<int, int>(0, 1),
                    new Tuple<int, int>(1, 0)
                };

                for (int i = 0; i < 4; i++)
                {
                    // Choose random combination (up/down and left/right)
                    var combinationIndex = Random.Next(possibleCombinations.Count);
                    var combination = possibleCombinations[combinationIndex];
                    possibleCombinations.RemoveAt(combinationIndex);

                    // Create positions to check for being open.
                    var checkX = startPosition.X + combination.Item1;
                    var checkY = startPosition.Y + combination.Item2;

                    if (!tileLayer.WithinLayer(checkX, checkY)) continue;
                    if (tileLayer.MapTiles[checkX, checkY].TileStatus != Tile.TileType.NotDug) continue;

                    var possibleTiles = new List<Tile>
                    {
                        tileLayer.MapTiles[startPosition.X, startPosition.Y],
                        tileLayer.MapTiles[checkX, checkY]
                    };

                    // Attempt to generate path
                    var pathGenerated = false;
                    while (!pathGenerated)
                    {
                        // If a path cannot be generated, set it back to the original layout.
                        if (possibleTiles.Count <= 0)
                        {
                            tileLayer = originalLayout;
                            return false;
                        }

                        var randomWallTileIndex = Random.Next(possibleTiles.Count);
                        var chosenWallTile = possibleTiles[randomWallTileIndex];
                        possibleTiles.RemoveAt(randomWallTileIndex);

                        var path = Pathfinding.DijkstraFindPathToOpenArea(
                            Map,
                            chosenWallTile,
                            tileLayer.Entrance.AreaTiles[Random.Next(tileLayer.Entrance.AreaTiles.Count)]);

                        if (path == null) continue;

                        foreach (var pathTile in path)
                        {
                            if (pathTile.TileStatus == Tile.TileType.Stairs) continue;
                            if (pathTile.TileStatus == Tile.TileType.Room) continue;
                            tileLayer.SetTile(pathTile.Position.X, pathTile.Position.Y, Tile.TileType.Dug, null);
                            pathGenerated = true;
                        }
                    }

                    // Change tiles to be stairs
                    tileLayer.MapTiles[startPosition.X, startPosition.Y].TileStatus = Tile.TileType.Stairs;
                    tileLayer.MapTiles[checkX, checkY].TileStatus = Tile.TileType.Stairs;

                    // Create matching stairs on the next layer
                    if (layer > 0)
                    {
                        if (map.MapLayers[layer - 1] != null)
                        {
                            var newEntrance = new Area();
                            newEntrance.AddTile(map.MapLayers[layer - 1].MapTiles[startPosition.X, startPosition.Y]);
                            newEntrance.AddTile(map.MapLayers[layer - 1].MapTiles[checkX, checkY]);
                            map.MapLayers[layer - 1].MapTiles[startPosition.X, startPosition.Y].TileStatus = Tile.TileType.Stairs;
                            map.MapLayers[layer - 1].MapTiles[checkX, checkY].TileStatus = Tile.TileType.Stairs;
                            map.MapLayers[layer - 1].Entrance = newEntrance;
                        }
                    }

                    generated = true;

                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the layer is finished with regards to generating (no more rooms needed or only 10%
        /// of the layer is open).
        /// </summary>
        /// <param name="tileLayer">The layer to check for.</param>
        /// <param name="openPositions">The open positions on the layer.</param>
        /// <returns>True if the layer is finished; false otherwise.</returns>
        private bool IsLayerFinished(TileLayer tileLayer, List<Position> openPositions)
        {
            if (this.CurrentNumberOfRooms <= 0)
                return true;
            if (openPositions.Count <= (tileLayer.X * tileLayer.Y) * 0.1)
                return true;

            return false;
        }
    }
}
