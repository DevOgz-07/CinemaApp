using CinemaApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services
{
    public class SalonService
    {
        private readonly Database _db;

        public SalonService(Database db)
        {
            _db = db;

            if (_db.Salonlar.Count == 0)
            {
                _db.Salonlar.AddRange(new[]
                {
                    new Models.Salon { Id = 1, Kapasite = 150, Seanslar = DefaultSeans() },
                    new Models.Salon { Id = 2, Kapasite = 200, Seanslar = DefaultSeans() },
                    new Models.Salon { Id = 3, Kapasite = 180, Seanslar = DefaultSeans() },
                    new Models.Salon { Id = 4, Kapasite = 225, Seanslar = DefaultSeans() },
                    new Models.Salon { Id = 5, Kapasite = 175, Seanslar = DefaultSeans() }
                });
            }
        }

        private System.Collections.Generic.List<Models.Seans> DefaultSeans()
        {
             return new System.Collections.Generic.List<Models.Seans>
             {
                 new Models.Seans { Saat = "14:00" },
                 new Models.Seans { Saat = "18:00" }
             };
        }

        public void SalonaFilmAta()
        {
            Console.WriteLine("--- Filmler ---");
            foreach (var f in _db.Filmler)
                Console.WriteLine($"{f.Id} - {f.Ad}");

            Console.Write("Film ID: ");
            int filmId = int.Parse(Console.ReadLine());

            Console.WriteLine("--- Salonlar ---");
            foreach (var s in _db.Salonlar)
                Console.WriteLine($"{s.Id}. Salon (Kapasite: {s.Kapasite})");

            Console.Write("Salon ID: ");
            int salonId = int.Parse(Console.ReadLine());

            _db.Salonlar.First(s => s.Id == salonId).FilmId = filmId;
            _db.Save();

            Console.WriteLine("Film salona atandı.");
        }
    }
}
