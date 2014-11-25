namespace PCG_DFFortressGenerator.Classes.Stockpiles
{
    /// <summary>
    /// A class that represents a finished goods stockpile.
    /// </summary>
    class FinishedGoods : Stockpile
    {
        public FinishedGoods()
        {
            AreaName = "D";

            MinSize = 20;
            MaxSize = 50;
        }
    }
}
