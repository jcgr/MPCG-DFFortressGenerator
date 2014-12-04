using System;
using System.Collections.Generic;
using System.Linq;
using PCG_DFFortressGenerator.Classes.Rooms;

namespace PCG_DFFortressGenerator.Classes
{
    class LayoutGenerator
    {
        public Map Map { get; private set; }

        private int NumberOfDwarves { get; set; }

        private int NumberOfRooms { get; set; }

        private Random Random { get; set; }

        private List<Area> ChosenAreas { get; set; }

        public LayoutGenerator(Map map, List<Area> chosenAreas, int numberOfDwarves)
        {
            Map = map;
            Random = new Random();
            ChosenAreas = chosenAreas;
            NumberOfDwarves = numberOfDwarves;
            NumberOfRooms = CalculateNumberOfRooms();

            GenerateEntrance(Map.MapLayers[Map.Z - 1]);
//            GenerateRooms(Map.MapLayers[Map.Z - 1]);
            for (int z = Map.Z - 1; z >= 0; z--)
            {
                GenerateRooms(Map, z);
                if (NumberOfRooms <= 0)
                    break;
            }

        }

        /// <summary>
        /// Calculates the number of rooms required.
        /// </summary>
        /// <returns>The number of rooms required.</returns>
        private int CalculateNumberOfRooms()
        {
            var rooms = 0;

            if (ChosenAreas.OfType<Bedroom>().Any())
                rooms += (int)Math.Ceiling(NumberOfDwarves / 8d);
            if (ChosenAreas.OfType<DiningRoom>().Any())
                rooms += (int)Math.Ceiling(NumberOfDwarves / 6d);

            rooms += NumberOfDwarves / 2;
//            Console.WriteLine(NumberOfDwarves + " rooms: " + rooms);
            return rooms;
        }

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

//            var entrance = layer.GenerateAndAddArea(tile2, 0, tile1, 0, new Entrance());

            var entrance = new Entrance();
            entrance.AddTile(layer.MapTiles[tile2, 0]);
            entrance.AddTile(layer.MapTiles[tile1, 0]);
            layer.MapTiles[tile2, 0].TileStatus = Tile.TileType.Room;
            layer.MapTiles[tile1, 0].TileStatus = Tile.TileType.Room;
            layer.MapTiles[tile2, 0].AreaType = entrance;
            layer.MapTiles[tile1, 0].AreaType = entrance;
            layer.Entrance = entrance;
        }

        private void GenerateRooms(Map map, int layer)
        {
            var done = false;
            var openPositions = new List<Position>();
            var tileLayer = map.MapLayers[layer];

            foreach (var mapTile in tileLayer.MapTiles)
                if (mapTile.TileStatus == Tile.TileType.NotDug)
                    openPositions.Add(mapTile.Position);

            while (!done)
            {
                GenerateRoomAndPath(tileLayer, openPositions, layer);
                done = IsLayerFinished(tileLayer, openPositions);
            }

//            Console.WriteLine("After layer " + layer + " there are " + NumberOfRooms + " remaining");

            if (NumberOfRooms <= 0)
                return;

            GenerateStairsToNextLevel(map, layer, openPositions);
        }

        private void GenerateRoomAndPath(TileLayer tileLayer, List<Position> openPositions, int layer)
        {
            var generated = false;
            var tempArea = new Area();
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
                    if (!this.TilesOpenBetween(startPosition.X, startPosition.Y, checkX, checkY, tileLayer)) continue;

                    // Create area
                    tempArea = tileLayer.GenerateAndAddArea(
                        startPosition.X + combination.Item1
                        , startPosition.Y + combination.Item2
                        , checkX - combination.Item1
                        , checkY - combination.Item2
                        , new Area());

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
                            Console.WriteLine(NumberOfRooms + " - " + layer);
//                        Console.WriteLine(layer);
                        var path = Pathfinding.DijkstraFindPathTo(Map, chosenWallTile,
                            tileLayer.Entrance.AreaTiles[0]);
                                
                        if (path == null) continue;
                                
                        foreach (var pathTile in path)
                        {
                            if (pathTile.TileStatus == Tile.TileType.Stairs) continue;
                            if (pathTile.TileStatus == Tile.TileType.Room) continue;
                            tileLayer.SetTile(pathTile.Position.X, pathTile.Position.Y, Tile.TileType.Dug,
                                null);
                            pathGenerated = true;
                        }
                    }

