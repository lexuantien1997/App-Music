using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public class Charts
    {
        private string topicName;
        private string typeSong;
        private List<Song> listSong;
        private List<Song> listPlaylist;
        private List<Song> listMV;

        public Charts(string _topicName,string _typeSong)
        {
            TopicName = _topicName;
            TypeSong = _typeSong;

            ListSong = new List<Song>();
            ListPlaylist = new List<Song>();
            ListMV = new List<Song>();
        }

        public Charts()
        {
            ListSong = new List<Song>();
            ListPlaylist = new List<Song>();
            ListMV = new List<Song>();
        }

        public string TopicName { get => topicName; set => topicName = value; }
        public List<Song> ListSong { get => listSong; set => listSong = value; }
        public string TypeSong { get => typeSong; set => typeSong = value; }
        public List<Song> ListPlaylist { get => listPlaylist; set => listPlaylist = value; }
        public List<Song> ListMV { get => listMV; set => listMV = value; }
    }
}
