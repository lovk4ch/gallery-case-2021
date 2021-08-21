using System.Windows;

namespace gallery_case_2021
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainClick(object sender, RoutedEventArgs e)
        {
            player1_data.SetData();
            player2_data.SetData();
        }
    }
}