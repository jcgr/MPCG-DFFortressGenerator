namespace PCG_DFFortressGenerator.Evolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The genotype of an area layout used in mutation.
    /// </summary>
    public class AreaLayoutGenotype
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaLayoutGenotype"/> class.
        /// </summary>
        /// <param name="generation"> The generation where this born. </param>
        /// <param name="areas">The areas to use.</param>
        public AreaLayoutGenotype(int generation, List<AreaGenotype> areas)
        {
            this.Areas = areas;
            this.FitnessValue = double.MinValue;
            this.Generation = generation;
        }

        /// <summary>
        /// Gets the generation of this area layout.
        /// </summary>
        public int Generation { get; private set; }

        /// <summary>
        /// Gets the fitness value of the area layout.
        /// </summary>
        public double FitnessValue { get; private set; }

        /// <summary>
        /// Gets the areas of the layout.
        /// </summary>
        public List<AreaGenotype> Areas { get; private set; }

        /// <summary>
        /// Gets a random room type.
        /// </summary>
        /// <returns>The random room type.</returns>
        public static string GetRandomRoom()
        {
            var nextInt = Evolver.Random.Next(23);
            switch (nextInt)
            {
                    // ----------
                    // Rooms
                    // ----------
                    // Barracks
                case 0:
                    return "r";

                    // Bedroom
                case 1:
                    return "b";

                    // Dining Room
                case 2:
                    return "d";

                    // Entrance would normally be 3, but we should not be able to generate án Entrance randomly in any room.
                    // Farm
                case 3:
                    return "f";

                    // Office
                case 4:
                    return "o";

                    // ----------
                    // Workshops
                    // ----------
                    // Brewery
                case 5:
                    return "q";

                    // Carpenter
                case 6:
                    return "c";

                    // Craftdwarf
                case 7:
                    return "¤";

                    // Fishery
                case 8:
                    return "e";

                    // Kitchen
                case 9:
                    return "k";

                    // Mason
                case 10:
                    return "m";

                    // Metalsmith
                case 11:
                    return "h";

                    // Smelter
                case 12:
                    return "s";

                    // Wood Furnace
                case 13:
                    return "u";

                    // ----------
                    // Stockpiles
                    // ----------
                    // BarBlock
                case 14:
                    return "B";

                    // Cloth
                case 15:
                    return "C";

                    // Finished Goods
                case 16:
                    return "G";

                    // Food
                case 17:
                    return "D";

                    // Furniture
                case 18:
                    return "U";

                    // Leather
                case 19:
                    return "L";

                    // Stone
                case 20:
                    return "S";

                    // Weaponry
                case 21:
                    return "W";

                    // Wood
                case 22:
                    return "T";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Mutates a layout into a new child.
        /// </summary>
        /// <param name="generation"> The generation where this born. </param>
        /// <param name="mutationChance"> The chance that a room is mutated into a different room.  </param>
        /// <param name="requiredAreas"> The required areas for the generation. </param>
        /// <returns> The newly created <see cref="AreaLayoutGenotype"/>.  </returns>
        public AreaLayoutGenotype Mutate(int generation, double mutationChance, Dictionary<string, int> requiredAreas)
        {
            // TODO: Grooss check mutate
            var rand = new Random();
            var newLayoutList = new List<AreaGenotype>();
 
            for (var i = 0; i < this.Areas.Count; i++)
            {
                var oldArea = this.Areas[i];
                var areaName = rand.NextDouble() <= mutationChance && oldArea.Name != "@"
                                   ? GetRandomRoom()
                                   : oldArea.Name;
                var area = new AreaGenotype(oldArea.Distances, areaName);
                newLayoutList.Add(area);
            }

            var newLayout = new AreaLayoutGenotype(this.Generation, newLayoutList);
            newLayout.FitnessValue = newLayout.CalculateFitness(requiredAreas);
            return newLayout;
        }

        /// <summary>
        /// Calculates the fitness of this area layout.
        /// </summary>
        /// <param name="requiredAreas">The areas required</param>
        /// <returns> The fitness of the layout as a <see cref="double"/>. </returns>
        private double CalculateFitness(Dictionary<string, int> requiredAreas)
        {
            // TODO: Grooss check CalculateFitness
            var fitness = 0.0;

            foreach (var keyValuePair in requiredAreas)
            {
                var areaAmount = this.Areas.Count(a => a.Name == keyValuePair.Key);
                var requiredRooms = keyValuePair.Value;
                if (areaAmount >= requiredRooms)
                {
                    continue;
                }

                var missingRooms = requiredRooms - areaAmount;
                fitness -= missingRooms * (Evolver.MissingRoomPenalty + (Evolver.MissingRoomPenaltyScalingFactor * this.Generation));
            }

            foreach (var a in this.Areas)
            {
                for (var i = 0; i < Areas.Count; i++)
                {
                    var target = this.Areas[i];
                    if (target == a)
                    {
                        continue;
                    }

                    try
                    {
                        var dist = a.Distances[i];
                        var weight = Evolver.AreaWeights[a.Name][target.Name];
                        fitness += dist * weight;
                    }
                    catch (KeyNotFoundException knfe)
                    {
                        fitness += 0.0;
                    }
                }
            }

            return fitness;
        }
    }
}
