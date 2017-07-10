using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public static class urlCharts
    {
        public static string Us_UkUrlSong = "bai-hat/top-20.au-my.html";
        public static string Us_UkUrlVideo;
        public static string Us_UkUrlPlaylist;

        public static string V_PopUrlSong = "bai-hat/top-20.nhac-viet.html";
        public static string V_PopUrlVideo;
        public static string V_PopUrlPlaylist;

        public static string K_PopUrlSong = "bai-hat/top-20.nhac-han.html";
        public static string K_PopUrlVideo;
        public static string K_PopUrlPlaylist;



        #region Get url of 3 charts: V-pop / us-uk / k-pop

        /* Because link will be changed after a week -> firt : must specify link for every weak -> do something
         */

       public static void GetUrlCharts()
        {
            GetUrlChart(ref V_PopUrlSong, ref V_PopUrlPlaylist, ref V_PopUrlVideo);
            GetUrlChart(ref K_PopUrlSong, ref K_PopUrlPlaylist, ref K_PopUrlVideo);
            GetUrlChart(ref Us_UkUrlSong, ref Us_UkUrlPlaylist, ref Us_UkUrlVideo);
        }

       private static void GetUrlChart(ref string song, ref string playlist, ref string video)
        {
            string html = Manage.CrawlData(song);
            string pattern = @"<div class=""btn_view_select"">(.*?)</div>";
            html = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
            pattern = @"http://(.*?)html";
            var links = Manage.GetDataWithRegex(html, pattern);
            song = links[0].Value; playlist = links[1].Value; video = links[2].Value;
        }

        #endregion
    }
}
