namespace PCG_DFFortressGenerator.Classes
{
    class Tile
    {
        /// <summary>
        /// Determines what the tile contains.
        /// </summary>
        public enum TileType
        {
            NotDug,
            Dug,
            Room,
            StairUp,
            StairDown
        };

        public TileType TileStatus { get; set; }

        public Room RoomType { get; set; }

        public Position Position { get; set; }

        public Tile(TileType tileStatus, Room roomType, Position position)
        {
            TileStatus = tileStatus;
            RoomType = roomType;
            Position = position;
        }

        public Tile() 
            : this(TileType.NotDug, null, null)
        {
        }

        public Tile(Position position)
            : this(TileType.NotDug, null, position)
        {
        }

        public override string ToString()
        {
            switch (TileStatus)
            {
                case TileType.NotDug:
                    return "X";

                case TileType.Dug:
                    return " ";

                case TileType.Room:
                    return RoomType.RoomName;

                case TileType.StairUp:
                    return "^";

                case TileType.StairDown:
                    return "v";

                default:
                    return "X";
            }
        }
    }
}
