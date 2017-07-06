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
        private string topicName;
        private string typeSong;
        private List<Song> listSong;
        private List<Song> listPlaylist;
        private List<Song> listMV;
        
        public string TopicName { get => topicName; set => topicName = value; }
        public string TypeSong { get => typeSong; set => typeSong = value; }
        public List<Song> ListSong { get => listSong; set => listSong = value; }
        public List<Song> ListPlaylist { get => listPlaylist; set => listPlaylist = value; }
        public List<Song> ListMV { get => listMV; set => listMV = value; }
    }
}
