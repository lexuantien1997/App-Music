using App_Music.Algorithm;
using App_Music.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace App_Music.Play_Song___Video
{
    /// <summary>
    /// Interaction logic for ucPlaySong.xaml
    /// </summary>
    public partial class ucPlaySong : UserControl, INotifyPropertyChanged
    {
        //
        private string sourceImage = "/Image/Play Song/play-button.png";
        public string SourceImage
        {
            get => sourceImage;
            set
            {
                sourceImage = value;
                OnPropertyChanged("SourceImage");
            }
        }

        // Binding mediaplayer
        public static MediaElement mediaPlayer;      
        // Event
        private event EventHandler minimizeFullScreen;
        public event EventHandler MinimizeFullScreen
        {
            add
            {
                minimizeFullScreen += value;
            }

            remove
            {
                minimizeFullScreen -= value;
            }
        }
       
        // Event
        private event EventHandler playPauseChanged;
        public event EventHandler PlayPauseChanged
        {
            add
            {
                playPauseChanged += value;
            }

            remove
            {
                playPauseChanged -= value;
            }
        }


        public ucPlaySong()
        {
            InitializeComponent();
            imgPlaySong.DataContext = this;
            ucPlaySongFullScreen.imgSinger = imgSinger;
            ucPlaySongFullScreen.tbSingerName = tbSingerName;
            ucPlaySongFullScreen.tbSongName = tbSongName;
            Setting();
        }

        void Setting()
        {
            imgPlaySong.PreviewMouseDown += imgPlaySong_PreviewMouseDown;
        }

        private void imgPlaySong_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaPlayer != null)
            {
                if (playPauseChanged != null)
                {
                    playPauseChanged(imgPlaySong, new EventArgs());
                }
            }

        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaPlayer != null)
            {
                if (minimizeFullScreen != null)
                {
                    minimizeFullScreen(this, new EventArgs());
                }
            }
        }
        #region Binding data with INotifyPropertyChanged - UI
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
