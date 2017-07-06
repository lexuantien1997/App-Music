using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
    public static class ListChart
    {
        private static ChartSong k_Pop = new ChartSong();
        private static ChartSong v_Pop = new ChartSong();
        private static ChartSong us_Uk = new ChartSong();

        public static ChartSong K_Pop { get => k_Pop; set => k_Pop = value; }
        public static ChartSong V_Pop { get => v_Pop; set => v_Pop = value; }
        public static ChartSong Us_Uk { get => us_Uk; set => us_Uk = value; }
    }
}
