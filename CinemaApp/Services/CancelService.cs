using CinemaApp.Data;
using CinemaApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            var biletler = _db.Biletler.ToList();

            if (!biletler.Any())
            {
                Console.WriteLine("❌ Aktif bilet bulunmamaktadır.");
                Console.WriteLine("Devam etmek için bir tuşa basın...");
                Console.ReadKey();
                return;
            }

            // ───── BİLETLERİ LİSTELE ─────────────────────────────
            Console.WriteLine("\n--- AKTİF BİLETLER ---\n");
            for (int i = 0; i < biletler.Count; i++)
            {
                var t = biletler[i];
                var film = _db.Filmler.FirstOrDefault(f => f.Id == t.FilmId);
                var salon = _db.Salonlar.FirstOrDefault(s => s.Id == t.SalonId);

                string filmAd = string.IsNullOrWhiteSpace(film?.Ad) ? "Bilgi Yok" : film.Ad;
                string seansSaat = string.IsNullOrWhiteSpace(t.Seans) ? "Bilgi Yok" : t.Seans;

                Console.WriteLine($"{i + 1}. 🎟 {t.TicketNo} | 🎬 {filmAd} | 🏛 Salon {salon?.Id} | 🕒 {seansSaat} | 💺 {t.KoltukNo}");
            }

            Console.Write("\nİptal etmek istediğiniz biletin sıra numarasını girin: ");
            if (!int.TryParse(Console.ReadLine(), out int sec) || sec < 1 || sec > biletler.Count)
            {
                Console.WriteLine("❌ Geçersiz seçim!");
                return;
            }

            var ticket = biletler[sec - 1];
            var filmInfo = _db.Filmler.FirstOrDefault(f => f.Id == ticket.FilmId);
            var salonInfo = _db.Salonlar.FirstOrDefault(s => s.Id == ticket.SalonId);

            // ───── SEANS BULMA ─────────────────────────────
            var seans = salonInfo?.Seanslar.FirstOrDefault(
                s => s.Saat == ticket.Saat || s.Saat == ticket.Seans || s.Id == ticket.seansId
            );

            if (salonInfo == null || seans == null)
            {
                Console.WriteLine("❌ Salon veya seans bilgisi bulunamadı!");
                return;
            }

            // ───── BİLET ÖNİZLEME ─────────────────────────────
            Console.WriteLine("\n──────── BİLET BİLGİLERİ ────────");
            Console.WriteLine($"🎟 Bilet No:    {ticket.TicketNo}");
            Console.WriteLine($"🎬 Film:        {filmInfo?.Ad ?? "Bilgi Yok"}");
            Console.WriteLine($"🏛 Salon:       {salonInfo.Id}");
            Console.WriteLine($"🕒 Seans:       {ticket.Seans}");
            Console.WriteLine($"💺 Koltuk:      {ticket.KoltukNo}");
            Console.WriteLine($"🎫 Tip:         {ticket.BiletTipi}");
            Console.WriteLine($"📅 Tarih:       {ticket.Tarih:dd.MM.yyyy HH:mm}");
            Console.WriteLine("─────────────────────────────────");

            Console.Write("\nBu bileti iptal etmek istediğinize emin misiniz? (E/H): ");
            var cevap = Console.ReadLine()?.ToLower();

            if (cevap != "e")
            {
                Console.WriteLine("\n❗ İşlem iptal edildi.");
                return;
            }

            // ───── YOĞUNLUK AZALTMA ─────────────────────────────
            if (ticket.BiletTipi.ToLower() == "tam")
                seans.SatilanTam--;
            else
                seans.SatilanOgrenci--;

            // ───── KOLTUĞU BOŞALT ───────────────────────────────
            seans.DoluKoltuklar.Remove(ticket.KoltukNo);

            // ───── TICKET SİL ─────────────────────────────────────
            _db.Biletler.Remove(ticket);
            _db.Save();

            Console.WriteLine("\n✅ Bilet başarıyla iptal edildi.");

            // ───── PDF SİLME ─────────────────────────────────────
            string pdfPath = Path.Combine(Environment.CurrentDirectory, "Tickets", $"Bilet_{ticket.TicketNo}.pdf");

            if (File.Exists(pdfPath))
            {
                try
                {
                    File.Delete(pdfPath);
                    Console.WriteLine($"🗑 PDF silindi: {pdfPath}");
                }
                catch
                {
                    Console.WriteLine("⚠ PDF silinemedi!");
                }
            }

            // ───── GÜNCEL KOLTUK DURUMU ─────────────────────────
            Console.WriteLine("\n📊 Güncel Salon Koltuk Durumu:\n");
            KoltukHaritasiGoster(seans, salonInfo.Kapasite);

            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }

        // ───── KOLTUK HARİTASI OLUŞTURMA ─────────────────────────
        private void KoltukHaritasiGoster(Seans seans, int kapasite)
        {
            for (int i = 1; i <= kapasite; i++)
            {
                if (seans.DoluKoltuklar.Contains(i))
                    Console.Write("[ X ]");   // dolu
                else
                    Console.Write("[   ]");   // boş

                if (i % 10 == 0)  // her 10 koltuk = 1 sıra
                    Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
