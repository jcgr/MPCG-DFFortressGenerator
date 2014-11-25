namespace PCG_DFFortressGenerator.Classes
{
    /// <summary>
    /// A class that represents one of the tiles of the map.
    /// </summary>
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
            StairDown,
            StairUpDown
        };

        /// <summary>
        /// The status of the tile.
        /// </summary>
        public TileType TileStatus { get; set; }

        /// <summary>
        /// The type of room that the tile contains (null if it is not a specific type of room)
        /// </summary>
        public Room RoomType { get; set; }

        /// <summary>
        /// The position of the tile.
        /// </summary>
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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
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

                case TileType.StairUpDown:
                    return "|";

                default:
                    return "X";
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tile) obj);
        }

        protected bool Equals(Tile other)
        {
            return Equals(Position, other.Position);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return (Position != null ? Position.GetHashCode() : 0);
        }
    }
}
