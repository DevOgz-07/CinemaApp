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
            string folder = Path.Combine(Environment.CurrentDirectory, "Tickets");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, $"Bilet_{t.TicketNo}.pdf");

            try
            {
                using (var writer = new PdfWriter(path))
                using (var pdf = new PdfDocument(writer))
                using (var doc = new Document(pdf))
                {
                    // Normal ve Bold fontlar
                    string fontPathRegular = @"C:\Windows\Fonts\segoeui.ttf";   // Segoe UI Regular
                    string fontPathBold = @"C:\Windows\Fonts\segoeuib.ttf";      // Segoe UI Bold

                    var fontProgramRegular = FontProgramFactory.CreateFont(fontPathRegular); // Framework 7.3 uyumluluğu için kullanılabilir.
                    var fontProgramBold = FontProgramFactory.CreateFont(fontPathBold);

                    PdfFont font = PdfFontFactory.CreateFont(fontProgramRegular, PdfEncodings.IDENTITY_H);
                    PdfFont fontBold = PdfFontFactory.CreateFont(fontProgramBold, PdfEncodings.IDENTITY_H);

                    // Başlık
                    doc.Add(new Paragraph("SİNEMA BİLETİ")
                        .SetFont(fontBold)
                        .SetFontSize(24)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    );

                    doc.Add(new Paragraph("\n"));

                    // Bilgiler tabloyla
                    var table = new Table(2).UseAllAvailableWidth();
                    table.AddCell(new Cell().Add(new Paragraph("Film:").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(filmAd).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Salon:").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.SalonId.ToString()).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Koltuk:").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.KoltukNo.ToString()).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Seans:").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.Saat).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Bilet Tipi:").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.BiletTipi).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Tarih:").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.Tarih.ToString("dd.MM.yyyy HH:mm")).SetFont(font)));

                    doc.Add(table);

                    // Alt not
                    doc.Add(new Paragraph("\nİyi Seyirler!")
                        .SetFont(font)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    );
                }

                Console.WriteLine($"PDF oluşturuldu: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("PDF oluşturulurken hata: " + ex.Message);
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
