using App_Music.Algorithm;
using App_Music.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace App_Music.Play_Song___Video
{
    /// <summary>
    /// Interaction logic for ucPlaySongFullScreen.xaml
    /// </summary>
    public partial class ucPlaySongFullScreen : UserControl, INotifyPropertyChanged
    {
        //
        Song CurrentSong;
        // 1
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

        // 2
        private List<Song> songInfo;
        public List<Song> SongInfo
        {
            get => songInfo;
            set
            {
                songInfo = value;
                CurrentSong = SongInfo[0];
                Setting();
            }
        }

        // Binding image - text block
        public static Image imgSinger;
        public static TextBlock tbSingerName;
        public static TextBlock tbSongName;
        // Using mediaplayer to play a song
        // MediaPlayer mediaPlayer = new MediaPlayer();

        // event
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

        // event
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
        // 
        bool IsDragging = false;
        DispatcherTimer timer;
        public ucPlaySongFullScreen()
        {
            InitializeComponent();
            imgPlayPause.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }


        private void GetLyRic(string html)
        {
            if (CurrentSong.TypeOfSong != TypeSong.Video)
            {
                GetKaraokeLyric(html);
                GetNormalLyric();
                rbKaraokeLyric_Click(rbKaraokeLyric, null);
            }
        }

        void GetKaraokeLyric(string html)
        {
            if (CurrentSong.KaraokeLyric == null)
            {
                // GetLyric: there are 2 lyric:
                // + Karaoke and Normal Lyric
                // If karaoke exists -> get it 
                // Else get Normal Lyric if exists
                if (CurrentSong.TypeOfSong != TypeSong.Video) // get karaoke lyric
                {
                    // Check If lyric exists Karaoke:
                    //if (Regex.IsMatch(html, "<lyric><![CDATA[http://lrc.nct.nixcdn.com/null]]></lyric>", RegexOptions.Singleline) == true || Regex.IsMatch(html, "<lyric><![CDATA[]]></lyric>", RegexOptions.Singleline) == true)
                    if (html.Contains("<lyric><![CDATA[http://lrc.nct.nixcdn.com/null]]></lyric>") || html.Contains("<lyric><![CDATA[]]></lyric>"))
                        return;
                    string pattern = "<lyric>(.*?)</lyric>";
                    string data = Manage.GetFirstValueRegex(html, pattern).Value;
                    pattern = @"http://(.*?)]";
                    CurrentSong.LinkLyric = Manage.GetFirstValueRegex(data, pattern).Value.Replace("]", "");
                    /* Lyric in NhacuaTui using RC4 and Hex to Encode with Key is:Lyr1cjust4nct
                        * So First: Download file *.lyric
                        * Second : copy data and start Decoding 
                    */

                    string HexLyric = Manage.DownloadLyric(CurrentSong.LinkLyric);
                    // Change Key to byte[]
                    byte[] Lyric = RC4.HexStringToByteArray(HexLyric);
                    byte[] key = Encoding.ASCII.GetBytes("Lyr1cjust4nct");
                    // Start Decode
                    Byte[] result = RC4.Decode(key, Lyric);
                    CurrentSong.KaraokeLyric = Encoding.UTF8.GetString(result);

                }
            }
        }

        void GetNormalLyric()
        {
            if (CurrentSong.Lyric == null)
            {
                string html = Manage.GetNormalLyric(CurrentSong.UrlSong);
                string pattern = @"id=""divLyric""(.*?)</p>";
                html = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
                pattern = @"<br />(.*?)</p>";
                html = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
                CurrentSong.Lyric = html.Replace("<br />", "").Replace("</p>", "");
            }
        }
        /// <summary>
        /// Get big size image
        /// </summary>
        void GetBigImage(string input)
        {
            if (CurrentSong.ImageSingerBigSize == null)
            {
                // Get url singer
                string pattern = @"<newtab>(.*?)</newtab>";
                string html = Manage.GetFirstValueRegex(input, pattern, RegexOptions.Singleline).Value;
                pattern = @"com/(.*?)html";
                html = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
                html = html.Replace("com/", "");

                // Get Big image
                if (html == "")
                    CurrentSong.ImageSingerBigSize = @"/Image/Play Song/i_love_music_2-wallpaper-2880x1800.jpg";
                else
                {
                    string data = Manage.CrawlData(html);
                    pattern = @"image_src(.*?)/>";
                    data = Manage.GetFirstValueRegex(data, pattern, RegexOptions.Singleline).Value;
                    pattern = @"http(.*?)""";
                    data = Manage.GetFirstValueRegex(data, pattern, RegexOptions.Singleline).Value;
                    CurrentSong.ImageSingerBigSize = data.Replace(@"""", "");
                }

            }
        }
        private void Setting()
        {
            string html = GetRealUrlDownload();
            // Check Tyesong
            if (CurrentSong.TypeOfSong == TypeSong.Video)
            {
                mediaPlayer.Visibility = System.Windows.Visibility.Visible; // Show Video
                lbLyric.Visibility = System.Windows.Visibility.Hidden;
                rbKaraokeLyric.Click -= rbKaraokeLyric_Click;
                rbNormalLyric.Click -= rbNormalLyric_Click;
            }
            else
            {
                lbLyric.Visibility = System.Windows.Visibility.Visible;
                mediaPlayer.Visibility = System.Windows.Visibility.Hidden; // Hide Video
                rbKaraokeLyric.Click += rbKaraokeLyric_Click;
                rbNormalLyric.Click += rbNormalLyric_Click;
            }
            // Binding from another user control
            ucPlaySong.mediaPlayer = mediaPlayer;
            MainWindow.mediaPlayer = mediaPlayer;

            // Binding data image from another user cotrol
            imgSinger.Source = new BitmapImage(new Uri(CurrentSong.ImageSinger));
            tbSingerName.Text = CurrentSong.SingerName;
            tbSongName.Text = CurrentSong.SongName;
            // Get Big Image
            GetBigImage(html);
            // asyn Get Lyric
            //Task t = new Task(() => { GetLyRic(html); });
            //t.Start();
            GetLyRic(html);
            // Bingding data context
            DataContext = CurrentSong;

            //
            lbListSong.ItemsSource = SongInfo;
            // Setting media
            mediaPlayer.LoadedBehavior = MediaState.Manual;
            mediaPlayer.Source = new Uri(CurrentSong.RealUrlDownload);
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.Play();
        }

        private string GetRealUrlDownload()
        {
            // Get Speical url contains all of datas of this Song but High Quality Music
            String html = Manage.CrawlData(CurrentSong.UrlSong);
            string pattern = @"http://www.nhaccuatui.com/flash(.*?)""";
            CurrentSong.UrlData = Manage.GetFirstValueRegex(html, pattern).Value.Remove(0, 26).Replace("\"", "");
            // Get Real UrlSong
            html = Manage.CrawlData(CurrentSong.UrlData);
            pattern = @"<location>(.*?)</location>";
            string data = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
            string patternn = @"http://(.*?)]";
            CurrentSong.RealUrlDownload = Manage.GetFirstValueRegex(data, patternn).Value.Replace("]", "");
            return html;
        }

        #region Media setting
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            SliderTimeLine.Value = 0;
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                SliderTimeLine.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                tbDuration.Text = new TimeSpan(0, (int)(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds / 60), (int)(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds % 60)).ToString(@"mm\:ss");
                SliderTimeLine.SmallChange = 1;
            }
            timer.Start();
        }

        #endregion


        /// <summary>
        /// Event minimize control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Click play - pause a song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgPlayPause_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (playPauseChanged != null)
            {
                playPauseChanged(imgPlayPause, new EventArgs());
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


        #region Slider Song

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsDragging)
            {
                SliderTimeLine.Value = mediaPlayer.Position.TotalSeconds;
                tbPosition.Text = new TimeSpan(0, (int)(SliderTimeLine.Value / 60), (int)(SliderTimeLine.Value % 60)).ToString(@"mm\:ss");

            }
        }
        private void SliderTimeLine_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsDragging = true;
        }

        private void SliderTimeLine_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsDragging = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(SliderTimeLine.Value);
            tbPosition.Text = new TimeSpan(0, (int)(mediaPlayer.Position.TotalSeconds / 60), (int)(mediaPlayer.Position.TotalSeconds % 60)).ToString(@"mm\:ss");

        }

        private void SliderTimeLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(SliderTimeLine.Value);
            tbPosition.Text = new TimeSpan(0, (int)(mediaPlayer.Position.TotalSeconds / 60), (int)(mediaPlayer.Position.TotalSeconds % 60)).ToString(@"mm\:ss");
        }

        #endregion

        private void rbListSong_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (rbListSong.IsChecked == true)
            {
                cvCircle.Visibility = System.Windows.Visibility.Hidden;
                gridListSong.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void rbSingerImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (rbSingerImage.IsChecked == true)
            {
                cvCircle.Visibility = System.Windows.Visibility.Visible;
                gridListSong.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CurrentSong = (sender as TextBlock).DataContext as Song;
            Setting();
        }

        private void rbNormalLyric_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (rbNormalLyric.IsChecked == true && CurrentSong.Lyric != null)
                lbLyric.Text = CurrentSong.Lyric;
        }

        private void rbKaraokeLyric_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (rbNormalLyric.IsChecked == true)
                rbKaraokeLyric.IsChecked = true;

            if (rbKaraokeLyric.IsChecked == true && CurrentSong.KaraokeLyric != null)
                lbLyric.Text = CurrentSong.KaraokeLyric;
        }
    }
}
