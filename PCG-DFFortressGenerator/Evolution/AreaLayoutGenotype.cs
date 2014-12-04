namespace PCG_DFFortressGenerator.Evolution
{
    using System.Collections.Generic;

    using PCG_DFFortressGenerator.Classes;

    public class AreaLayoutGenotype
    {
        private double fitnessValue;

        private bool isFitnessCalculated;

        public double FitnessValue 
        {
            get
            {
                if (!this.isFitnessCalculated)
                {
                    fitnessValue = this.CalculateFitness();
                }
                else
                {
                    return FitnessValue;
                }
            }
        }

        public List<AreaGenotype> Areas;

        public AreaLayoutGenotype()
        {
            this.fitnessValue = double.MinValue;
        }

        private double CalculateFitness()
        {
            foreach (var ag in this.Areas)
            {
                // TODO: Calculate something
            }

            this.fitnessValue = 0.0;
        }
    }

    public class AreaGenotype
    {
        public Tile.TileType Type { get; private set; }

        public Dictionary<int, double> Distances;
    }
}
