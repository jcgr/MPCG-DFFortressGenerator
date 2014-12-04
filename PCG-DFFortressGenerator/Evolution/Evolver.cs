﻿using PCG_DFFortressGenerator.Classes.Stockpiles;

namespace PCG_DFFortressGenerator.Evolution
{
    using System;
    using System.Collections.Generic;
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
        public const int NumberOfGenerations = 20;

        /// <summary>
        /// The number of different layouts are used in evolution.
        /// </summary>
        public const int NumberOfLayoutsToGenerate = 5;

        /// <summary>
        /// The penalty to apply to a fitness of a layout if it does not contain the required rooms.
        /// </summary>
        public const double MissingRoomPenalty = 10;

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
            this.GeneratedMaps = new List<Map>();
            AreaWeights = GenerateAreaWeights();

            var numberOfRoomsRequired = this.CalculateNumberOfRooms(chosenAreas, numberOfDwarves);
            var lg = new LayoutGenerator(x, y, z, numberOfRoomsRequired);

            for (var i = 0; i < NumberOfLayoutsToGenerate; i++)
            {
                this.GeneratedMaps[i] = lg.GenerateNewLayout();
                this.GeneratedMaps[i].CalculateDistancesBetweenRooms();
            }

            this.RequiredAreas = new Dictionary<string, int>(); // TODO: Extract area names from chosenAreas (Done? Melnyk, check)
            for (var i = 0; i < chosenAreas.Count; i++)
            {
                var tempName = chosenAreas[i].AreaName;
                if (tempName.Equals("b"))
                    RequiredAreas.Add(tempName, (int)Math.Ceiling(numberOfDwarves / 8d));
                else if (tempName.Equals("d"))
                    RequiredAreas.Add(tempName, (int)Math.Ceiling(numberOfDwarves / 6d));
                else
                    RequiredAreas.Add(tempName, 1);
            }

