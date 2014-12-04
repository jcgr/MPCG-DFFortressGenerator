namespace PCG_DFFortressGenerator.Evolution
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The genotype of an area layout used in mutation.
    /// </summary>
    public class AreaLayoutGenotype
    {
        /// <summary>
        /// The fitness value of the layout.
        /// </summary>
        private double fitnessValue;

        /// <summary>
        /// Only calculate fitness if 
        /// </summary>
        private bool isFitnessCalculated;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaLayoutGenotype"/> class.
        /// </summary>
        public AreaLayoutGenotype()
        {
            this.fitnessValue = double.MinValue;
        }

        /// <summary>
        /// Gets the fitness value of the area layout.
        /// </summary>
        public double FitnessValue 
        {
            get
            {
                if (this.isFitnessCalculated)
                {
                    return this.fitnessValue;
                }

                this.fitnessValue = this.CalculateFitness();
                this.isFitnessCalculated = true;
                return this.fitnessValue;
            }
        }

        /// <summary>
        /// Gets the areas of the layout.
        /// </summary>
        public List<AreaGenotype> Areas { get; private set; }

        /// <summary>
        /// Mutates a .
        /// </summary>
        /// <param name="mutationChance"> The chance that a room is mutated into a different room. </param>
        /// <returns> The newly created <see cref="AreaLayoutGenotype"/>. </returns>
        public AreaLayoutGenotype Mutate(double mutationChance)
        {
            // TODO: Implement mutate
            return null;
        }

        private string GetRandomRoom()
        {
            var rand = new Random(23);
            var nextInt = rand.Next();
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
//                // See Case 3 for explanation
//                case 23:
//                    return "";
            }

            return string.Empty;
            // TODO: Finish randomzing rooms (Done? Melnyk, check)
        }

        /// <summary>
        /// Calculates the fitness of this area layout.
        /// </summary>
        /// <returns> The fitness of the layout as a <see cref="double"/>. </returns>
        private double CalculateFitness()
        {
            foreach (var ag in this.Areas)
            {
                // TODO: Calculate fitness (Melnyk)
            }

            return 0.0;
        }
    }
}
