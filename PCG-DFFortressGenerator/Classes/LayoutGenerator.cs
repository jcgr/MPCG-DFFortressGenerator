using System;
using System.Collections.Generic;
using System.Linq;
using PCG_DFFortressGenerator.Classes.Rooms;

namespace PCG_DFFortressGenerator.Classes
{
    class LayoutGenerator
    {
        private int NumberOfDwarves { get; set; }

        private int NumberOfRooms { get; set; }

        public Map Map { get; private set; }

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
            GenerateRooms(Map.MapLayers[Map.Z - 1]);
//            for (int z = Map.Z - 1; z > 0; z--)
//            {
//                GenerateRooms(Map.MapLayers[z]);
//            }

        }

        private int CalculateNumberOfRooms()
        {
            var rooms = 0;

            if (ChosenAreas.OfType<Bedroom>().Any())
                rooms += (int)Math.Ceiling(NumberOfDwarves / 8d);
            if (ChosenAreas.OfType<DiningRoom>().Any())
                rooms += (int)Math.Ceiling(NumberOfDwarves / 6d);

            rooms += NumberOfDwarves / 2;

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
                {
                    tile2 = tile1 + 1;
                }
                else
                {
                    tile2 = tile1 - 1;
                }
            }

            var entrance = layer.GenerateAndAddArea(tile2, 0, tile1, 0, new Entrance());
            layer.Entrance = entrance;
//            var entrance = new Entrance();
//            entrance.AddTile(layer.MapTiles[tile1, 0]);
//            entrance.AddTile(layer.MapTiles[tile2, 0]);
//            layer.SetTile(tile1, 0, Tile.TileType.Room, entrance);
//            layer.SetTile(tile2, 0, Tile.TileType.Room, entrance);
//            layer.AddArea(entrance);

//            // TODO: This is test!
//            Console.WriteLine("---------------");
//            foreach (var areaTile in entrance.AreaTiles)
//            {
//                Console.WriteLine(areaTile);
//            }
//            Console.WriteLine("---------------");

//            foreach (var areaTile in entrance.AreaTiles)
//            {
//                Console.WriteLine(areaTile);
//            }
//            Console.WriteLine("---------------");
//            entrance.test();
//            Console.WriteLine("---------------");

//            var barracks = layer.GenerateAndAddArea(4, 4, 3, 3, new Barracks());

//            var newArea = barracks.Copy();
//            Console.WriteLine(newArea.AreaTiles[0].TileStatus + " - " + newArea.AreaTiles.Count);
        }

        private void GenerateRooms(TileLayer tileLayer)
        {
            var done = false;
            var openPositions = new List<Position>();

            foreach (var mapTile in tileLayer.MapTiles)
                if (mapTile.TileStatus == Tile.TileType.NotDug)
                    openPositions.Add(mapTile.Position);

            while (!done)
            {
                GenerateRoomAndPath(tileLayer, openPositions);
                done = IsLayerFinished(tileLayer);
            }
        }

        private void GenerateRoomAndPath(TileLayer tileLayer, List<Position> openPositions)
        {
            var generated = false;
            var tempArea = new Area();

            while (!generated)
            {
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
                    var combinationIndex = Random.Next(possibleCombinations.Count);
                    var combination = possibleCombinations[combinationIndex];
                    possibleCombinations.RemoveAt(combinationIndex);

                    var checkX = startPosition.X + (combination.Item1 * 5);
                    var checkY = startPosition.Y + (combination.Item2 * 5);

                    if (tileLayer.WithinLayer(checkX, checkY))
                    {
                        if (this.TilesOpenBetweeen(startPosition.X, startPosition.Y, checkX, checkY, tileLayer))
                        {
                            tempArea = tileLayer.GenerateAndAddArea(
                                startPosition.X + combination.Item1
                                , startPosition.Y + combination.Item2
                                , checkX - combination.Item1
                                , checkY - combination.Item2
                                , new Area());

                            foreach (var areaTile in tempArea.AreaTiles)
                            {
                                var tempPosition = areaTile.Position;
                                openPositions.Remove(tempPosition);
                            }

                            NumberOfRooms--;

                            break;
                        }
                    }
                }

                if (openPositions.Contains(startPosition))
                    openPositions.Remove(startPosition);

                Console.WriteLine(NumberOfRooms);

                if (NumberOfRooms <= 0)
                    generated = true;

                if (openPositions.Count <= tileLayer.X + tileLayer.Y)
                    generated = true;
            }
        }

        private bool TilesOpenBetweeen(int startX, int startY, int endX, int endY, TileLayer tileLayer)
        {
            var xDifference = endX - startX;
            var yDifference = endY - startY;
            var xChange = xDifference < 0 ? -1 : 1;
            var yChange = yDifference < 0 ? -1 : 1;

            for (var x = 0; Math.Abs(x) <= Math.Abs(xDifference); x += xChange)
                for (var y = 0; Math.Abs(y) <= Math.Abs(yDifference); y += yChange)
                    if (tileLayer.MapTiles[x + startX, y + startY].TileStatus != Tile.TileType.NotDug
                        || tileLayer.MapTiles[x + startX, y + startY].TileStatus != Tile.TileType.RoomWall)
                        return false;

            return true;
        }

        private bool IsLayerFinished(TileLayer tileLayer)
        {
            return true;
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
