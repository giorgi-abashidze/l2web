using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Models
{
    public class Clan
    {
        public string ClanName { get; set; }
        public string LeaderName { get; set; }
        public int ClanLevel { get; set; }
        public byte[] Icon { get; set; }

        public string getIconBase64() {

            string imgString = Convert.ToBase64String(Icon);
            return String.Format("data:image/bmp;base64,{0} \" ", imgString);

        }

        
    }
}
