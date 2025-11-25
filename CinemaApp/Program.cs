using CinemaApp.Data;
using CinemaApp.Services;
using System;

namespace CinemaApp
{
    internal class Program
    {
        static void Main()
        {
            var db = Database.Load();

            var filmSrv = new FilmService(db);
            var salonSrv = new SalonService(db);
            var ticketSrv = new TicketService(db);
            var cancelSrv = new CancelService(db);
            var ticketListSrv = new TicketListService(db);
            var reportSrv = new ReportService(db);

            while (true)
            {
                Console.WriteLine("\n--- MENÜ ---");
                Console.WriteLine("1 - Film Ekle");
                Console.WriteLine("2 - Film Listele");
                Console.WriteLine("3 - Film Sil");
                Console.WriteLine("4 - Salona Film Ata");
                Console.WriteLine("5 - Bilet Sat");
                Console.WriteLine("6 - Bilet İptal Et");
                Console.WriteLine("7 - Biletleri Listele");
                Console.WriteLine("8 - Gün Sonu Raporu");
                Console.WriteLine("9 - Çıkış");

                Console.Write("Seçim: ");

                if (!int.TryParse(Console.ReadLine(), out int sec))
                {
                    Console.WriteLine("Geçersiz seçim!");
                    continue;
                }

                switch (sec)
                {
                    case 1: filmSrv.FilmEkle(); break;
                    case 2: filmSrv.FilmListele(); break;
                    case 3: filmSrv.FilmSil(); break;
                    case 4: salonSrv.SalonaFilmAta(); break;
                    case 5: ticketSrv.BiletSat(); break;
                    case 6: cancelSrv.BiletIptal(); break;  // <-- Bilet Silme / İptal
                    case 7: ticketListSrv.BiletleriListele(); break;
                    case 8: reportSrv.GunSonuRaporu(); break;
                    case 9: return;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }
            }
        }
    }
}
