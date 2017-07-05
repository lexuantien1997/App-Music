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

        #region Binding data with INotifyPropertyChanged
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
            DataContext = this;
            tggVPop_Click(null, null);
        }


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


        #region Get Real Url Song - Video - Playlist

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



        #region Transfer data in website to a song

        /// <summary>
        /// Get data of a song
        /// </summary>
        /// <param name="input">string input</param>
        /// <param name="lSong">list song holds data</param>
        /// <param name="id">id song ++ </param>
        private void GetSong(string input, ObservableCollection<Song> lSong, int id, TypeSong typeSong)
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

        #region Set data when crawl in web

        /// <summary>
        /// List Song's crawled
        /// </summary>
        /// <param name="input">string html</param>
        /// <param name="lsong">list contains data</param>
        /// <param name="typeSong"> type of list song</param>
        /// <returns></returns>
        ObservableCollection<Song> CrawlListSong(string input, ObservableCollection<Song> lsong, TypeSong typeSong)
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

        #region Event Click Country
        private void tggUSUK_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(tggUSUK);

            if (ListChart.Us_Uk == null)
            {
                ListChart.Us_Uk = new Class.Chart();
                string html = Manage.CrawlData("bai-hat/top-20.au-my.tuan-27.2017.html");
                CrawlListSong(html, ListChart.Us_Uk.ListSong, TypeSong.Song);

                html = Manage.CrawlData("video/top-20.au-my.tuan-27.2017.html");
                CrawlListSong(html, ListChart.Us_Uk.ListMV, TypeSong.Video);

                html = Manage.CrawlData("playlist/top-20.au-my.tuan-27.2017.html");
                CrawlListSong(html, ListChart.Us_Uk.ListPlaylist, TypeSong.Playlist);
            }

            lbSong.ItemsSource = ListChart.Us_Uk.ListSong;
            lbMV.ItemsSource = ListChart.Us_Uk.ListMV;
            lbPlaylist.ItemsSource = ListChart.Us_Uk.ListPlaylist;

        }

        private void tggVPop_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(tggVPop);

            if (ListChart.V_Pop == null)
            {
                ListChart.V_Pop = new Class.Chart();
                string html = Manage.CrawlData("bai-hat/top-20.nhac-viet.html");
                CrawlListSong(html, ListChart.V_Pop.ListSong, TypeSong.Song);

                html = Manage.CrawlData("video/top-20.nhac-viet.tuan-27.2017.html");
                CrawlListSong(html, ListChart.V_Pop.ListMV, TypeSong.Video);

                html = Manage.CrawlData("playlist/top-20.nhac-viet.tuan-27.2017.html");
                CrawlListSong(html, ListChart.V_Pop.ListPlaylist, TypeSong.Playlist);
            }

            lbSong.ItemsSource = ListChart.V_Pop.ListSong;
            lbMV.ItemsSource = ListChart.V_Pop.ListMV;
            lbPlaylist.ItemsSource = ListChart.V_Pop.ListPlaylist;

        }

        private void tggKPop_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(tggKPop);

            if (ListChart.K_Pop == null)
            {
                ListChart.K_Pop = new Class.Chart();
                string html = Manage.CrawlData("bai-hat/top-20.nhac-han.tuan-27.2017.html");
                CrawlListSong(html, ListChart.K_Pop.ListSong, TypeSong.Song);

                html = Manage.CrawlData("video/top-20.nhac-han.tuan-27.2017.html");
                CrawlListSong(html, ListChart.K_Pop.ListMV, TypeSong.Video);

                html = Manage.CrawlData("playlist/top-20.nhac-han.tuan-27.2017.html");
                CrawlListSong(html, ListChart.K_Pop.ListPlaylist, TypeSong.Playlist);
            }

            lbSong.ItemsSource = ListChart.K_Pop.ListSong;
            lbMV.ItemsSource = ListChart.K_Pop.ListMV;
            lbPlaylist.ItemsSource = ListChart.K_Pop.ListPlaylist;
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
            return ((double)value) / valueChange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
    #endregion
}
