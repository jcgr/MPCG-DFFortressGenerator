namespace PCG_DFFortressGenerator.Evolution
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using PCG_DFFortressGenerator.Classes;
    using PCG_DFFortressGenerator.Classes.Rooms;

    /// <summary>
    /// The evolver used in evolving maps.
    /// </summary>
    public class Evolver
    {
        #region Mutation Constants
        /// <summary>
        /// The chance that a room will mutate into a different room.
        /// </summary>
        public const double MutationChance = 0.30;

        /// <summary>
        /// The number of children to generate per parent in a new generation.
        /// </summary>
        public const int ChildrenToSpawn = 10;

        /// <summary>
        /// Number of generations to run.
        /// </summary>
        public const int NumberOfGenerations = 100;

        /// <summary>
        /// The number of different assignments that are used in evolution.
        /// </summary>
        public const int NumberOfAssignmentsToGenerate = 1;

        /// <summary>
        /// The penalty to apply to a fitness of an assignment if it does not contain the required rooms.
        /// </summary>
        public const double MissingRoomPenalty = 100;

        /// <summary>
        /// The amount to increase the penalty per generation.
        /// </summary>
        public const double MissingRoomPenaltyScalingFactor = 2;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Evolver"/> class.
        /// </summary>
        /// <param name="x"> The height of the map. </param>
        /// <param name="y"> The width of the map. </param>
        /// <param name="z"> The depth of the map. </param>
        /// <param name="chosenAreas"> The required rooms for this evolution.  </param>
        /// <param name="numberOfDwarves"> The number Of dwarves to plan for. </param>
        public Evolver(int x, int y, int z, List<Area> chosenAreas, int numberOfDwarves)
        {
            Random = new Random();
            this.CurrentGeneration = 0;
            this.GeneratedMaps = new List<Map>();
            this.GeneratedAssignments = new List<AreaAssignmentsGenotype>();
            AreaWeights = GenerateAreaWeights();

            var numberOfRoomsRequired = CalculateNumberOfRooms(chosenAreas, numberOfDwarves);
            var lg = new MapGenerator(x, y, z, numberOfRoomsRequired);

            var sw = new Stopwatch();
            sw.Start();
            //Console.WriteLine("-----------------------------------");
            //Console.WriteLine();
            Console.WriteLine("Starting layout generation - " + (sw.ElapsedMilliseconds / 1000d));
            // Generate layouts and calculate distances between rooms for each layout.
            for (var i = 0; i < NumberOfAssignmentsToGenerate; i++)
            {
                this.GeneratedMaps.Add(lg.GenerateNewMap());
                Console.WriteLine("Layout " + i + " generated - " + (sw.ElapsedMilliseconds / 1000d));
                this.GeneratedMaps[i].CalculateDistancesBetweenAreas();
                Console.WriteLine("Distance for layout " + i + " calculated - " + (sw.ElapsedMilliseconds / 1000d));
            }
            //Console.WriteLine("Finished layout generation - " + (sw.ElapsedMilliseconds / 1000d));
            //Console.WriteLine();
            //Console.WriteLine("-----------------------------------");
            //Console.WriteLine();

            this.RequiredAreas = GetRequiredAreas(chosenAreas, numberOfDwarves);

            //Console.WriteLine("Initializeing layout populations - " + (sw.ElapsedMilliseconds / 1000d));
            for (var i = 0; i < NumberOfAssignmentsToGenerate; i++)
            {
                var listOfAreas = new List<AreaGenotype>();
                foreach (var b in this.GeneratedMaps[i].GetAllAreas())
                {
                    var areaToAdd = (b.AreaName == "@") ? new AreaGenotype(b.Distances, "@") : new AreaGenotype(b.Distances, AreaAssignmentsGenotype.GetRandomRoom());
                    listOfAreas.Add(areaToAdd);
                }

                this.GeneratedAssignments.Add(new AreaAssignmentsGenotype(this.CurrentGeneration, listOfAreas));
                //Console.WriteLine(i + " has been populated");
            }

            //Console.WriteLine("Layout populations finished - " + (sw.ElapsedMilliseconds / 1000d));
            //Console.WriteLine();
            //Console.WriteLine("-----------------------------------");
            //Console.WriteLine();

            CurrentGeneration++;

            //Console.WriteLine("Starting mutation - " + (sw.ElapsedMilliseconds / 1000d));
            // Mutate layouts
            while (CurrentGeneration < NumberOfGenerations)
            {
                for (var l = 0; l < this.GeneratedAssignments.Count; l++)
                {
                    var children = new List<AreaAssignmentsGenotype>();
                    var parent = this.GeneratedAssignments[l];

                    for (var i = 0; i < ChildrenToSpawn; i++)
                    {
                        children.Add(parent.Mutate(CurrentGeneration, MutationChance, RequiredAreas));
                    }

                    var bestChild = children.FirstOrDefault(c => Math.Abs(c.FitnessValue - children.Max(assignment => assignment.FitnessValue)) < 1E-17);
                    var bestFit = ((bestChild != null) && (parent.FitnessValue <= bestChild.FitnessValue)) ? bestChild : parent;

                    this.GeneratedAssignments[l] = bestFit;
                }

                //Console.WriteLine("Generation " + CurrentGeneration + "/" + NumberOfGenerations + " is finished - " + (sw.ElapsedMilliseconds / 1000d));
                CurrentGeneration++;
            }

            //Console.WriteLine("Mutation finished - " + (sw.ElapsedMilliseconds / 1000d));
            //Console.WriteLine();
            //Console.WriteLine("-----------------------------------");
            //Console.WriteLine();

            //Console.WriteLine("Copying generated layouts to maps - " + (sw.ElapsedMilliseconds / 1000d));
            for (var i = 0; i < GeneratedMaps.Count; i++)
            {
                for (var j = 0; j < this.GeneratedAssignments[i].Areas.Count; j++)
                {
                    var oldArea = GeneratedMaps[i].GetAllAreas()[j];
                    var newName = this.GeneratedAssignments[i].Areas[j].Name;
                    var mapLayer = GeneratedMaps[i].MapLayers[oldArea.AreaTiles[0].Position.Z];
                    mapLayer.ReplaceArea(oldArea, new Area(areaName: newName));
                }
                //Console.WriteLine("Layout " + i + " copied");
            }

            //Console.WriteLine("Layouts copied - " + (sw.ElapsedMilliseconds / 1000d));
            //Console.WriteLine();
            //Console.WriteLine("-----------------------------------");
            //Console.WriteLine();

            //for (var mapIndex = 0; mapIndex < GeneratedMaps.Count; mapIndex++)
            //{
            //    var map = GeneratedMaps[mapIndex];
            //    Console.WriteLine("Map " + mapIndex + " looks like this:");

            //    for (var layer = map.Z - 1; layer >= 0; layer--)
            //    {
            //        if (map.MapLayers[layer].LayerAreas.Count <= 0) continue;
            //        Console.WriteLine();
            //        Console.WriteLine("Layer " + layer);
            //        Console.WriteLine(map.MapLayers[layer]);
            //    }

            //    Console.WriteLine();
            //    Console.WriteLine("-----------------------------------");
            //    Console.WriteLine();
            //}
            //Console.WriteLine("Done! - " + (sw.ElapsedMilliseconds / 1000d));

            // TODO: (Grooss check) Do evolution loop
        }

        /// <summary>
        /// Gets the weights between areas.
        /// </summary>
        public static Dictionary<string, Dictionary<string, double>> AreaWeights { get; private set; }

        /// <summary>
        /// Gets the random object.
        /// </summary>
        public static Random Random { get; private set; }

        /// <summary>
        /// Gets the current generation of the evolution.
        /// </summary>
        public int CurrentGeneration { get; private set; }

        /// <summary>
        /// Gets the generated assignments.
        /// </summary>
        public List<AreaAssignmentsGenotype> GeneratedAssignments { get; private set; }

        /// <summary>
        /// Gets the maps generated by the evolver.
        /// </summary>
        public List<Map> GeneratedMaps { get; private set; }

        /// <summary>
        /// Gets or sets the required rooms for the area assignments.
        /// </summary>
        public Dictionary<string, int> RequiredAreas { get; set; }

        /// <summary>
        /// The generate area weights.
        /// </summary>
        /// <returns> The Dictionary containing the weights between areas. </returns>
        private static Dictionary<string, Dictionary<string, double>> GenerateAreaWeights()
        {
            var finalDict = new Dictionary<string, Dictionary<string, double>>();
            const double Close = -1.0;
            const double Far = 1.0;

            // ----------
            // Rooms
            // ----------
            // Barracks
            var barrackDict = new Dictionary<string, double>
                                    {
                                        { "r", Close },
                                        { "b", Far },
                                        { "d", Far },
                                        { "@", Close }
                                    };
            finalDict.Add("r", barrackDict);

            // Bedroom
            var bedroomDict = new Dictionary<string, double> { { "b", Close }, { "r", Far }, { "@", Far } };
            finalDict.Add("b", bedroomDict);

            // Dining Room
            var diningroomDict = new Dictionary<string, double> { { "d", Close }, { "r", Far }, { "@", Far } };
            finalDict.Add("d", diningroomDict);

            // Entrance
            var entranceDict = new Dictionary<string, double>
                                    {
                                        { "@", Close },
                                        { "r", Close },
                                        { "b", Far },
                                        { "d", Far }
                                    };
            finalDict.Add("@", entranceDict);

            // Farm
            var farmDict = new Dictionary<string, double> { { "f", Close } };
            finalDict.Add("f", farmDict);

            // Office
            var officeDict = new Dictionary<string, double> { { "o", Close } };
            finalDict.Add("o", officeDict);

            // ----------
            // Workshops
            // ----------
            // Brewery
            var breweryDict = new Dictionary<string, double> { { "q", Close }, { "D", Close } };
            finalDict.Add("q", breweryDict);

            // Carpenter
            var carpenterDict = new Dictionary<string, double> { { "c", Close }, { "U", Close }, { "T", Close } };
            finalDict.Add("c", carpenterDict);

            // Craftdwarf
            var craftdwarfDict = new Dictionary<string, double>
                                    {
                                        { "¤", Close },
                                        { "G", Close },
                                        { "S", Close },
                                        { "T", Close }
                                    };
            finalDict.Add("¤", craftdwarfDict);

            // Fishery
            var fisheryDict = new Dictionary<string, double> { { "e", Close }, { "D", Close } };
            finalDict.Add("e", fisheryDict);

            // Kitchen
            var kitchenDict = new Dictionary<string, double> { { "k", Close }, { "D", Close } };
            finalDict.Add("k", kitchenDict);

            // Mason
            var masonDict = new Dictionary<string, double> { { "m", Close }, { "U", Close }, { "S", Close } };
            finalDict.Add("m", masonDict);

            // Metalsmith
            var metalsmithDict = new Dictionary<string, double> { { "h", Close }, { "B", Close }, { "W", Close } };
            finalDict.Add("h", metalsmithDict);

            // Smelter
            var smelterDict = new Dictionary<string, double> { { "s", Close }, { "u", Close }, { "B", Close } };
            finalDict.Add("s", smelterDict);

            // Wood Furnace
            var woodfurnaceDict = new Dictionary<string, double>
                                    {
                                        { "u", Close },
                                        { "s", Close },
                                        { "B", Close },
                                        { "T", Close }
                                    };
            finalDict.Add("u", woodfurnaceDict);

            // ----------
            // Stockpiles
            // ----------
            // BarBlock
            var barblockDict = new Dictionary<string, double>
                                    {
                                        { "B", Close },
                                        { "s", Close },
                                        { "h", Close },
                                        { "u", Close }
                                    };
            finalDict.Add("B", barblockDict);

            // Cloth
            var clothDict = new Dictionary<string, double> { { "C", Close } };
            finalDict.Add("C", clothDict);

            // Finished Goods
            var finishedgoodsDict = new Dictionary<string, double> { { "G", Close }, { "¤", Close } };
            finalDict.Add("G", finishedgoodsDict);

            // Food
            var foodDict = new Dictionary<string, double>
                                    {
                                        { "D", Close },
                                        { "d", Close },
                                        { "e", Close },
                                        { "k", Close },
                                        { "q", Close }
                                    };
            finalDict.Add("D", foodDict);

            // Furniture
            var furnitureDict = new Dictionary<string, double> { { "U", Close }, { "c", Close }, { "m", Close } };
            finalDict.Add("U", furnitureDict);

            // Leather
            var leatherDict = new Dictionary<string, double> { { "L", Close } };
            finalDict.Add("L", leatherDict);

            // Stone
            var stoneDict = new Dictionary<string, double> { { "S", Close }, { "¤", Close }, { "m", Close } };
            finalDict.Add("S", stoneDict);

            // Weaponry
            var weaponryDict = new Dictionary<string, double> { { "W", Close }, { "h", Close } };
            finalDict.Add("W", weaponryDict);

            // Wood
            var woodDict = new Dictionary<string, double>
                                    {
                                        { "T", Close },
                                        { "c", Close },
                                        { "¤", Close },
                                        { "u", Close }
                                    };
            finalDict.Add("T", woodDict);

            return finalDict;
        }

        /// <summary>
        /// Converts chosen areas and number of dwarves into a dictionary.
        /// </summary>
        /// <param name="chosenAreas">The areas chosen by the user.</param>
        /// <param name="numberOfDwarves">The number of dwarves to accomodate.</param>
        /// <returns>The dictionary.</returns>
        private static Dictionary<string, int> GetRequiredAreas(IEnumerable<Area> chosenAreas, int numberOfDwarves)
        {
            var ra = new Dictionary<string, int>();

            foreach (var tempName in chosenAreas.Select(t => t.AreaName))
            {
                if (tempName.Equals("b"))
                {
                    ra.Add(tempName, (int)Math.Ceiling(numberOfDwarves / 8d));
                }
                else if (tempName.Equals("d"))
                {
                    ra.Add(tempName, (int)Math.Ceiling(numberOfDwarves / 6d));
                }
                else
                {
                    ra.Add(tempName, 1);
                }
            }

            return ra;
        }

        /// <summary>
        /// Calculates the number of rooms required.
        /// </summary>
        /// <param name="requiredAreas"> The areas that must be in the map. </param>
        /// <param name="numberOfDwarves"> The number of dwarves to take into account when generating. </param>
        /// <returns> The number of rooms required. </returns>
        private static int CalculateNumberOfRooms(List<Area> requiredAreas, int numberOfDwarves)
        {
            var rooms = 0;

            if (requiredAreas.OfType<Bedroom>().Any())
            {
                rooms += (int)Math.Ceiling(numberOfDwarves / 8d);
            }

            if (requiredAreas.OfType<DiningRoom>().Any())
            {
                rooms += (int)Math.Ceiling(numberOfDwarves / 6d);
            }

            rooms += numberOfDwarves / 2;
            return rooms;
        }
    }
}
