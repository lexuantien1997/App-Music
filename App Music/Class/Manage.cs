using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public static class Manage
    {
        private static int countSong=20;

        private static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://www.nhaccuatui.com/")           
        };

        private static HttpClient client1 = new HttpClient()
        {
            BaseAddress = new Uri("http://m.nhaccuatui.com/")
        };

        public static int CountSong { get => countSong; set => countSong = value; }

        /// <summary>
        /// Crawl data using Http Client
        /// </summary>
        /// <param name="url">Location of website</param>
        /// <returns></returns>
        public static string CrawlData(string url)
        {
            string html = client.GetStringAsync(url).Result;
            return html;
        }

        public static string CrawlDataInWebMobile(string url)
        {
            string html = client1.GetStringAsync(url).Result;
            return html;
        }

        /// <summary>
        /// Get data with Regex
        /// </summary>
        /// <param name="input">The input string to search with Regex</param>
        /// <param name="pattern">The input (Query) of Regex</param>
        /// <param name="rexOption">Regex Option using</param>
        /// <returns></returns>
        public static MatchCollection GetDataWithRegex(string input,string pattern,RegexOptions rexOption=RegexOptions.None)
        {
            var courseList = Regex.Matches(input,pattern,rexOption);
            return courseList;
        }

        public static Match GetFirstValueRegex(string input, string pattern, RegexOptions rexOption = RegexOptions.None)
        {
            var courseList = Regex.Match(input, pattern, rexOption);
            return courseList;
        }
    }
       
}
