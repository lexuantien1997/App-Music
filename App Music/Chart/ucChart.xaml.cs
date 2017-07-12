using App_Music.Algorithm;
using App_Music.Class;
using App_Music.Play_Song___Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using xNet;
namespace App_Music.Chart
{
    /// <summary>
    /// Interaction logic for ucChart.xaml
    /// </summary>
    public partial class ucChart : UserControl, INotifyPropertyChanged
    {
        public object GetControl { get; set; }
        #region Binding data with INotifyPropertyChanged - UI
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        #endregion

        List<Song> CrawlAChart(string inputLink, List<Song> lSong, TypeSong typeSong)
        {
            string html = Manage.CrawlData(inputLink);
            string pattern = @"<div class=""box_info_field"">(.*?)class=""last_weeks_position";
            var data = Manage.GetDataWithRegex(html, pattern, RegexOptions.Singleline);

            foreach (Match item in data)
            {
                Song song = GetASong(item.Value, typeSong, (lSong.Count + 1).ToString());
                lSong.Add(song);
            }
            return lSong;
        }



        Song GetASong(string input, TypeSong typeSong, string id)
        {
            Song song = new Song();
            song.ID = @"/Image/ID Chart/_" + id + ".png"; // Get id song
            song.TypeOfSong = typeSong;

            // Get urlSong - singer name - artist - image singer
            string pattern = @"""http://www.nhaccuatui(.*?)alt";
            Match data = Manage.GetFirstValueRegex(input, pattern, RegexOptions.Singleline);

            // Get url image singer and urlSong
            pattern = @"http://(.*?)""";
            MatchCollection links = Manage.GetDataWithRegex(data.Value, pattern);
            song.UrlSong = links[0].Value.Substring(0, links[0].Length - 1).Replace("http://www.nhaccuatui.com/", ""); // UrlSong           
            song.ImageSinger = links[1].Value.Substring(0, links[1].Length - 1); // ImageSinger

            // Get Artist and Singer name
            string[] value = GetSingerNameAndArtist(data.Value);
            song.SongName = value[0]; // Add song name
            song.SingerName = value[1]; // Add singer name

            // Get Highest Position
            pattern = @"></span>(.*?)</a>";
            data = Manage.GetFirstValueRegex(input, pattern);
            song.HighestPosition = data.Value.Replace("></span>", "").Replace("</a>", "");

            return song;
        }

        string[] GetSingerNameAndArtist(string input)
        {
            string pattern = @"title=""(.*?)"">";
            string result = Manage.GetDataWithRegex(input, pattern)[0].Value;
            int index = result.IndexOf("title=\"") + 7; // Plus 7 because we'll split starting from " ( > "). \: Using with special character
            result = result.Substring(index, result.Length - index - 2); // sub 2 because SongAndSinger = ...">

            string[] split = { " - " }; // Conditional to split
            string[] SongSinger = result.Split(split, StringSplitOptions.RemoveEmptyEntries);

            // If split have more than two songsinger . That mean, we have more than one condition existing in "result"
            if (SongSinger.Length > 2)
            {
                for (int i = 1; i < SongSinger.Length - 1; i++)
                    SongSinger[0] = SongSinger[0] + " - " + SongSinger[i];
                SongSinger[1] = SongSinger[SongSinger.Length - 1];
            }

            return SongSinger;
        }

        public ucChart()
        {
            InitializeComponent();


            urlCharts.GetUrlCharts();
            tggSong.IsChecked = true;
            tggVPop.IsChecked = true;
            SetMouseClickToggleButton(tggSong, ref tggPrevTypeSong);
            tggVPop_Click(null, null);
        }

        ToggleButton tggPrevCountry = null;
        ToggleButton tggPrevTypeSong = null;
        void SetMouseClickToggleButton(ToggleButton tggCurrent, ref ToggleButton tggPrevious)
        {
            if (tggPrevious != null)
                tggPrevious.IsChecked = false;
            tggPrevious = tggCurrent;
        }


        private void tggTypeOfSong_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tggCurrent = sender as ToggleButton;
            SetMouseClickToggleButton(tggCurrent, ref tggPrevTypeSong);

            if (tggVPop.IsChecked == true)
                GetLinkSpecifyChart(ListChart.V_Pop, urlCharts.V_PopUrlSong, urlCharts.V_PopUrlPlaylist, urlCharts.V_PopUrlVideo);
            else if (tggKPop.IsChecked == true)
                GetLinkSpecifyChart(ListChart.K_Pop, urlCharts.K_PopUrlSong, urlCharts.K_PopUrlPlaylist, urlCharts.K_PopUrlVideo);
            else
                GetLinkSpecifyChart(ListChart.Us_Uk, urlCharts.Us_UkUrlSong, urlCharts.Us_UkUrlPlaylist, urlCharts.Us_UkUrlVideo);
        }

        void GetLinkSpecifyChart(ChartSong chartSong, string urlSong, string urlPlaylist, string urlVideo)
        {
            if (tggSong.IsChecked == true)
            {
                if (chartSong.ListSong == null)
                {
                    chartSong.ListSong = new List<Song>();
                    chartSong.ListSong = CrawlAChart(urlSong, chartSong.ListSong, TypeSong.Song);
                }
                listBoxChart.ItemsSource = chartSong.ListSong;
            }
            else if (tggPlaylist.IsChecked == true)
            {
                if (chartSong.ListPlaylist == null)
                {
                    chartSong.ListPlaylist = new List<Song>();
                    chartSong.ListPlaylist = CrawlAChart(urlPlaylist, chartSong.ListPlaylist, TypeSong.Playlist);
                }
                listBoxChart.ItemsSource = chartSong.ListPlaylist;
            }
            else
            {
                if (chartSong.ListVideo == null)
                {
                    chartSong.ListVideo = new List<Song>();
                    chartSong.ListVideo = CrawlAChart(urlVideo, chartSong.ListVideo, TypeSong.Video);
                }
                listBoxChart.ItemsSource = chartSong.ListVideo;
            }

        }

