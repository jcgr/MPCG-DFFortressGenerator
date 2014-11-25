namespace PCG_DFFortressGenerator.Classes.Stockpiles
{
    /// <summary>
    /// A class that represents a refuse stockpile.
    /// </summary>
    class Refuse : Stockpile
    {
        public Refuse()
        {
            AreaName = "R";

            MinSize = 15;
            MaxSize = 50;
        }
    }
}
