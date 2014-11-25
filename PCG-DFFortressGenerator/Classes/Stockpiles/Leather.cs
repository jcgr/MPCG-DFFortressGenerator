namespace PCG_DFFortressGenerator.Classes.Stockpiles
{
    /// <summary>
    /// A class that represents a leather stockpile.
    /// </summary>
    class Leather : Stockpile
    {
        public Leather()
        {
            AreaName = "L";

            MinSize = 9;
            MaxSize = 25;
        }
    }
}
