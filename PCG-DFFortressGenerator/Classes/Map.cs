using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCG_DFFortressGenerator.Classes
{
    class Map
    {
        // Representation of map
        public char[, ,] MapData { get; private set; }

        public Map(int x, int y, int z)
        {
            MapData = new char[x, y, z];
        }
    }
}
