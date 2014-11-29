﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PCG_DFFortressGenerator.Classes;

namespace PCG_DFFortressGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerateFortress_OnClick(object sender, RoutedEventArgs e)
        {
            // Needs check for whether value is number or not.
            int x = Convert.ToInt32(cbMapSizeX.Text);
            int y = Convert.ToInt32(cbMapSizeY.Text);
            int z = Convert.ToInt32(cbMapSizeZ.Text);
            Map map = new Map(x, y, z);
//            Console.WriteLine(map.ToString());
        }

        private void TbNumberOfDwarves_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            tbNumberOfDwarves.Text = new String(tbNumberOfDwarves.Text.Where(c => Char.IsDigit(c)).ToArray());
            tbNumberOfDwarves.SelectionStart = tbNumberOfDwarves.Text.Length;
        }
    }
}
