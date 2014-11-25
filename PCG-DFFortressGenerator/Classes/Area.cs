using System;

namespace PCG_DFFortressGenerator.Classes
{
    /// <summary>
    /// A class that represents a special area of the fortress (a room, a workshop or a stockpile).
    /// </summary>
    class Area
    {
        public int MinHeight { get; set; }

        public int MinWidth { get; set; }

        public int MaxHeight { get; set; }

        public int MaxWidth { get; set; }

        public int MinSize { get; set; }

        public int MaxSize { get; set; }

        // Make sure it is only one character due to the map being ASCII for now.
        public String AreaName { get; protected set; }

        public Area()
        {
            MinHeight = 0;
            MaxHeight = 100;
            MinWidth = 0;
            MaxWidth = 100;
            MinSize = 0;
            MaxSize = 10000;
            AreaName = "ø";
        }
    }
}
