using CinemaApp.Data;
using System;
using System.Linq;

namespace CinemaApp.Services
{
    public class TicketListService
    {
        private readonly Database _db;

        public TicketListService(Database db)
        {
            _db = db;
        }

        public void BiletleriListele()
        {
            Console.WriteLine("\n--- AKTİF BİLETLER ---\n");

            if (!_db.Biletler.Any())
            {
                Console.WriteLine("Aktif bilet bulunmuyor.");
                return;
            }

            // Kolon başlıkları
            Console.WriteLine(
                $"{"BiletNo",-12} | {"Film",-20} | {"Salon",-6} | {"Seans",-6} | {"Koltuk",-6} | {"Tip",-8} | {"Tarih",-16}"
            );
            Console.WriteLine(new string('-', 90));

            foreach (var t in _db.Biletler)
            {
                var film = _db.Filmler.FirstOrDefault(f => f.Id == t.FilmId);
                var salon = _db.Salonlar.FirstOrDefault(s => s.Id == t.SalonId);

                string filmAd = film != null ? film.Ad : "SİLİNMİŞ";
                string salonAd = salon != null ? salon.Id.ToString() : "SİLİNMİŞ";

                Console.WriteLine(
                    $"{t.TicketNo,-12} | " +
                    $"{filmAd,-20} | " +
                    $"{salonAd,-6} | " +
                    $"{t.Saat,-6} | " +
                    $"{t.KoltukNo,-6} | " +
                    $"{t.BiletTipi,-8} | " +
                    $"{t.Tarih:dd.MM.yyyy HH:mm}"
                );
            }
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}
