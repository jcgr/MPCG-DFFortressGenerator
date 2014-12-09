namespace PCG_DFFortressGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using Classes;
    using Classes.Rooms;
    using Classes.Stockpiles;
    using Classes.Workshops;

    using PCG_DFFortressGenerator.Evolution;

    /// <summary>
    /// The window for the GUI.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the progress block.
        /// </summary>
        public static TextBlock ProgressBlock { get; set; }

        /// <summary>
        /// Gets or sets the original z level.
        /// </summary>
        private int OriginalZLevel { get; set; }

        /// <summary>
        /// Gets or sets the map in focus.
        /// </summary>
        private Map Map { get; set; }

        /// <summary>
        /// Gets or sets the list of generated maps.
        /// </summary>
        private List<Map> Maps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map has been generated.
        /// Used to prevent "OnSelectionChanged" methods from throwing errors.
        /// </summary>
        private bool MapGenerated { get; set; }

        /// <summary>
        /// Forces an update of the progress text block
        /// </summary>
        /// <param name="text">The text to update it with.</param>
        public static void UpdateProgressBlock(string text)
        {
            ProgressBlock.Text = text;

            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new DispatcherOperationCallback(
                    delegate
                    {
                        frame.Continue = false;
                        return null;
                    }),
                null);
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// Happens when the button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void BtnGenerateFortress_OnClick(object sender, RoutedEventArgs e)
        {
            this.MapGenerated = false;
            ProgressBlock = this.tbProgress;
            UpdateProgressBlock("Generation started. \n Please wait.");

            // Creates a new map with the chosen values.
            var x = Convert.ToInt32(cbMapSizeX.Text);
            var y = Convert.ToInt32(cbMapSizeY.Text);
            var z = Convert.ToInt32(cbMapSizeZ.Text);
            this.OriginalZLevel = z - 1;

            var evolver = new Evolver();
            evolver.EvolveMaps(x, y, z, this.FindChosenAreas(), Convert.ToInt32(cbNumberOfDwarves.Text));

            this.Maps = evolver.GeneratedMaps;
            this.Map = this.Maps[0];
            this.Map.CurrentZLevel = this.OriginalZLevel;

            // Puts the maps into cbGeneratedMaps
            this.cbGeneratedMaps.Items.Clear();
            for (var mapIndex = 1; mapIndex <= this.Maps.Count; mapIndex++)
                this.cbGeneratedMaps.Items.Add(mapIndex);

            this.cbGeneratedMaps.SelectedIndex = 0;

            // Puts the correct level values into cbZLevels
            cbZLevel.Items.Clear();
            for (var zz = z; zz > 0; zz--)
                this.cbZLevel.Items.Add(zz);

            this.cbZLevel.SelectedIndex = 0;
            ProgressBlock.Text = "Generation finished!";

            this.MapGenerated = true;
            this.tbMapDisplay.Text = Map.ToString();
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
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void CbZLevel_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Map == null
                || !this.MapGenerated)
                return;

            this.Map.CurrentZLevel = Convert.ToInt32(cbZLevel.SelectedItem) - 1;
            this.tbMapDisplay.Text = Map.ToString();
        }

        /// <summary>
        /// Fires when the cbGeneratedMaps combobox's selection is changed. Uses the chosen value to
        /// select the chosen map.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void CbGeneratedMaps_OnSelectionChangedbGeneratedMaps_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Maps == null
                || !this.MapGenerated)
                return;

            this.Map = this.Maps[Convert.ToInt32(this.cbGeneratedMaps.SelectedItem)];
            this.Map.CurrentZLevel = this.OriginalZLevel;
            this.cbZLevel.SelectedIndex = this.OriginalZLevel;
            this.tbMapDisplay.Text = Map.ToString();
        }
    }
}
