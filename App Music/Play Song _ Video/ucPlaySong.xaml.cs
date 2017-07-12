using App_Music.Algorithm;
using App_Music.Class;
using System;
using System.Collections.Generic;
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
    public partial class ucPlaySong : UserControl
    {
        private Song songInfo;
        private string input;
        public ucPlaySong()
        {
            InitializeComponent();

        }

        public Song SongInfo
        {
            get => songInfo;
            set
            {
                songInfo = value;
                Setting();
            }
        }

        public string Input { get => input; set => input = value; }

        public void GetLyRic()
        {
            // GetLyric: there are 2 lyric:
            // + Karaoke and Normal Lyric
            // If karaoke exists -> get it 
            // Else get Normal Lyric if exists
            if (SongInfo.TypeOfSong != TypeSong.Video)
            {
                // Check If lyric exists Karaoke:
                // wtf ?? Not Working ??? Regex.IsMatch(html, @"<lyric><![CDATA[http://lrc.nct.nixcdn.com/null]]></lyric>") == false && !Regex.IsMatch(html, @"<lyric><![CDATA[]]></lyric>")
                if (Input.Contains("<lyric><![CDATA[http://lrc.nct.nixcdn.com/null]]></lyric>") == false && Input.Contains("<lyric><![CDATA[]]></lyric>") == false)
                {
                    string pattern = "<lyric>(.*?)</lyric>";
                    string data = Manage.GetFirstValueRegex(Input, pattern).Value;
                    pattern = @"http://(.*?)]";
                    SongInfo.LinkLyric = Manage.GetFirstValueRegex(data, pattern).Value.Replace("]", "");
                    /* Lyric in NhacuaTui using RC4 and Hex to Encode with Key is:Lyr1cjust4nct
                        * So First: Download file *.lyric
                        * Second : copy data and start Decoding 
                    */

                    string HexLyric = Manage.DownloadLyric(SongInfo.LinkLyric);
                    // Change Key to byte[]
                    byte[] Lyric = RC4.HexStringToByteArray(HexLyric);
                    byte[] key = Encoding.ASCII.GetBytes("Lyr1cjust4nct");
                    // Start Decode
                    Byte[] result = RC4.Decode(key, Lyric);
                    SongInfo.Lyric = Encoding.UTF8.GetString(result);
                }
            }
        }
        void Setting()
        {
            Task t = new Task(() => { GetLyRic(); });
            t.Start();
            imgSinger.Source = new BitmapImage(new Uri(SongInfo.ImageSinger));
            tbSongName.Text = SongInfo.SongName;
            tbSingerName.Text = SongInfo.SingerName;
            PlaySong();


        }
        MediaPlayer mediaPlayer = new MediaPlayer();
        void PlaySong()
        {
            mediaPlayer.Open(new Uri((SongInfo.RealUrlDownload)));
            mediaPlayer.Play();
            if (IsPlayed == false)
                imgPlaySong.Source = new BitmapImage(new Uri(@"pack://application:,,,/Image/Play Song/play-button.png"));
            IsPlayed = true;
            imgPlaySong.PreviewMouseDown -= imgPlaySong_PreviewMouseDown;
            imgPlaySong.PreviewMouseDown += imgPlaySong_PreviewMouseDown;
            if (IsRepeat == false)
                imgRepeat.Source = new BitmapImage(new Uri(@"pack://application:,,,/Image/Play Song/repeat (1).png"));
            IsRepeat = true;           
            imgRepeat.PreviewMouseDown -= ImgRepeat_PreviewMouseDown;
            imgRepeat.PreviewMouseDown += ImgRepeat_PreviewMouseDown;
        }

        private void ImgRepeat_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsRepeat==true)
            {
                imgRepeat.Source= new BitmapImage(new Uri(@"pack://application:,,,/Image/Play Song/repeat (2).png"));
                mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
                imgRepeat.ToolTip = "Repeat";
            }
            else
            {
                imgRepeat.Source = new BitmapImage(new Uri(@"pack://application:,,,/Image/Play Song/repeat (1).png"));
                mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
                imgRepeat.ToolTip = "Not repeat";
            }
            IsRepeat = !IsRepeat;            
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Open(new Uri((SongInfo.RealUrlDownload)));
            mediaPlayer.Play();
        }

        bool IsPlayed;
        bool IsRepeat;
        // Click pause - Play Song
        private void imgPlaySong_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPlayed == true)
            {
                imgPlaySong.Source = new BitmapImage(new Uri(@"pack://application:,,,/Image/Play Song/pause (1).png"));
                mediaPlayer.Pause();
                imgPlaySong.ToolTip = "Pause";
            }
            else
            {
                imgPlaySong.Source = new BitmapImage(new Uri(@"pack://application:,,,/Image/Play Song/play-button.png"));
                mediaPlayer.Play();
                imgPlaySong.ToolTip = "Play";
            }
            IsPlayed = !IsPlayed;
        }
    }
}
