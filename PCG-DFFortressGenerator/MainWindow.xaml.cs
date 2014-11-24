using System;
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
            int x = Convert.ToInt32(tbMapSizeX.Text);
            int y = Convert.ToInt32(tbMapSizeY.Text);
            int z = Convert.ToInt32(tbMapSizeZ.Text);
            Map map = new Map(x, y, z);
            Console.WriteLine(map.ToString());
        }

        private void TbMapSizeX_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            tbMapSizeX.Text = new String(tbMapSizeX.Text.Where(c => Char.IsDigit(c)).ToArray());
            tbMapSizeX.SelectionStart = tbMapSizeX.Text.Length;
        }

        private void TbMapSizeY_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            tbMapSizeY.Text = new String(tbMapSizeY.Text.Where(c => Char.IsDigit(c)).ToArray());
            tbMapSizeY.SelectionStart = tbMapSizeY.Text.Length;
        }

        private void TbMapSizeZ_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            tbMapSizeZ.Text = new String(tbMapSizeZ.Text.Where(c => Char.IsDigit(c)).ToArray());
            tbMapSizeZ.SelectionStart = tbMapSizeZ.Text.Length;
        }
    }
}
