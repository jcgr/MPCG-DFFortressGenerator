namespace PCG_DFFortressGenerator.Classes.Stockpiles
{
    /// <summary>
    /// A class that represents a stockpile area.
    /// </summary>
    class Stockpile : Area
    {
        public Stockpile()
        {
            MinSize = 9;
            MaxSize = 10000;

            MinHeight = 3;
            MinWidth = 3;
        }
    }
}