            // TODO: Do evolution loop
        }

        /// <summary>
        /// Gets the weights between areas.
        /// </summary>
        public static Dictionary<string, Dictionary<string, double>> AreaWeights { get; private set; }

        /// <summary>
        /// Gets the maps generated by the evolver.
        /// </summary>
        public List<Map> GeneratedMaps { get; private set; }

        /// <summary>
        /// Gets or sets the required rooms for the layout.
        /// </summary>
        public Dictionary<string, int> RequiredAreas { get; set; }

        /// <summary>
        /// The generate area weights.
        /// </summary>
        /// <returns> The <see cref="Dictionary"/> containing the weights between areas. </returns>
        private static Dictionary<string, Dictionary<string, double>> GenerateAreaWeights()
        {
            var finalDict = new Dictionary<string, Dictionary<string, double>>();
            const double close = 1d;
            const double far = -1d;

            // ----------
            // Rooms
            // ----------
            // Barracks
            var barrackDict = new Dictionary<string, double>
            {
                {"r", close}, 
                {"b", far}, 
                {"d", far}, 
                {"@", close}
            };
            finalDict.Add("r", barrackDict);

            // Bedroom
            var bedroomDict = new Dictionary<string, double>
            {
                {"b", close}, 
                {"r", far}, 
                {"@", close}
            };
            finalDict.Add("b", bedroomDict);

            // Dining Room
            var diningroomDict = new Dictionary<string, double>
            {
                { "d", close }, 
                { "r", far }, 
                { "@", far }
            };
            finalDict.Add("d", diningroomDict);

            // Entrance
            var entranceDict = new Dictionary<string, double>
            {
                { "@", close }, 
                { "r", close }, 
                { "b", far }, 
                { "d", far }
            };
            finalDict.Add("@", entranceDict);

            // Farm
            var farmDict = new Dictionary<string, double>
            {
                {"f", close}
            };
            finalDict.Add("f", farmDict);

            // Office
            var officeDict = new Dictionary<string, double>
            {
                {"o", close}
            };
            finalDict.Add("o", officeDict);

            // ----------
            // Workshops
            // ----------
            // Brewery
            var breweryDict = new Dictionary<string, double>
            {
                {"q", close}, 
                {"D", close}
            };
            finalDict.Add("q", breweryDict);

            // Carpenter
            var carpenterDict = new Dictionary<string, double>
            {
                {"c", close}, 
                {"U", close}, 
                {"T", close}
            };
            finalDict.Add("c", carpenterDict);

            // Craftdwarf
            var craftdwarfDict = new Dictionary<string, double>
            {
                {"¤", close}, 
                {"G", close}, 
                {"S", close}, 
                {"T", close}
            };
            finalDict.Add("¤", craftdwarfDict);

            // Fishery
            var fisheryDict = new Dictionary<string, double>
            {
                {"e", close}, 
                {"D", close}
            };
            finalDict.Add("e", fisheryDict);

            // Kitchen
            var kitchenDict = new Dictionary<string, double>
            {
                {"k", close}, 
                {"D", close}
            };

            // Mason
            var masonDict = new Dictionary<string, double>
            {
                {"m", close}, 
                {"U", close}, 
                {"S", close}
            };
            finalDict.Add("m", masonDict);

            // Metalsmith
            var metalsmithDict = new Dictionary<string, double>
            {
                {"h", close}, 
                {"B", close}, 
                {"W", close}
            };
            finalDict.Add("h", metalsmithDict);

            // Smelter
            var smelterDict = new Dictionary<string, double>
            {
                {"s", close}, 
                {"u", close}, 
                {"B", close}
            };
            finalDict.Add("s", smelterDict);

            // Wood Furnace
            var woodfurnaceDict = new Dictionary<string, double>
            {
                {"u", close},
                {"s", close},
                {"B", close},
                {"T", close}
            };
            finalDict.Add("u", woodfurnaceDict);

            // ----------
            // Stockpiles
            // ----------
            // BarBlock
            var barblockDict = new Dictionary<string, double>
            {
                {"B", close}, 
                {"s", close}, 
                {"h", close}, 
                {"u", close}
            };
            finalDict.Add("B", barblockDict);

            // Cloth
            var clothDict = new Dictionary<string, double>
            {
                {"C", close}
            };
            finalDict.Add("C", clothDict);

            // Finished Goods
            var finishedgoodsDict = new Dictionary<string, double>
            {
                {"G", close}, 
                {"¤", close}
            };
            finalDict.Add("G", finishedgoodsDict);

            // Food
            var foodDict = new Dictionary<string, double>
            {
                {"D", close},
                {"d", close},
                {"e", close},
                {"k", close},
                {"q", close}
            };
            finalDict.Add("D", foodDict);

            // Furniture
            var furnitureDict = new Dictionary<string, double>
            {
                {"U", close}, 
                {"c", close}, 
                {"m", close}
            };
            finalDict.Add("U", furnitureDict);

            // Leather
            var leatherDict = new Dictionary<string, double>
            {
                {"L", close}
            };
            finalDict.Add("L", leatherDict);

            // Stone
            var stoneDict = new Dictionary<string, double>
            {
                {"S", close}, 
                {"¤", close}, 
                {"m", close}
            };
            finalDict.Add("S", stoneDict);

            // Weaponry
            var weaponryDict = new Dictionary<string, double>
            {
                {"W", close}, 
                {"h", close}
            };
            finalDict.Add("W", weaponryDict);

            // Wood
            var woodDict = new Dictionary<string, double>
            {
                {"T", close}, 
                {"c", close}, 
                {"¤", close}, 
                {"u", close}
            };
            finalDict.Add("T", woodDict);

            // TODO: Write area weights in code (Done? Melnyk, check)
            return finalDict;
        }

        /// <summary>
        /// Calculates the number of rooms required.
        /// </summary>
        /// <param name="requiredAreas"> The areas that must be in the layout. </param>
        /// <param name="numberOfDwarves"> The number of dwarves to take into account when generating. </param>
        /// <returns> The number of rooms required. </returns>
        private int CalculateNumberOfRooms(List<Area> requiredAreas, int numberOfDwarves)
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
