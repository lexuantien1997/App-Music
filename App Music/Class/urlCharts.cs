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
        public static string Us_UkUrlVideo = "video/top-20.au-my.tuan-";
        public static string Us_UkUrlPlaylist = "playlist/top-20.au-my.tuan-";

        public static string V_PopUrlSong = "bai-hat/top-20.nhac-viet.html";
        public static string V_PopUrlVideo = "video/top-20.nhac-viet.tuan-";
        public static string V_PopUrlPlaylist = "playlist/top-20.nhac-viet.tuan-";

        public static string K_PopUrlSong = "bai-hat/top-20.nhac-han.html";
        public static string K_PopUrlVideo = "video/top-20.nhac-han.tuan-";
        public static string K_PopUrlPlaylist = "playlist/top-20.nhac-han.tuan-";



        #region Get url of 3 charts: V-pop / us-uk / k-pop

        /* Because link will be changed after a week -> firt : must specify link for every weak -> do something
         */

        public static void GetUrlCharts()
        {
            if (V_PopUrlVideo == "video/top-20.nhac-viet.tuan-" || V_PopUrlPlaylist == "playlist/top-20.nhac-viet.tuan-")
                GetUrlChart(ref V_PopUrlSong, ref V_PopUrlPlaylist, ref V_PopUrlVideo);
            if (K_PopUrlVideo == "video/top-20.nhac-han.tuan-" || K_PopUrlPlaylist == "playlist/top-20.nhac-han.tuan-")
                GetUrlChart(ref K_PopUrlSong, ref K_PopUrlPlaylist, ref K_PopUrlVideo);
            if (Us_UkUrlVideo == "video/top-20.au-my.tuan-" || Us_UkUrlPlaylist == "playlist/top-20.au-my.tuan-")
                GetUrlChart(ref Us_UkUrlSong, ref Us_UkUrlPlaylist, ref Us_UkUrlVideo);
        }

        private static void GetUrlChart(ref string song, ref string playlist, ref string video)
        {
            string html = Manage.CrawlData(song);
            //string pattern = @"<div class=""btn_view_select"">(.*?)</div>";
            //html = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
            //pattern = @"http://(.*?)html";
            //var links = Manage.GetDataWithRegex(html, pattern);
            string pattern = @"<h2><strong>(.*?)</strong>";
            html = Manage.GetFirstValueRegex(html, pattern, RegexOptions.Singleline).Value;
            html = html.Replace("<h2><strong>Tuần ", "").Replace("</strong>", "") + "." + DateTime.Now.Year + ".html";
            /*song += html;*/
            playlist += html; video += html;
        }

        #endregion
    }
}
