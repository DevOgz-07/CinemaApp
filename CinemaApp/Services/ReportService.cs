using CinemaApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services
{
    public class ReportService
    {
        private readonly Database _db;

        public ReportService(Database db)
        {
            _db = db;
        }

        public void GunSonuRaporu()
        {
            Console.WriteLine("\n--- GÜN SONU RAPORU ---");

            int toplam = 0;

            foreach (var s in _db.Salonlar)
            {
                var f = _db.Filmler.FirstOrDefault(x => x.Id == s.FilmId);

                Console.WriteLine($"\n{s.Id}. Salon - Film: {f?.Ad ?? "Atanmamış"}");

                foreach (var k in s.Seanslar)
                {
                    int gelir = (k.SatilanTam * 50) + (k.SatilanOgrenci * 35);
                    toplam += gelir;

                    Console.WriteLine($"Seans {k.Saat}: Tam={k.SatilanTam}, Öğrenci={k.SatilanOgrenci}, Gelir={gelir}₺");
                }
            }

            Console.WriteLine($"\nTOPLAM KAZANÇ = {toplam}₺");
        }
    }
}
