namespace PCG_DFFortressGenerator.Classes
{
    /// <summary>
    /// A class that represents the position of something.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="x"> The x-coordinate. </param>
        /// <param name="y"> The yx-coordinate. </param>
        /// <param name="z"> The zx-coordinate. </param>
        public Position(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gets the x-coordinate of the position
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the y-coordinate of the position
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Gets the z-coordinate of the position
        /// </summary>
        public int Z { get; private set; }

        /// <summary>
        /// Creates a copy of the position.
        /// </summary>
        /// <returns>A copy of the position.</returns>
        public Position Copy()
        {
            return new Position(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return "(X: " + this.X + ", Y: " + this.Y + ", Z: " + this.Z + ")";
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((Position)obj);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.X;
                hashCode = (hashCode * 397) ^ this.Y;
                hashCode = (hashCode * 397) ^ this.Z;
                return hashCode;
            }
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals(Position other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }
    }
}