﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PCG_DFFortressGenerator.Classes;
using PCG_DFFortressGenerator.Classes.Rooms;
using PCG_DFFortressGenerator.Classes.Stockpiles;
using PCG_DFFortressGenerator.Classes.Workshops;

namespace PCG_DFFortressGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map Map { get; set; }

        /// <summary>
        /// Used to prevent "CbZLevel_OnSelectionChanged" from throwing errors.
        /// </summary>
        private bool _mapGenerated;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerateFortress_OnClick(object sender, RoutedEventArgs e)
        {
            _mapGenerated = false;

            // Creates a new map with the chosen values.
            var x = Convert.ToInt32(cbMapSizeX.Text);
            var y = Convert.ToInt32(cbMapSizeY.Text);
            var z = Convert.ToInt32(cbMapSizeZ.Text);
            Map = new Map(x, y, z, this) {CurrentZLevel = z - 1};

            // Puts the correct level values into cbZLevels
            cbZLevel.Items.Clear();
            var newZLevels = new List<string>();
            for (var zz = 1; zz <= z; zz++)
            {
                cbZLevel.Items.Add(zz);
                newZLevels.Add("" + zz);
            }
            cbZLevel.SelectedIndex = z - 1;
            Console.WriteLine(Map.CurrentZLevel);

            LayoutGenerator lg = new LayoutGenerator(Map, FindChosenAreas(), Convert.ToInt32(cbNumberOfDwarves.Text));
            Console.WriteLine(Map.ToString());

            var tempList = FindChosenAreas();
            Console.WriteLine(tempList.OfType<Stone>().Any());
            Console.WriteLine(tempList.OfType<Barracks>().Any());
            Console.WriteLine(tempList.OfType<Craftdwarf>().Any());

            tbMapDisplay.Text = Map.ToString();
            _mapGenerated = true;
        }

        /// <summary>
        /// Creates a list of all the areas the user wants to have in their fortress.
        /// </summary>
        /// <returns>A list containing one of each of the areas the user wants in their fortress.</returns>
        private List<Area> FindChosenAreas()
        {
            var chosenRooms = new List<Area> {new Entrance()};

            if (chkBarracks.IsChecked != null && chkBarracks.IsChecked.Value)
                chosenRooms.Add(new Barracks());
            if (chkBedroom.IsChecked != null && chkBedroom.IsChecked.Value)
                chosenRooms.Add(new Bedroom());
            if (chkDiningRoom.IsChecked != null && chkDiningRoom.IsChecked.Value)
                chosenRooms.Add(new DiningRoom());
            if (chkFarm.IsChecked != null && chkFarm.IsChecked.Value)
                chosenRooms.Add(new Farm());
            if (chkOffice.IsChecked != null && chkOffice.IsChecked.Value)
                chosenRooms.Add(new Office());

            if (chkBrewery.IsChecked != null && chkBrewery.IsChecked.Value)
                chosenRooms.Add(new Brewery());
            if (chkCarpenters.IsChecked != null && chkCarpenters.IsChecked.Value)
                chosenRooms.Add(new Carpenter());
            if (chkCraftdwarfs.IsChecked != null && chkCraftdwarfs.IsChecked.Value)
                chosenRooms.Add(new Craftdwarf());
            if (chkFishery.IsChecked != null && chkFishery.IsChecked.Value)
                chosenRooms.Add(new Fishery());
            if (chkKitchen.IsChecked != null && chkKitchen.IsChecked.Value)
                chosenRooms.Add(new Kitchen());
            if (chkMasons.IsChecked != null && chkMasons.IsChecked.Value)
                chosenRooms.Add(new Mason());
            if (chkMetalsmiths.IsChecked != null && chkMetalsmiths.IsChecked.Value)
                chosenRooms.Add(new Metalsmith());
            if (chkSmelter.IsChecked != null && chkSmelter.IsChecked.Value)
                chosenRooms.Add(new Smelter());
            if (chkWoodFurnace.IsChecked != null && chkWoodFurnace.IsChecked.Value)
                chosenRooms.Add(new WoodFurnace());

            if (chkBarBlock.IsChecked != null && chkBarBlock.IsChecked.Value)
                chosenRooms.Add(new BarBlock());
            if (chkCloth.IsChecked != null && chkCloth.IsChecked.Value)
                chosenRooms.Add(new Cloth());
            if (chkFinishedGoods.IsChecked != null && chkFinishedGoods.IsChecked.Value)
                chosenRooms.Add(new FinishedGoods());
            if (chkFood.IsChecked != null && chkFood.IsChecked.Value)
                chosenRooms.Add(new Food());
            if (chkFurniture.IsChecked != null && chkFurniture.IsChecked.Value)
                chosenRooms.Add(new Furniture());
            if (chkLeather.IsChecked != null && chkLeather.IsChecked.Value)
                chosenRooms.Add(new Leather());
            if (chkStone.IsChecked != null && chkStone.IsChecked.Value)
                chosenRooms.Add(new Stone());
            if (chkWeaponry.IsChecked != null && chkWeaponry.IsChecked.Value)
                chosenRooms.Add(new Weaponry());
            if (chkWood.IsChecked != null && chkWood.IsChecked.Value)
                chosenRooms.Add(new Wood());

            return chosenRooms;
        }

        /// <summary>
        /// Fires when the cbZLevel combobox's selection is changed. Uses the chosen value to
        /// print the corresponding level of the map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbZLevel_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Map == null
                || !_mapGenerated)
                return;
            
            Map.CurrentZLevel = Convert.ToInt32(cbZLevel.SelectedItem) - 1;
            tbMapDisplay.Text = Map.ToString();
        }
    }
}
