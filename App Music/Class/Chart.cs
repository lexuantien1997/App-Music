using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public class Chart
    {
        private string topicName;
        private string typeSong;
        private ObservableCollection<Song> listSong;
        private ObservableCollection<Song> listPlaylist;
        private ObservableCollection<Song> listMV;
        public Chart()
        {
            listSong = new ObservableCollection<Song>();
            listPlaylist = new ObservableCollection<Song>();
            listMV = new ObservableCollection<Song>();
        }
        public string TopicName { get => topicName; set => topicName = value; }
        public string TypeSong { get => typeSong; set => typeSong = value; }
        public ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }
        public ObservableCollection<Song> ListPlaylist { get => listPlaylist; set => listPlaylist = value; }
        public ObservableCollection<Song> ListMV { get => listMV; set => listMV = value; }
    }
}
