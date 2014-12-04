namespace PCG_DFFortressGenerator.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Rooms;
    using Stockpiles;
    using Workshops;

    /// <summary>
    /// A class that represents a special area of the fortress (a room, a workshop or a stockpile).
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Area"/> class.
        /// </summary>
        /// <param name="minHeight"> The minimum height of the area. </param>
        /// <param name="maxHeight"> The maximum The minimum height of the area. </param>
        /// <param name="minWidth"> The minimum width of the area. </param>
        /// <param name="maxWidth"> The maximum width of the area. </param>
        /// <param name="areaName"> The name of the area. </param>
        public Area(int minHeight = 4, int maxHeight = 4, int minWidth = 4, int maxWidth = 4, string areaName = "1")
        {
            this.MinHeight = minHeight;
            this.MaxHeight = maxHeight;
            this.MinWidth = minWidth;
            this.MaxWidth = maxWidth;
            this.AreaName = areaName;

            this.AreaTiles = new List<Tile>();
        }

        /// <summary>
        /// Gets or sets the minimum height of the area.
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum width of the area.
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum height of the area.
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the area.
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// Gets the tiles that makes up the area.
        /// </summary>
        public List<Tile> AreaTiles { get; private set; }

        /// <summary>
        /// Gets or sets name of the area.
        /// Should only be a single character due to the map being ASCII.
        /// </summary>
        public string AreaName { get; protected set; }

        /// <summary>
        /// Gets the distances to all other areas in the map that this area is in.
        /// </summary>
        public Dictionary<int, double> Distances { get; private set; }

        /// <summary>
        /// Adds a tile to the area.
        /// </summary>
        /// <param name="tile"> The tile to add to the area. </param>
        public void AddTile(Tile tile)
        {
            AreaTiles.Add(tile);
        }

        /// <summary>
        /// Creates a deep copy of the area.
        /// </summary>
        /// <returns>A copy of the area.</returns>
        public Area Copy()
        {
            var newArea = new Area();
            var areaName = GetType().Name;

            switch (areaName)
            {
                // Rooms
                case "Barracks":
                    newArea = new Barracks();
                    break;
                case "Bedroom":
                    newArea = new Bedroom();
                    break;
                case "DiningRoom":
                    newArea = new DiningRoom();
                    break;
                case "Entrance":
                    newArea = new Entrance();
                    break;
                case "Farm":
                    newArea = new Farm();
                    break;
                case "Office":
                    newArea = new Office();
                    break;

                // Workshops
                case "Brewery":
                    newArea = new Brewery();
                    break;
                case "Carpenter":
                    newArea = new Carpenter();
                    break;
                case "Craftdwarf":
                    newArea = new Craftdwarf();
                    break;
                case "Fishery":
                    newArea = new Fishery();
                    break;
                case "Kitchen":
                    newArea = new Kitchen();
                    break;
                case "Mason":
                    newArea = new Mason();
                    break;
                case "Metalsmith":
                    newArea = new Metalsmith();
                    break;
                case "Smelter":
                    newArea = new Smelter();
                    break;
                case "WoodFurnace":
                    newArea = new WoodFurnace();
                    break;

                // Stockpiles
                case "BarBlock":
                    newArea = new BarBlock();
                    break;
                case "Cloth":
                    newArea = new Cloth();
                    break;
                case "FinishedGoods":
                    newArea = new FinishedGoods();
                    break;
                case "Food":
                    newArea = new Food();
                    break;
                case "Furniture":
                    newArea = new Furniture();
                    break;
                case "Leather":
                    newArea = new Leather();
                    break;
                case "Stone":
                    newArea = new Stone();
                    break;
                case "Weaponry":
                    newArea = new Weaponry();
                    break;
                case "Wood":
                    newArea = new Wood();
                    break;
            }

            newArea.AreaTiles = AreaTiles.Select(areaTile => new Tile(areaTile.TileStatus, newArea, areaTile.Position)).ToList();
            return newArea;
        }
    }
}
