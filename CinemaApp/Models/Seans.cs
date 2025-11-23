using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Seans
    {
        public string Saat { get; set; }
        public int SatilanTam { get; set; }
        public int SatilanOgrenci { get; set; }

        public List<int> DoluKoltuklar { get; set; } = new List<int>();
    }
}
