using System;

namespace PCG_DFFortressGenerator.Classes
{
    class Room
    {
        private int minHeight, minWidth, maxHeight, maxWidth, minSize, maxSize;

        // Make sure it is only one character due to the map being ASCII for now.
        public String RoomName { get; protected set; }
    }
}
