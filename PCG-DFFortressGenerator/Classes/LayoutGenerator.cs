using System;
using System.Collections.Generic;
using System.Windows.Documents;
using PCG_DFFortressGenerator.Classes.Rooms;

namespace PCG_DFFortressGenerator.Classes
{
    class LayoutGenerator
    {
        public Map Map { get; private set; }

        private int NumberOfDwarves { get; set; }

        private int NumberOfRooms { get; set; }

        private Random Random { get; set; }

        public LayoutGenerator(Map map, List<Area> chosenRooms, int numberOfDwarves)
        {
            Map = map;
            Random = new Random();
            NumberOfDwarves = numberOfDwarves;
            NumberOfRooms = CalculateNumberOfRooms();
            
            Map.MapLayers[Map.Z - 1] = GenerateEntrance(Map.MapLayers[Map.Z - 1]);
            for (int z = Map.Z - 1; z > 0; z--)
            {
                Map.MapLayers[z] = GenerateRooms(Map.MapLayers[z]);
            }

        }

        private int CalculateNumberOfRooms()
        {
            return 1;
        }

        private TileLayer GenerateEntrance(TileLayer layer)
        {
            var tile1 = Random.Next(layer.X - 2) + 1;
            var tile2 = -1;

            if (tile1 == 1)
                tile2 = 2;
            if (tile1 == layer.X - 1)
                tile2 = layer.X - 2;

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
            
            var entrance = new Entrance();
            layer.SetTile(tile1, 0, Tile.TileType.Room, entrance);
            layer.SetTile(tile2, 0, Tile.TileType.Room, entrance);
            entrance.AddTile(layer.MapTiles[tile1, 0]);
            entrance.AddTile(layer.MapTiles[tile2, 0]);
            layer.AddArea(entrance);

            return layer;
        }

        private TileLayer GenerateRooms(TileLayer tileLayer)
        {
            bool done = false;

            while (!done)
            {
                // Generate room
                // Generate path to entrance
                // Check if done with layer
                done = true;
            }

            return tileLayer;
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
