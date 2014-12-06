namespace PCG_DFFortressGenerator.Classes
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A static class used for pathfinding in the fortress.
    /// </summary>
    public static class Pathfinding
    {
        /// <summary>
        /// Finds the distance from the start til to the end tile.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="start">The tile to start at.</param>
        /// <param name="end">The tile to end at.</param>
        /// <returns>The distance of the path. Returns -1 if there is no path.</returns>
        public static int DijkstraFindDistanceTo(Map map, Tile start, Tile end)
        {
            var path = DijkstraFindPathTo(map, start, end);
            if (path == null)
                return -1;
            return path.Count;
        }

        /// <summary>
        /// Finds the distance from the start til to one of the end tiles.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="start">The tile to start at.</param>
        /// <param name="endLocations">The set of tiles to use as end tiles.</param>
        /// <returns>The distance of the path. Returns -1 if there is no path.</returns>
        public static int DijkstraFindDistanceTo(Map map, Tile start, List<Tile> endLocations)
        {
            var path = DijkstraFindPathTo(map, start, endLocations);
            if (path == null)
            {
                return -1;
            }

            return path.Count;
        }

        /// <summary>
        /// Finds the distance from the start til to the end tile.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="start">The tile to start at.</param>
        /// <param name="tt">The tiletype to look for.</param>
        /// <returns>The distance of the path. Returns -1 if there is no path.</returns>
        public static int DijkstraFindDistanceTo(Map map, Tile start, Tile.TileType tt)
        {
            var path = DijkstraFindPathTo(map, start, tt);
            if (path == null) return -1;
            return path.Count;
        }

        /// <summary>
        /// Finds the shortest path from the start til to the end tile (uses Dijkstra's algorithm)
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="start">The tile to start at.</param>
        /// <param name="tt">The tiletype to look fort.</param>
        /// <returns>A linked list with the tiles that make up the path. Returns null if there is no path.</returns>
        public static LinkedList<Tile> DijkstraFindPathTo(Map map, Tile start, Tile.TileType tt)
        {
            TileNode endTile = null;

            var vertices = new List<TileNode>();
            var visited = new List<Tile>();

            var startTile = new TileNode { Tile = start };
            vertices.Add(startTile);
            visited.Add(start);

            // Find the path
            while (vertices.Count > 0)
            {
                // Grab first element of the list
                var tempTile = vertices[0];
                vertices.RemoveAt(0);

                // If it is the correct stair type, we're done
                if (tempTile.Tile.TileStatus.Equals(tt))
                {
                    endTile = tempTile;
                    break;
                }

                // Find neighbours and add them to the list of vertices to check, if they haven't been visited already.
                var neighbours = FindNeighbourTiles(map, tempTile.Tile);
                foreach (var neighbour in neighbours)
                {
                    if (visited.Contains(neighbour)) continue;

                    var newPfTile = new TileNode { Tile = neighbour, ParentNode = tempTile };
                    vertices.Add(newPfTile);
                    visited.Add(neighbour);
                }
            }

            // If we cannot reach the end tile, return null.
            if (endTile == null)
                return null;

            // Recreate the path to the stairs.
            var path = new LinkedList<Tile>();
            while (endTile != null)
            {
                path.AddFirst(endTile.Tile);
                endTile = endTile.ParentNode;
            }

            return path;
        }

        /// <summary>
        /// Finds the shortest path from the start til to the end tile (uses Dijkstra's algorithm)
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="start">The tile to start at.</param>
        /// <param name="end">The tile to end at.</param>
        /// <returns>A linked list with the tiles that make up the path. Returns null if there is no path.</returns>
        public static LinkedList<Tile> DijkstraFindPathTo(Map map, Tile start, Tile end)
        {
            TileNode endTile = null;

            var vertices = new List<TileNode>();
            var visited = new List<Tile>();

            var startTile = new TileNode { Tile = start };
            vertices.Add(startTile);
            visited.Add(start);

            // Find the path
            while (vertices.Count > 0)
            {
                // Grab first element of the list
                var tempTile = vertices[0];
                vertices.RemoveAt(0);

                // If it is the correct stair type, we're done
                if (tempTile.Tile.Equals(end))
                {
                    endTile = tempTile;
                    break;
                }

                // Find neighbours and add them to the list of vertices to check, if they haven't been visited already.
                var neighbours = FindNeighbourTiles(map, tempTile.Tile);
                foreach (var neighbour in neighbours)
                {
                    if (visited.Contains(neighbour)) continue;

                    var newPfTile = new TileNode { Tile = neighbour, ParentNode = tempTile };
                    vertices.Add(newPfTile);
                    visited.Add(neighbour);
                }
            }

            // If we cannot reach the end tile, return null.
            if (endTile == null)
                return null;

            // Recreate the path to the stairs.
            var path = new LinkedList<Tile>();
            while (endTile != null)
            {
                path.AddFirst(endTile.Tile);
                endTile = endTile.ParentNode;
            }

            return path;
        }

        /// <summary>
        /// Finds the shortest path from the start til to the end tile (uses Dijkstra's algorithm)
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="start">The tile to start at.</param>
        /// <param name="endLocations">The set of tiles to look for.</param>
        /// <returns>A linked list with the tiles that make up the path. Returns null if there is no path.</returns>
        public static LinkedList<Tile> DijkstraFindPathTo(Map map, Tile start, List<Tile> endLocations)
        {
            TileNode endTile = null;

            var vertices = new List<TileNode>();
            var visited = new List<Tile>();

            var startTile = new TileNode { Tile = start };
            vertices.Add(startTile);
            visited.Add(start);

            // Find the path
            while (vertices.Count > 0)
            {
                // Grab first element of the list
                var tempTile = vertices[0];
                vertices.RemoveAt(0);

                // If it is the correct stair type, we're done
                if (Enumerable.Contains(endLocations, tempTile.Tile))
                {
                    endTile = tempTile;
                    break;
                }

                // Find neighbours and add them to the list of vertices to check, if they haven't been visited already.
                var neighbours = FindNeighbourTiles(map, tempTile.Tile);
                foreach (var neighbour in neighbours)
                {
                    if (visited.Contains(neighbour))
                    {
                        continue;
                    }

                    var newPfTile = new TileNode { Tile = neighbour, ParentNode = tempTile };
                    vertices.Add(newPfTile);
                    visited.Add(neighbour);
                }
            }

            // If we cannot reach the end tile, return null.
            if (endTile == null)
            {
                return null;
            }

            // Recreate the path to the stairs.
            var path = new LinkedList<Tile>();
            while (endTile != null)
            {
                path.AddFirst(endTile.Tile);
                endTile = endTile.ParentNode;
            }

            return path;
        }

        /// <summary>
        /// Uses the Dijkstra algorithm to find a path that connects a tile to another tile (cannot search diagonally or through walls).
        /// </summary>
        /// <param name="map"> The map to do pathfinding on. </param>
        /// <param name="start"> The tile to start at. </param>
        /// <param name="end"> The tile to end at. </param>
        /// <returns> The list containing the path. </returns>
        public static LinkedList<Tile> DijkstraFindPathToOpenArea(Map map, Tile start, Tile end)
        {
            TileNode endTile = null;

            var vertices = new List<TileNode>();
            var visited = new List<Tile>();

            var startTile = new TileNode { Tile = start };
            vertices.Add(startTile);
            visited.Add(start);

            // Find the path
            while (vertices.Count > 0)
            {
                // Grab first element of the list
                var tempTile = vertices[0];
                vertices.RemoveAt(0);

                // If it is the correct stair type, we're done
                if (tempTile.Tile.Equals(end))
                {
                    endTile = tempTile;
                    break;
                }

                // Find neighbours and add them to the list of vertices to check, if they haven't been visited already.
                var neighbours = FindTilesForDigging(map, tempTile.Tile);
                foreach (var neighbour in neighbours)
                {
                    if (visited.Contains(neighbour)) continue;

                    var newPfTile = new TileNode { Tile = neighbour, ParentNode = tempTile };
                    vertices.Add(newPfTile);
                    visited.Add(neighbour);
                }
            }

            // If we cannot reach the end tile, return null.
            if (endTile == null)
            {
                return null;
            }

            // Recreate the path to the stairs.
            var pathToStairs = new LinkedList<Tile>();
            while (endTile != null)
            {
                pathToStairs.AddFirst(endTile.Tile);
                endTile = endTile.ParentNode;
            }

            return pathToStairs;
        }

        /// <summary>
        /// Finds the neighbours to the tile on this level.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="tile">The tile to find neighbours for.</param>
        /// <returns>A list of "legal" neighbours.</returns>
        private static IEnumerable<Tile> FindNeighbourTiles(Map map, Tile tile)
        {
            var neighbours = new List<Tile>();
            var tempX = tile.Position.X;
            var tempY = tile.Position.Y;
            var tempZ = tile.Position.Z;

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (OpenTile(map, new Position(tempX + x, tempY + y, tempZ)))
                        neighbours.Add(map.MapLayers[tempZ].MapTiles[tempX + x, tempY + y]);
                }
            }

            // If this tile has a stair going up, add the tile above as a neighbour.
            if (tile.TileStatus == Tile.TileType.Stairs)
                if (OpenTile(map, new Position(tempX, tempY, tempZ + 1)))
                    neighbours.Add(map.MapLayers[tempZ + 1].MapTiles[tempX, tempY]);

            // If this tile has a stair going down, add the tile below as a neighbour.
            if (tile.TileStatus == Tile.TileType.Stairs)
                if (OpenTile(map, new Position(tempX, tempY, tempZ - 1)))
                    neighbours.Add(map.MapLayers[tempZ - 1].MapTiles[tempX, tempY]);

            return neighbours;
        }

        /// <summary>
        /// Finds the neighbours to the tile on this level (which are not diagonal).
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="tile">The tile to find neighbours for.</param>
        /// <returns>A list of "legal" neighbours.</returns>
        private static IEnumerable<Tile> FindTilesForDigging(Map map, Tile tile)
        {
            var neighbours = new List<Tile>();
            var tempX = tile.Position.X;
            var tempY = tile.Position.Y;
            var tempZ = tile.Position.Z;

            for (var x = -1; x <= 1; x += 2)
            {
                if (IsDiggableTile(map, new Position(tempX + x, tempY, tempZ)))
                {
                    neighbours.Add(map.MapLayers[tempZ].MapTiles[tempX + x, tempY]);
                }
            }

            for (var y = -1; y <= 1; y += 2)
            {
                if (IsDiggableTile(map, new Position(tempX, tempY + y, tempZ)))
                {
                    neighbours.Add(map.MapLayers[tempZ].MapTiles[tempX, tempY + y]);
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Checks if the given position is part of a room wall and within the bounds of the map.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="pos">The position to check.</param>
        /// <returns>True if the position is inside the bounds of the map and is a wall; False otherwise.</returns>
        private static bool IsDiggableTile(Map map, Position pos)
        {
            if (!WithinMap(map, pos))
            {
                return false;
            }

            return map.MapLayers[pos.Z].MapTiles[pos.X, pos.Y].TileStatus != Tile.TileType.RoomWall;
        }

        /// <summary>
        /// Checks if the given position is an open tile (ie. not a wall and within the bounds of the map).
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="pos">The position to check.</param>
        /// <returns>True if the position is inside the bounds of the map and is not a wall; False otherwise.</returns>
        private static bool OpenTile(Map map, Position pos)
        {
            if (!WithinMap(map, pos))
            {
                return false;
            }

            return map.MapLayers[pos.Z].MapTiles[pos.X, pos.Y].TileStatus != Tile.TileType.NotDug;
        }

        /// <summary>
        /// Checks if the given position is within the bounds of the map.
        /// </summary>
        /// <param name="map">The map in use.</param>
        /// <param name="pos">The position to check.</param>
        /// <returns>True if the position is inside the bounds of the map; False otherwise.</returns>
        private static bool WithinMap(Map map, Position pos)
        {
            if (pos.X < 0 || pos.X >= map.X)
            {
                return false;
            }

            if (pos.Y < 0 || pos.Y >= map.Y)
            {
                return false;
            }

            return pos.Z >= 0 && pos.Z < map.Z;
        }

        /// <summary>
        /// A class that represents the tiles when used for pathfinding.
        /// </summary>
        private class TileNode
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TileNode"/> class.
            /// </summary>
            public TileNode()
            {
                this.GValue = 0;
                this.HValue = 0;
                this.Tile = null;
                this.ParentNode = null;
            }

            /// <summary>
            /// Gets the F score of the node (A*)
            /// </summary>
            public double FValue
            {
                get { return GValue + HValue; }
            }

            /// <summary>
            /// Gets or sets the tile represented by the node.
            /// </summary>
            public Tile Tile { get; set; }

            /// <summary>
            /// Gets or sets the parent node of this node.
            /// </summary>
            public TileNode ParentNode { get; set; }

            /// <summary>
            /// Gets or sets the G score (total cost to get here) of the node (A*)
            /// </summary>
            private double GValue { get; set; }

            /// <summary>
            /// Gets or sets the H score (heuristic to get to the goal) of the node (A*)
            /// </summary>
            private double HValue { get; set; }
        }
    }
}
