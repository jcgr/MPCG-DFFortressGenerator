namespace PCG_DFFortressGenerator.Evolution
{
    using System.Collections.Generic;

    /// <summary>
    /// The genotype of an "Area" used in mutation.
    /// </summary>
    public class AreaGenotype
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaGenotype"/> class.
        /// </summary>
        /// <param name="distances"> The distances between this room and all others. </param>
        /// <param name="name"> The name of the area. </param>
        public AreaGenotype(Dictionary<int, double> distances, string name)
        {
            this.Distances = distances;
            this.Name = name;
        }

        /// <summary>
        /// Gets the type of the room at this area.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the distances to all other rooms.
        /// </summary>
        public Dictionary<int, double> Distances { get; private set; }
    }
}