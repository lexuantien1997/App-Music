using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public class ChartSong
    {
        private List<Song> listSong;
        private List<Song> listPlaylist;
        private List<Song> listVideo;
        
        public List<Song> ListSong { get => listSong; set => listSong = value; }
        public List<Song> ListPlaylist { get => listPlaylist; set => listPlaylist = value; }
        public List<Song> ListVideo { get => listVideo; set => listVideo = value; }
    }
}
