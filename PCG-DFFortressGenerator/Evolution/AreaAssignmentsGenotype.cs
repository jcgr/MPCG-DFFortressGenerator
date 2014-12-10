namespace PCG_DFFortressGenerator.Evolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The genotype of an area assignment used in mutation.
    /// </summary>
    public class AreaAssignmentsGenotype
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaAssignmentsGenotype"/> class.
        /// </summary>
        /// <param name="generation"> The generation where this born. </param>
        /// <param name="areas">The areas to use.</param>
        public AreaAssignmentsGenotype(int generation, List<AreaGenotype> areas)
        {
            this.Areas = areas;
            this.FitnessValue = double.MinValue;
            this.Generation = generation;
        }

        /// <summary>
        /// Gets the generation of this area assignment.
        /// </summary>
        public int Generation { get; private set; }

        /// <summary>
        /// Gets the fitness value of the area assignment.
        /// </summary>
        public double FitnessValue { get; private set; }

        /// <summary>
        /// Gets the areas of the assignment.
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
        /// Mutates as assignment into a new child.
        /// </summary>
        /// <param name="generation"> The generation where this born. </param>
        /// <param name="mutationChance"> The chance that a room is mutated into a different room.  </param>
        /// <param name="requiredAreas"> The required areas for the generation. </param>
        /// <returns> The newly created <see cref="AreaAssignmentsGenotype"/>.  </returns>
        public AreaAssignmentsGenotype Mutate(int generation, double mutationChance, Dictionary<string, int> requiredAreas)
        {
            var rand = new Random();
            var newAssignmentList = new List<AreaGenotype>();

            for (var i = 0; i < this.Areas.Count; i++)
            {
                var oldArea = this.Areas[i];
                var areaName = (rand.NextDouble() <= mutationChance && oldArea.Name != "@")
                                   ? GetRandomRoom() : oldArea.Name;
                var area = new AreaGenotype(oldArea.Distances, areaName);
                newAssignmentList.Add(area);
            }

            var newAssignment = new AreaAssignmentsGenotype(this.Generation, newAssignmentList);
            newAssignment.FitnessValue = newAssignment.CalculateFitness(requiredAreas);
            return newAssignment;
        }

        /// <summary>
        /// Calculates the fitness of this area assignment.
        /// </summary>
        /// <param name="requiredAreas">The areas required</param>
        /// <returns> The fitness of the assignment as a <see cref="double"/>. </returns>
        private double CalculateFitness(Dictionary<string, int> requiredAreas)
        {
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

            var areaOccourances = new Dictionary<string, int>();

            // Find fitness for each individual room
            foreach (var a in this.Areas)
            {
                var roomFitness = 0.0;

                var distanceValues = Evolver.AreaWeights[a.Name];
                for (var i = 0; i < this.Areas.Count; i++)
                {
                    var target = this.Areas[i];
                    if (target == a)
                    {
                        continue;
                    }

                    double weight;
                    if (distanceValues.TryGetValue(target.Name, out weight))
                    {
                        var dist = a.Distances[i];
                        roomFitness += dist * weight;
                    }
                }

                // Increase occourance of this area
                if (areaOccourances.ContainsKey(a.Name))
                    areaOccourances[a.Name]++;
                else
                    areaOccourances[a.Name] = 1;

                int occouranceValue;

                // Lower fitness if we have enough of the room already.
                if (areaOccourances.TryGetValue(a.Name, out occouranceValue))
                {
                    int requiredAreaValue;

                    if (requiredAreas.TryGetValue(a.Name, out requiredAreaValue))
                    {
                        if (occouranceValue > requiredAreaValue)
                            roomFitness = roomFitness / Math.Pow(2, occouranceValue - requiredAreaValue);
                    }
                    else
                    {
                        if (occouranceValue >= 1)
                            roomFitness = roomFitness / Math.Pow(2, occouranceValue);
                    }
                }

                fitness += roomFitness;
            }

            return fitness;
        }
    }
}
