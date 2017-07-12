using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using App_Music.Chart;
using System.Windows.Controls.Primitives;

namespace App_Music
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window/*, INotifyPropertyChanged*/
    {

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            btnHome_Click(null,null);
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Play_Song___Video.ucPlaySong PlaySong = new Play_Song___Video.ucPlaySong();            
            gridPlaySong.Children.Add(PlaySong);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        #region Click into every toggle button in menu option


        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnHome);
        }

        private void btnTopics_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnTopics);
        }

        private void btnCharts_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnCharts);
            ucChart Charts = new ucChart();
            Charts.GetControl = gridPlaySong.Children;
            GridMain.Children.Clear();
            GridMain.Children.Add(Charts);
        }

        private void btnSongs_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnSongs);
        }

        private void btnPlaylists_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnPlaylists);
        }

        private void btnVideos_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnVideos);
        }

        private void btnArtists_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnArtists);
        }

        private void btnMusicFile_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnMusicFile);
        }

        private void btnDownloading_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnDownloading);
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnSetting);
        }

        ToggleButton tggPrevious = null;
        private void BindingMouseClick(ToggleButton tgg)
        {
            if (tggPrevious == tgg)
            {
                tgg.IsChecked = true;
                return;
            }
            if (tggPrevious != null)
                tggPrevious.IsChecked = false;
            tgg.IsChecked = true;
            tggPrevious = tgg;
        }


        #endregion


    }

    #region Convert value in UI
    public class ChangeValue : IValueConverter
    {
        private int valueChange;

        public int ValueChange { get => valueChange; set => valueChange = value; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) + valueChange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
    #endregion
}
