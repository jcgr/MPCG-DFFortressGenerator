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
    class Area
    {
        public int MinHeight { get; set; }

        public int MinWidth { get; set; }

        public int MaxHeight { get; set; }

        public int MaxWidth { get; set; }

        public List<Tile> AreaTiles { get; private set; }

        // Make sure it is only one character due to the map being ASCII for now.
        public String AreaName { get; protected set; }

        public Area()
        {
            MinHeight = 4;
            MaxHeight = 4;
            MinWidth = 4;
            MaxWidth = 4;
            AreaName = "1";

            AreaTiles = new List<Tile>();
        }

        /// <summary>
        /// Adds a tile to the area.
        /// </summary>
        /// <param name="tile">The tile.</param>
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
