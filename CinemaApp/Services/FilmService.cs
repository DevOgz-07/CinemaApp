using CinemaApp.Data;
using CinemaApp.Models;
using System;
using System.IO;
using System.Linq;

namespace CinemaApp.Services
{
    public class FilmService
    {
        private readonly Database _db;

        public FilmService(Database db)
        {
            _db = db;
        }

        // Yeni ID güvenli şekilde oluşturulur
        private int YeniFilmId()
        {
            return _db.Filmler.Count == 0 ? 1 : _db.Filmler.Max(f => f.Id) + 1;
        }

        public void FilmEkle()
        {
            Console.WriteLine("=== Yeni Film Ekle ===");

            // Film Adı Zorunlu
            string ad;
            do
            {
                Console.Write("Film Adı: ");
                ad = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(ad))
                    Console.WriteLine("❗ Film adı boş bırakılamaz!");
            }
            while (string.IsNullOrWhiteSpace(ad));

            // Afiş yolu opsiyonel
            Console.Write("Afiş Yolu (Opsiyonel, Enter geçebilirsiniz): ");
            string afis = Console.ReadLine();

            // Eğer afiş yolu girilmişse kontrol et
            if (!string.IsNullOrWhiteSpace(afis))
            {
                if (!File.Exists(afis))
                {
                    Console.WriteLine("⚠️ Afiş bulunamadı! Yine de kaydedilsin mi? (E/H): ");
                    string cevap = Console.ReadLine().ToLower();

                    if (cevap != "e")
                        afis = null;
                }
            }

            // Film Kaydı
            var film = new Film
            {
                Id = YeniFilmId(),
                Ad = ad.Trim(),
                AfisYolu = string.IsNullOrWhiteSpace(afis) ? null : afis.Trim()
            };

            _db.Filmler.Add(film);
            _db.Save();

            Console.WriteLine("✅ Film başarıyla eklendi!");
        }

        public void FilmListele()
        {
            Console.WriteLine("=== Mevcut Filmler ===");

            if (_db.Filmler.Count == 0)
            {
                Console.WriteLine("Henüz film eklenmemiş.");
                Console.ReadKey();
                return;
            }

            foreach (var f in _db.Filmler)
                Console.WriteLine($"{f.Id} - {f.Ad}");

            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }

        public void FilmGuncelle()
        {
            Console.Write("Güncellenecek Film ID: ");
            int id = int.Parse(Console.ReadLine());

            var film = _db.Filmler.FirstOrDefault(f => f.Id == id);

            if (film == null)
            {
                Console.WriteLine("❗ Film bulunamadı!");
                return;
            }

            Console.WriteLine($"Seçilen film: {film.Ad}");

            Console.Write("Yeni Film Adı (Enter → değiştirme): ");
            string yeniAd = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(yeniAd))
                film.Ad = yeniAd.Trim();

            Console.Write("Yeni Afiş Yolu (Enter → değiştirme): ");
            string yeniAfis = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(yeniAfis))
            {
                if (File.Exists(yeniAfis))
                    film.AfisYolu = yeniAfis.Trim();
                else
                    Console.WriteLine("⚠️ Dosya bulunamadı! Afiş güncellenmedi.");
            }

            _db.Save();
            Console.WriteLine("✅ Film güncellendi!");
        }

        public void FilmSil()
        {
            Console.Write("Silinecek Film ID: ");
            int id = int.Parse(Console.ReadLine());

            var film = _db.Filmler.FirstOrDefault(x => x.Id == id);

            if (film == null)
            {
                Console.WriteLine("❗ Film bulunamadı!");
                return;
            }

            Console.WriteLine($"Film Silinecek: {film.Ad}");
            Console.Write("Emin misiniz? (Sil E/ Silme H): ");
            string cevap = Console.ReadLine().ToLower();

            if (cevap != "e")
            {
                Console.WriteLine("İşlem iptal edildi.");
                return;
            }

            _db.Filmler.Remove(film);
            _db.Save();

            Console.WriteLine("🗑️ Film silindi.");
        }
    }
}
