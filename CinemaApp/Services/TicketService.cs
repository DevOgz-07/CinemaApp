using CinemaApp.Data;
using CinemaApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font;

namespace CinemaApp.Services
{
    public class TicketService
    {
        private readonly Database _db;

        public TicketService(Database db)
        {
            _db = db;
        }

        private void CreatePdf(Ticket t, string filmAd)
        {
            // Tickets klasörünü oluştur
            string folder = Path.Combine(Environment.CurrentDirectory, "Tickets");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, $"Bilet_{t.TicketNo}.pdf");

            using (var writer = new PdfWriter(path))
            using (var pdf = new PdfDocument(writer))
            using (var doc = new Document(pdf))
            {
                // Türkçe karakter ve emoji destekli font (Windows için)
                string fontPath = @"C:\Windows\Fonts\ARIALUNI.TTF"; // Arial Unicode MS
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                doc.Add(new Paragraph("SİNEMA BİLETİ").SetFont(font).SetFontSize(20));
                doc.Add(new Paragraph($"Film: {filmAd}").SetFont(font));
                doc.Add(new Paragraph($"Salon: {t.SalonId}").SetFont(font));
                doc.Add(new Paragraph($"Koltuk: {t.KoltukNo}").SetFont(font));
                doc.Add(new Paragraph($"Seans: {t.Saat}").SetFont(font));
                doc.Add(new Paragraph($"Bilet Tipi: {t.BiletTipi}").SetFont(font));
                doc.Add(new Paragraph($"Tarih: {t.Tarih:dd.MM.yyyy HH:mm}").SetFont(font));

                Console.WriteLine($"PDF oluşturuldu: {path}");
            }
        }

        public void BiletSat()
        {
            Console.WriteLine("--- Film Atanmış Salonlar ---");

            foreach (var s in _db.Salonlar.Where(x => x.FilmId != null))
            {
                var film = _db.Filmler.First(f => f.Id == s.FilmId);
                Console.WriteLine($"{s.Id}. Salon → {film.Ad}");
            }

            Console.Write("Salon ID: ");
            int salonId = int.Parse(Console.ReadLine());

            var salon = _db.Salonlar.First(s => s.Id == salonId);
            var filmData = _db.Filmler.First(f => f.Id == salon.FilmId);

            Console.Write("Seans (14:00 / 18:00): ");
            string seans = Console.ReadLine();

            var sSeans = salon.Seanslar.First(s => s.Saat == seans);

            Console.WriteLine($"Dolu koltuklar: {string.Join(",", sSeans.DoluKoltuklar)}");

            Console.Write("Koltuk Seç: ");
            int koltuk = int.Parse(Console.ReadLine());

            if (sSeans.DoluKoltuklar.Contains(koltuk))
            {
                Console.WriteLine("Bu koltuk dolu!");
                return;
            }

            Console.Write("Bilet Tipi (Tam / Ogrenci): ");
            string tip = Console.ReadLine();

            if (tip.ToLower() == "tam")
                sSeans.SatilanTam++;
            else
                sSeans.SatilanOgrenci++;

            sSeans.DoluKoltuklar.Add(koltuk);

            var ticket = new Ticket
            {
                TicketNo = Guid.NewGuid().ToString().Substring(0, 8),
                FilmId = filmData.Id,
                SalonId = salon.Id,
                KoltukNo = koltuk,
                Saat = seans,
                BiletTipi = tip,
                Tarih = DateTime.Now
            };

            _db.Biletler.Add(ticket);
            _db.Save();

            Console.WriteLine("Bilet Satıldı!");
            CreatePdf(ticket, filmData.Ad);
        }
    }
}
