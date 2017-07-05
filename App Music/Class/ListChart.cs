using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Music.Class
{
   public static class ListChart
    {
        private static Chart k_Pop;
        private static Chart v_Pop;
        private static Chart us_Uk;

        public static Chart K_Pop { get => k_Pop; set => k_Pop = value; }
        public static Chart V_Pop { get => v_Pop; set => v_Pop = value; }
        public static Chart Us_Uk { get => us_Uk; set => us_Uk = value; }
    }
}
