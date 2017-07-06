using App_Music.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace App_Music.Chart
{
    /// <summary>
    /// Interaction logic for ucChart.xaml
    /// </summary>
    public partial class ucChart : UserControl, INotifyPropertyChanged
    {

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

        public ucChart()
        {
            InitializeComponent();
            ListChart.V_Pop.ListSong = new List<Song>();
            string html = Manage.CrawlData(urlCharts.urlSongV_Pop);
            lbMain.ItemsSource = CrawlListSong(html, ListChart.V_Pop.ListSong, TypeSong.Song);

            // BindingMouseClick(ref tggSong, ref tggPreviousTypeSong);
            // tggVPop_Click(null, null);

        }

        private void tggSong_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(ref tggSong, ref tggPreviousTypeSong);
            CheckClickInTypeSong();
        }

        private void tggPlaylist_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(ref tggPlaylist, ref tggPreviousTypeSong);
            CheckClickInTypeSong();
        }

        private void tggMV_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick( ref tggMV, ref tggPreviousTypeSong);
            CheckClickInTypeSong();
        }

       private void CheckClickInTypeSong()
        {
            if (tggVPop.IsChecked == true)
                ChooseConditional2CrawlData(ListChart.V_Pop, urlCharts.urlSongV_Pop, urlCharts.urlVideoV_Pop, urlCharts.urlPlaylistV_Pop);
            else if (tggKPop.IsChecked == true)
                ChooseConditional2CrawlData(ListChart.K_Pop, urlCharts.urlSongK_Pop, urlCharts.urlVideoK_Pop, urlCharts.urlPlaylistK_Pop);
            else
                ChooseConditional2CrawlData(ListChart.Us_Uk, urlCharts.urlSongUs_Uk, urlCharts.urlVideoUs_Uk, urlCharts.urlPlaylistUs_Uk);
        }

        #region Event Click Country
        private void ChooseConditional2CrawlData(ChartSong chartSong, string urlSong, string urlVideo, string urlPlaylist)
        {
            if (tggSong.IsChecked == true)
            {
                if (chartSong.ListSong == null)
                {
                    chartSong.ListSong = new List<Song>();
                    string html = Manage.CrawlData(urlSong);
                    lbMain.ItemsSource = CrawlListSong(html, chartSong.ListSong, TypeSong.Song);
                }
                else
                    lbMain.ItemsSource = chartSong.ListSong;

            }
            else if (tggPlaylist.IsChecked == true)
            {
                if (chartSong.ListPlaylist == null)
                {
                    chartSong.ListPlaylist = new List<Song>();
                    string html = Manage.CrawlData(urlPlaylist);
                    lbMain.ItemsSource = CrawlListSong(html, chartSong.ListPlaylist, TypeSong.Playlist);
                }
                else
                    lbMain.ItemsSource = chartSong.ListPlaylist;
            }
            else if(tggMV.IsChecked==true)
            {
                if (chartSong.ListMV == null)
                {
                    chartSong.ListMV = new List<Song>();
                    string html = Manage.CrawlData(urlVideo);
                    lbMain.ItemsSource = CrawlListSong(html, chartSong.ListMV, TypeSong.Video);
                }
                else
                    lbMain.ItemsSource = chartSong.ListMV;
            }
        }


        private void tggUSUK_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(ref tggUSUK, ref tggPreviousCountry);
            ChooseConditional2CrawlData(ListChart.Us_Uk, urlCharts.urlSongUs_Uk, urlCharts.urlVideoUs_Uk, urlCharts.urlPlaylistUs_Uk);

        }

        private void tggVPop_Click(object sender,  RoutedEventArgs e)
        {
            BindingMouseClick(ref tggVPop, ref tggPreviousCountry);
            ChooseConditional2CrawlData(ListChart.V_Pop, urlCharts.urlSongV_Pop , urlCharts.urlVideoV_Pop, urlCharts.urlPlaylistV_Pop);
          
        }

        private void tggKPop_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(ref tggKPop, ref tggPreviousCountry);
            ChooseConditional2CrawlData(ListChart.K_Pop, urlCharts.urlSongK_Pop, urlCharts.urlVideoK_Pop, urlCharts.urlPlaylistK_Pop);          
        }

        #endregion




        #region Binding Mouse click
        ToggleButton tggPreviousCountry = null;
        ToggleButton tggPreviousTypeSong = null;
        private void BindingMouseClick(ref ToggleButton tgg, ref ToggleButton tggPrev)
        {
            if (tggPrev == tgg)
            {
                tgg.IsChecked = true;
                return;
            }
            if (tggPrev != null)
                tggPrev.IsChecked = false;
            tgg.IsChecked = true;
            tggPrev = tgg;
        }
        #endregion

        #region Set data when crawl in web

        /// <summary>
        /// List Song's crawled
        /// </summary>
        /// <param name="input">string html</param>
        /// <param name="lsong">list contains data</param>
        /// <param name="typeSong"> type of list song</param>
        /// <returns></returns>
        List<Song> CrawlListSong(string input, List<Song> lsong, TypeSong typeSong)
        {
            // The query of Regex
            string pattern = @"<div class=""box_info_field"">(.*?)"">";

            MatchCollection chart = Manage.GetDataWithRegex(input, pattern, RegexOptions.Singleline);

            for (int i = 0; i < Manage.CountSong; i++)
            {
                GetSong(chart[i].ToString(), lsong, i, typeSong);
            }
            return lsong;
        }

        #endregion

        #region Transfer data in website to a song

        /// <summary>
        /// Get data of a song
        /// </summary>
        /// <param name="input">string input</param>
        /// <param name="lSong">list song holds data</param>
        /// <param name="id">id song ++ </param>
        private void GetSong(string input, List<Song> lSong, int id, TypeSong typeSong)
        {
            Song song = new Song(); // Create a song

            song.ID = SetId(id); // Add id

            // Get url song
            string pattern = @"com/(.*?)""";
            string result = Manage.GetDataWithRegex(input, pattern)[0].Value;
            int index = result.IndexOf("com/") + 4; // Plus 4 because we'll split starting from / + 1 ( > /) 
            song.UrlSong = result.Substring(index, result.Length - index - 1); // Add url song

            song.RealUrlDownload = GetUrlByTypeSong(song.UrlSong, typeSong); // Add real url song/playlist/video

            // Get Singer name and song name
            pattern = @"title=""(.*?)"">";
            result = Manage.GetDataWithRegex(input, pattern)[0].Value;
            index = result.IndexOf("title=\"") + 7; // Plus 7 because we'll split starting from " ( > "). \: Using with special character
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

            song.SongName = SongSinger[0]; // Add song name
            song.SingerName = SongSinger[1]; // Add singer name

            lSong.Add(song); // Add a song into list<song>
        }

        #endregion

        /// <summary>
        /// Add 0 into id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string SetId(int id)
        {
            if (id + 1 < 10)
                return "0" + (id + 1).ToString();
            return (id + 1).ToString();
        }

        /// <summary>
        /// Function will select a function suitable for a Song based on TypeSong
        /// </summary>
        /// <param name="FakeUrl">Url song</param>
        /// <param name="typeSong"> type song in Enum TypeSong</param>
        /// <returns></returns>
        private string GetUrlByTypeSong(string FakeUrl, TypeSong typeSong)
        {
            switch (typeSong)
            {
                case TypeSong.Song:
                    return GetRealUrlSong(FakeUrl);
                case TypeSong.Playlist:
                    return GetRealUrlPlaylist(FakeUrl);
                case TypeSong.Video:
                    return GetRealUrlVideo(FakeUrl);
            }
            return "";
        }

        #region Get Real Url Download Song - Video - Playlist

        /* Song and Playlist are the same kind of url:
                * It has 2 kinds:  http://f9/.../mp3 or http://aredir/.../mp3
         * But in the future : may be Song and Playlist will different -> Create 2  Function -> we can fix it easily if it chances
         * Video is deffrent from Song and Playlist
         * It's a video => Save file as *.mp4 : http://vredir/.../mp4 
         */
        private string GetRealUrlSong(string FakeUrl)
        {
            string result = Manage.CrawlDataInWebMobile(FakeUrl);

            string pattern = @"http://aredir(.*?).mp3";

            if (Regex.IsMatch(result, @"http://f9") == true)
                pattern = @"http://f9(.*?).mp3";

            string realUrlSong = Manage.GetFirstValueRegex(result, pattern, RegexOptions.Singleline).Value;

            return realUrlSong;
        }

        private string GetRealUrlPlaylist(string FakeUrl)
        {
            string result = Manage.CrawlDataInWebMobile(FakeUrl);

            string pattern = @"http://aredir(.*?).mp3";

            if (Regex.IsMatch(result, @"http://f9") == true)
                pattern = @"http://f9(.*?).mp3";

            string realUrlSong = Manage.GetFirstValueRegex(result, pattern, RegexOptions.Singleline).Value;

            return realUrlSong;
        }


        private string GetRealUrlVideo(string FakeUrl)
        {
            string result = Manage.CrawlDataInWebMobile(FakeUrl);

            string pattern = @"http://vredir(.*?).mp4";
            string realUrlSong = Manage.GetFirstValueRegex(result, pattern, RegexOptions.Singleline).Value;

            return realUrlSong;
        }



        #endregion



        private void lbPlaylist_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Song song = (sender as ListBox).SelectedItem as Song;
            if (song.RealUrlDownload[song.RealUrlDownload.Length - 1]=='4')
                Manage.DownloadSong(song,".mp4");
            else
                Manage.DownloadSong(song, ".mp3");
        }

        
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
