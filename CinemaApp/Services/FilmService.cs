using CinemaApp.Data;
using CinemaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services
{
    public class FilmService
    {
        private readonly Database _db;

        public FilmService(Database db)
        {
            _db = db;
        }

        public void FilmEkle()
        {
            Console.Write("Film Adı: ");
            string ad = Console.ReadLine();

            Console.Write("Afiş Yolu (Opsiyonel): ");
            string afis = Console.ReadLine();

            _db.Filmler.Add(new Film
            {
                Id = _db.Filmler.Count + 1,
                Ad = ad,
                AfisYolu = afis
            });

            _db.Save();
            Console.WriteLine("Film eklendi!");
        }

        public void FilmListele()
        {
            foreach (var f in _db.Filmler)
                Console.WriteLine($"{f.Id} - {f.Ad}");

            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }


        public void FilmSil()
        {
            Console.Write("Silinecek Film ID: ");
            int id = int.Parse(Console.ReadLine());

            _db.Filmler.RemoveAll(x => x.Id == id);
            _db.Save();

            Console.WriteLine("Film silindi.");
        }
    }
}
