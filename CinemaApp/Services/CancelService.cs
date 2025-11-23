using CinemaApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services
{
    public class CancelService
    {
        private readonly Database _db;

        public CancelService(Database db)
        {
            _db = db;
        }

        public void BiletIptal()
        {
            Console.Write("İptal Edilecek Bilet No: ");
            string no = Console.ReadLine();

            var ticket = _db.Biletler.FirstOrDefault(t => t.TicketNo == no);

            if (ticket == null)
            {
                Console.WriteLine("Bilet bulunamadı!");
                return;
            }

            var salon = _db.Salonlar.First(s => s.Id == ticket.SalonId);
            var seans = salon.Seanslar.First(s => s.Saat == ticket.Saat);

            // Yoğunluk azaltma
            if (ticket.BiletTipi.ToLower() == "tam")
                seans.SatilanTam--;
            else
                seans.SatilanOgrenci--;

            seans.DoluKoltuklar.Remove(ticket.KoltukNo);

            _db.Biletler.Remove(ticket);
            _db.Save();

            Console.WriteLine("Bilet iptal edildi.");
        }
    }
}