                    foreach (var areaTile in tempArea.AreaTiles)
                        openPositions.Remove(areaTile.Position);

//                    Console.WriteLine("Room number: " + NumberOfRooms);

                    NumberOfRooms--;
                    generated = true;

                    break;
                }

                if (openPositions.Contains(startPosition))
                    openPositions.Remove(startPosition);

                if (possibleCombinations.Count <= 0)
                    generated = true;
            }
        }

        private void GenerateStairsToNextLevel(Map map, int layer, List<Position> openPositions)
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
                    var checkX = startPosition.X + (combination.Item1);
                    var checkY = startPosition.Y + (combination.Item2);

                    if (!tileLayer.WithinLayer(checkX, checkY)) continue;
//                    if (tileLayer.MapTiles[checkX, checkY].TileStatus == Tile.TileType.RoomWall) continue;
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
                            return;
                        }

                        var randomWallTileIndex = Random.Next(possibleTiles.Count);
                        var chosenWallTile = possibleTiles[randomWallTileIndex];
                        possibleTiles.RemoveAt(randomWallTileIndex);

                        var path = Pathfinding.DijkstraFindPathTo(Map, chosenWallTile,
                            tileLayer.Entrance.AreaTiles[0]);

                        if (path == null) continue;

                        foreach (var pathTile in path)
                        {
                            if (pathTile.TileStatus == Tile.TileType.Stairs) continue;
                            if (pathTile.TileStatus == Tile.TileType.Room) continue;
                            tileLayer.SetTile(pathTile.Position.X, pathTile.Position.Y, Tile.TileType.Dug,
                                null);
                            pathGenerated = true;
                        }
                    }

                    // Change tiles to be stairs
                    tileLayer.MapTiles[startPosition.X, startPosition.Y].TileStatus = Tile.TileType.Stairs;
                    tileLayer.MapTiles[checkX, checkY].TileStatus = Tile.TileType.Stairs;

                    // Do the same for the next layer
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
//                            Console.WriteLine("Generating stairs on level " + (layer - 1));
                        }
                    }

                    generated = true;

                    break;
                }
            }
        }

        /// <summary>
        /// Checks if the tiles between the given positions are "open" (not dug or part of a wall) on the given layer.
        /// </summary>
        /// <param name="startX">The start x-coordinate.</param>
        /// <param name="startY">The start y-coordinate.</param>
        /// <param name="endX">The start y-coordinate.</param>
        /// <param name="endY">The end y-coordinate.</param>
        /// <param name="tileLayer">The layer</param>
        /// <returns>True if all tiles are open; false otherwise.</returns>
        private bool TilesOpenBetween(int startX, int startY, int endX, int endY, TileLayer tileLayer)
        {
            var xDifference = endX - startX;
            var yDifference = endY - startY;
            var xChange = xDifference < 0 ? -1 : 1;
            var yChange = yDifference < 0 ? -1 : 1;

            for (var x = 0; Math.Abs(x) <= Math.Abs(xDifference); x += xChange)
                for (var y = 0; Math.Abs(y) <= Math.Abs(yDifference); y += yChange)
                    if (tileLayer.MapTiles[x + startX, y + startY].TileStatus != Tile.TileType.NotDug
                        && tileLayer.MapTiles[x + startX, y + startY].TileStatus != Tile.TileType.RoomWall)
                        return false;

            return true;
        }

        private bool IsLayerFinished(TileLayer tileLayer, List<Position> openPositions)
        {
            if (NumberOfRooms <= 0)
                return true;
            if (openPositions.Count <= (tileLayer.X * tileLayer.Y) * 0.1)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the given position is within the bounds of the map.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="pos">The position to check.</param>
        /// <returns>True if the position is inside the bounds of the map; False otherwise.</returns>
        private static bool WithinMap(Map map, Position pos)
        {
            if (pos.X < 0 || pos.X > map.X)
                return false;
            if (pos.Y < 0 || pos.Y > map.Y)
                return false;
            if (pos.Z < 0 || pos.Z > map.Z)
                return false;
            return true;
        }
    }
}
