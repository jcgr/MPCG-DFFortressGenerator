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
            var rand = new Random(24);
            var nextInt = rand.Next();
            switch (nextInt)
            {
                // Rooms
                // Barracks
                case 0:
                    return "r";
                //
                case 1:
                    return "";
                case 2:
                //
                case 3:
                //
                case 4:
                //
                case 5:
                // Workshops
                //
                case 6:
                //
                case 7:
                //
                case 8:
                //
                case 9:
                //
                case 10:
                //
                case 11:
                //
                case 12:
                //
                case 13:
                //
                case 14:
                // Stockpiles
                //
                case 15:
                //
                case 16:
                //
                case 17:
                //
                case 18:
                //
                case 19:
                //
                case 20:
                //
                case 21:
                //
                case 22:
                //
                case 23:

                    break;
            }

            return string.Empty;
            // TODO: Finish randomizing rooms (Grooss)
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
