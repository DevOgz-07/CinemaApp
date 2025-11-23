using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public int Kapasite { get; set; }
        public int? FilmId { get; set; }

        public List<Seans> Seanslar { get; set; }
    }
}
