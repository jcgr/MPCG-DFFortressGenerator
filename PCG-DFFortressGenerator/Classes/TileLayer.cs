namespace PCG_DFFortressGenerator.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A class that represents one layer of the map.
    /// </summary>
    public class TileLayer
    {
        /// <summary>
        /// The size of the layer on the x-axis (height)
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The size of the layer on the y-axis (width)
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The height at which the layer is in the map.
        /// </summary>
        public int ZLevel { get; private set; }

        /// <summary>
        /// The tiles that make up the layer.
        /// </summary>
        public Tile[,] MapTiles { get; set; }

        /// <summary>
        /// The areas on this layer.
        /// </summary>
        public List<Area> LayerAreas { get; private set; }

        public Area Entrance { get; set; }

        public TileLayer(int x, int y, int z)
        {
            X = x;
            Y = x;
            ZLevel = z;
            MapTiles = new Tile[x, y];
            LayerAreas = new List<Area>();
            
            for (var xx = 0; xx < x; xx++)
                for (var yy = 0; yy < y; yy++)
                    MapTiles[xx, yy] = new Tile(new Position(xx, yy, ZLevel));
        }

//        public void SetTile(int x, int y, Tile tile)
//        {
//            MapTiles[x, y] = tile;
//        }

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

        public Area GenerateAndAddArea(int startX, int startY, int endX, int endY, Area area)
        {
            var xDifference = endX - startX;
            var yDifference = endY - startY;
            var xChange = xDifference < 0 ? -1 : 1;
            var yChange = yDifference < 0 ? -1 : 1;

            for (var x = 0; Math.Abs(x) <= Math.Abs(xDifference); x += xChange)
            {
                for (var y = 0; Math.Abs(y) <= Math.Abs(yDifference); y += yChange)
                {
                    SetTile(x + startX, y + startY, Tile.TileType.Room, area);
                    area.AddTile(MapTiles[x + startX, y + startY]);
                }
            }

            AddArea(area);
            return area;
        }

        /// <summary>
        /// Adds an area to this layer.
        /// </summary>
        /// <param name="area">The area to add</param>
        public void AddArea(Area area)
        {
            LayerAreas.Add(area);
        }

        public void ReplaceArea(int index, Area newArea)
        {
            if (index >= LayerAreas.Count)
                return;

            foreach (var tile in LayerAreas[index].AreaTiles)
            {
                this.SetTile(tile.Position.X, tile.Position.Y, tile.TileStatus, newArea);
                tile.AreaType = newArea;
                newArea.AddTile(tile);
            }

            LayerAreas[index] = newArea;
        }

        public TileLayer Copy()
        {
            var newLayer = new TileLayer(X, Y, ZLevel);

            foreach (var layerArea in LayerAreas)
            {
                var tempArea = layerArea.Copy();
                newLayer.AddArea(tempArea);
                foreach (var tile in tempArea.AreaTiles)
                {
                    newLayer.SetTile(tile.Position.X, tile.Position.Y, tile.TileStatus, tile.AreaType);
                }
            }

            return newLayer;
        }

        /// <summary>
        /// Checks if the given position is within the bounds of the map.
        /// </summary>
        /// <param name="x">The x-coordinate to check.</param>
        /// <param name="y">The y-coordinate to check.</param>
        /// <returns>True if the position is inside the bounds of the map; False otherwise.</returns>
        public bool WithinLayer(int x, int y)
        {
            if (x < 0 || x >= X)
                return false;
            if (y < 0 || y >= Y)
                return false;
            return true;
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