        private void tggUSUK_Click(object sender, RoutedEventArgs e)
        {
            if (tggUSUK.IsChecked == false)
            { tggUSUK.IsChecked = true; return; }

            SetMouseClickToggleButton(tggUSUK, ref tggPrevCountry);
            GetLinkSpecifyChart(ListChart.Us_Uk, urlCharts.Us_UkUrlSong, urlCharts.Us_UkUrlPlaylist, urlCharts.Us_UkUrlVideo);
        }

        private void tggVPop_Click(object sender, RoutedEventArgs e)
        {
            if (tggVPop.IsChecked == false)
            { tggVPop.IsChecked = true; return; }

            SetMouseClickToggleButton(tggVPop, ref tggPrevCountry);
            GetLinkSpecifyChart(ListChart.V_Pop, urlCharts.V_PopUrlSong, urlCharts.V_PopUrlPlaylist, urlCharts.V_PopUrlVideo);
        }

        private void tggKPop_Click(object sender, RoutedEventArgs e)
        {
            if (tggKPop.IsChecked == false)
            { tggKPop.IsChecked = true; return; }

            SetMouseClickToggleButton(tggKPop, ref tggPrevCountry);
            GetLinkSpecifyChart(ListChart.K_Pop, urlCharts.K_PopUrlSong, urlCharts.K_PopUrlPlaylist, urlCharts.K_PopUrlVideo);
        }

        private void PlayASong_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Song song = (sender as Image).DataContext as Song;

            #region Way 1:
            //switch (song.TypeOfSong)
            //{
            //    case TypeSong.Song:
            //        song.RealUrlDownload = GetRealUrlSong(song.UrlSong);
            //        break;
            //    case TypeSong.Playlist:
            //        song.RealUrlDownload = GetRealUrlPlaylist(song.UrlSong);
            //        break;
            //    case TypeSong.Video:
            //        song.RealUrlDownload = GetRealUrlVideo(song.UrlSong);
            //        break;
            //}
            #endregion

            #region Way 2:
            // Get Speical url contains all of datas of this Song but High Quality Music
            String html = Manage.CrawlData(song.UrlSong);
            string pattern = @"http://www.nhaccuatui.com/flash(.*?)""";
            song.UrlData = Manage.GetFirstValueRegex(html, pattern).Value.Remove(0, 26).Replace("\"", "");
            // Get Real UrlSong
            html = Manage.CrawlData(song.UrlData);
            pattern = @"<location>(.*?)</location>";
            string data = Manage.GetFirstValueRegex(html, pattern,RegexOptions.Singleline).Value;
            pattern = @"http://(.*?)]";
            song.RealUrlDownload = Manage.GetFirstValueRegex(data, pattern).Value.Replace("]", "");

            // Bing data into another user control
            UIElementCollection UIControl = GetControl as UIElementCollection;
            // Lyric will be gotten in ucPlaySong 
            ucPlaySong PlaySong = UIControl[0] as ucPlaySong;
            PlaySong.Input = html;
            PlaySong.SongInfo = song;           
            #endregion


        }

        #region Get Real Url Download Song - Video - Playlist - Way 1

        /* Song and Playlist are the same kind of url:
                * It has 2 kinds:  http://f9/.../mp3 or http://aredir/.../mp3
         * But in the future : may be Song and Playlist will different -> Create 2  Function -> we can fix it easily if it chances
         * Video is deffrent from Song and Playlist
         * It's a video => Save file as *.mp4 : http://vredir/.../mp4 
         */
        private string GetRealUrlSong(string urlSong)
        {
            string result = Manage.CrawlDataInWebMobile(urlSong);

            string pattern = @"http://aredir(.*?).mp3";

            if (Regex.IsMatch(result, @"http://f9") == true)
                pattern = @"http://f9(.*?).mp3";

            string realUrlSong = Manage.GetFirstValueRegex(result, pattern, RegexOptions.Singleline).Value;

            return realUrlSong;
        }

        private string GetRealUrlPlaylist(string urlSong)
        {
            string result = Manage.CrawlDataInWebMobile(urlSong);

            string pattern = @"http://aredir(.*?).mp3";

            if (Regex.IsMatch(result, @"http://f9") == true)
                pattern = @"http://f9(.*?).mp3";

            string realUrlSong = Manage.GetFirstValueRegex(result, pattern, RegexOptions.Singleline).Value;

            return realUrlSong;
        }


        private string GetRealUrlVideo(string urlSong)
        {
            string result = Manage.CrawlDataInWebMobile(urlSong);

            string pattern = @"http://vredir(.*?).mp4";
            string realUrlSong = Manage.GetFirstValueRegex(result, pattern, RegexOptions.Singleline).Value;

            return realUrlSong;
        }

        #endregion


        //    private void lbPlaylist_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //    {
        //        Song song = (sender as ListBox).SelectedItem as Song;
        //        if (song.RealUrlDownload[song.RealUrlDownload.Length - 1]=='4')
        //            Manage.DownloadSong(song,".mp4");
        //        else
        //            Manage.DownloadSong(song, ".mp3");
        //    }


    }


    #region Convert value in UI
    public class ChangeValue : IValueConverter
    {
        private int valueChange;

        public int ValueChange { get => valueChange; set => valueChange = value; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) / valueChange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
    #endregion


}
