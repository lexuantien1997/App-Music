using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using App_Music.Chart;
using System.Windows.Controls.Primitives;
using App_Music.Play_Song___Video;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace App_Music
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window/*, INotifyPropertyChanged*/
    {
        public static MediaElement mediaPlayer;
        ucPlaySongFullScreen PlaySongFull = new ucPlaySongFullScreen();
        ucPlaySong PlaySong = new ucPlaySong();
        public MainWindow()
        {
            InitializeComponent();
            // add user control
            PlaySong.MinimizeFullScreen += PlaySong_MinimizeFullScreen;
            PlaySong.PlayPauseChanged += PlaySongFull_PlayPauseChanged;
            //  ucPlaySong.grid = gridApp;
            gridPlaySong.Children.Add(PlaySong);
            // add user control
            PlaySongFull.MinimizeFullScreen += PlaySongFull_MinimizeFullScreen;
            PlaySongFull.PlayPauseChanged += PlaySongFull_PlayPauseChanged; ;
            gridApp.Children.Add(PlaySongFull);
            gridApp.Children[1].Visibility = Visibility.Hidden;
            btnHome_Click(null, null);
        }

      

        #region Binding button play - pause
        bool IsPlayed = true;
        private void PlaySongFull_PlayPauseChanged(object sender, EventArgs e)
        {
            if (IsPlayed == true)
            {
                mediaPlayer.Pause();
                PlaySong.SourceImage = "/Image/Play Song/pause (1).png";
                PlaySongFull.SourceImage = "/Image/Play Song/pause (1).png";
            }
            else
            {
                mediaPlayer.Play();
                PlaySong.SourceImage = "/Image/Play Song/play-button.png";
                PlaySongFull.SourceImage = "/Image/Play Song/play-button.png";
            }
            IsPlayed = !IsPlayed;
        }

        #endregion

        #region Binding minimize 
        private void PlaySongFull_MinimizeFullScreen(object sender, EventArgs e)
        {
            gridApp.Children[1].Visibility = Visibility.Hidden;
            gridApp.Children[0].Visibility = Visibility.Visible;
        }

        private void PlaySong_MinimizeFullScreen(object sender, EventArgs e)
        {
            gridApp.Children[0].Visibility = Visibility.Hidden;
            gridApp.Children[1].Visibility = Visibility.Visible;
        }
        #endregion

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
            Charts.GetControl = gridApp.Children;
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
