using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Ticket
    {
        public string TicketNo { get; set; }
        public int FilmId { get; set; }
        public int SalonId { get; set; }
        public int KoltukNo { get; set; }
        public string Saat { get; set; }
        public string BiletTipi { get; set; }
        public DateTime Tarih { get; set; }
    }
}
