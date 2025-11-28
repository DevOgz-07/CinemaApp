using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Ticket
    {
        public string TicketNo { get; set; }   // Bilet numarası
        public int FilmId { get; set; }        // Film referansı
        public int SalonId { get; set; }       // Salon referansı
        public string Seans { get; set; }      // Örn: "14:00"
        public int KoltukNo { get; set; }      // Koltuk numarası
        public string BiletTipi { get; set; }  // Tam / Öğrenci
        public DateTime Tarih { get; set; }    // Satış tarihi
        public string Saat { get; set; }      // Örn: "14:00"
        public int seansId { get; set; }
    }
}
