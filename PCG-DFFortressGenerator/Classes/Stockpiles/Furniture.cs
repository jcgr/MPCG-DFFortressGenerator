namespace PCG_DFFortressGenerator.Classes.Stockpiles
{
    /// <summary>
    /// A class that represents a furniture stockpile.
    /// </summary>
    class Furniture : Stockpile
    {
        public Furniture()
        {
            AreaName = "U";

            MinSize = 40;
            MaxSize = 150;
        }
    }
}
