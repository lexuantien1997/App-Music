using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public class Song
    {
        private string iD=null;

        private string imageSinger;
        private string imageSingerBigSize=null;
        // name
        private string songName;
        private string singerName;

        private string highestPosition=null;
        // Lyric
        private string lyric=null;
        private string karaokeLyric = null;
        private string linkLyric;

        private bool isFavorited=false;
        // url
        private string realUrlDownload;
        private string urlSong;
        private string urlData;

        private TypeSong typeOfSong=TypeSong.None;
       
        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string UrlSong { get => urlSong; set => urlSong = value; }
        public string ID { get => iD; set => iD = value; }
        public string RealUrlDownload { get => realUrlDownload; set => realUrlDownload = value; }
        public string Lyric { get => lyric; set => lyric = value; }
        public string ImageSinger { get => imageSinger; set => imageSinger = value; }
        public string HighestPosition { get => highestPosition; set => highestPosition = value; }
        public bool IsFavorited { get => isFavorited; set => isFavorited = value; }
        public TypeSong TypeOfSong { get => typeOfSong; set => typeOfSong = value; }
        public string UrlData { get => urlData; set => urlData = value; }
        public string LinkLyric { get => linkLyric; set => linkLyric = value; }
        public string ImageSingerBigSize { get => imageSingerBigSize; set => imageSingerBigSize = value; }
        public string KaraokeLyric { get => karaokeLyric; set => karaokeLyric = value; }
    }
}
