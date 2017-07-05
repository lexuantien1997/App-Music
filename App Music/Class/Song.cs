using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public class Song
    {
        private string songName;
        private string singerName;
        private string urlSong;
        private string iD;
        private string realUrlDownload;

        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string UrlSong { get => urlSong; set => urlSong = value; }
        public string ID { get => iD; set => iD = value; }
        public string RealUrlDownload { get => realUrlDownload; set => realUrlDownload = value; }
       
    }
}
