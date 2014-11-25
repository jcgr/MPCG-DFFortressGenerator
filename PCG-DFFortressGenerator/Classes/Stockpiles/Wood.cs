namespace PCG_DFFortressGenerator.Classes.Stockpiles
{
    /// <summary>
    /// A class that represents a wood stockpile.
    /// </summary>
    class Wood : Stockpile
    {
        public Wood()
        {
            AreaName = "T";

            MinSize = 50;
            MaxSize = 200;
        }
    }
}
