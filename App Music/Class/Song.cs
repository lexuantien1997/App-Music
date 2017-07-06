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
        private string lyric;
        private string imageSinger;
        private string listenView;

        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string UrlSong { get => urlSong; set => urlSong = value; }
        public string ID { get => iD; set => iD = value; }
        public string RealUrlDownload { get => realUrlDownload; set => realUrlDownload = value; }
        public string Lyric { get => lyric; set => lyric = value; }
        public string ImageSinger { get => imageSinger; set => imageSinger = value; }
        public string ListenView { get => listenView; set => listenView = value; }
    }
}
