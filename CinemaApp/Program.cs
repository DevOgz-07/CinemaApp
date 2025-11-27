using CinemaApp.Data;
using CinemaApp.Helpers;
using CinemaApp.Services;
using System;
using System.Threading;

namespace CinemaApp
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Açılış animasyonları
            AnimationService.SinemaPerdesi();
            AnimationService.FilmRulosu();
            AnimationService.Yaz(new TypingEffect("Sistem Yükleniyor...\n", 15));

            // Veritabanı yükle
            var db = Database.Load();

            // Servisler
            var filmSrv = new FilmService(db);
            var salonSrv = new SalonService(db);
            var ticketSrv = new TicketService(db);
            var cancelSrv = new CancelService(db);
            var ticketListSrv = new TicketListService(db);
            var reportSrv = new ReportService(db);

            // Menü döngüsü
            while (true)
            {
                Console.Clear();
                MenuRenderer.ShowMenu();

                Console.Write("\nSeçim: "); // <-- eksik olan buydu

                if (!int.TryParse(Console.ReadLine(), out int sec))
                {
                    Console.WriteLine("Geçersiz seçim!");
                    Thread.Sleep(700);
                    continue;
                }

                switch (sec)
                {
                    case 1: filmSrv.FilmEkle(); break;
                    case 2: filmSrv.FilmListele(); break;
                    case 3: filmSrv.FilmSil(); break;
                    case 4: salonSrv.SalonaFilmAta(); break;
                    case 5: ticketSrv.BiletSat(); break;
                    case 6: cancelSrv.BiletIptal(); break;
                    case 7: ticketListSrv.BiletleriListele(); break;
                    case 8: reportSrv.GunSonuRaporu(); break;
                    case 9: return;

                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        Thread.Sleep(700);
                        break;
                }
            }
        }
    }
}

