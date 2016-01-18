using System;
using System.Windows;

namespace Anilinkz_Player
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

        private void BTNstart_Click(object sender, RoutedEventArgs e)
        {
            Classes.Page.GetData(10);
        }
    }
}
