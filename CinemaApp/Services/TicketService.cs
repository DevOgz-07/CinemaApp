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
using iText.Barcodes;

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
                    string fontPathRegular = @"C:\Windows\Fonts\segoeui.ttf";
                    string fontPathBold = @"C:\Windows\Fonts\segoeuib.ttf";

                    var fontProgramRegular = FontProgramFactory.CreateFont(fontPathRegular);
                    var fontProgramBold = FontProgramFactory.CreateFont(fontPathBold);

                    PdfFont font = PdfFontFactory.CreateFont(fontProgramRegular, PdfEncodings.IDENTITY_H);
                    PdfFont fontBold = PdfFontFactory.CreateFont(fontProgramBold, PdfEncodings.IDENTITY_H);

                    // ÜST BAŞLIK
                    doc.Add(new Paragraph("SİNEMA BİLETİ")
                        .SetFont(fontBold)
                        .SetFontSize(28)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    doc.Add(new Paragraph("──────────────").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    doc.Add(new Paragraph("\n"));

                    // QR CODE
                    var qr = new BarcodeQRCode(t.TicketNo);
                    var qrObject = qr.CreateFormXObject(pdf);
                    var qrImage = new Image(qrObject)
                        .SetWidth(120)
                        .SetHeight(120)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                    doc.Add(qrImage);

                    doc.Add(new Paragraph("\n"));

                    // BARKOD - CODE128
                    var barcode128 = new Barcode128(pdf);
                    barcode128.SetCode(t.TicketNo);
                    barcode128.SetCodeType(Barcode128.CODE128);

                    var bcImg = new Image(barcode128.CreateFormXObject(pdf))
                        .SetWidth(250)
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                    doc.Add(bcImg);
                    doc.Add(new Paragraph(t.TicketNo)
                        .SetFont(font)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    doc.Add(new Paragraph("\n──────────────\n")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    // BİLET BİLGİLERİ TABLOSU
                    var table = new Table(2).UseAllAvailableWidth();

                    table.AddCell(new Cell().Add(new Paragraph("Film").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(filmAd).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Salon").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.SalonId.ToString()).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Koltuk").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.KoltukNo.ToString()).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Seans").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.Saat).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Bilet Tipi").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.BiletTipi).SetFont(font)));

                    table.AddCell(new Cell().Add(new Paragraph("Satış Tarihi").SetFont(fontBold)));
                    table.AddCell(new Cell().Add(new Paragraph(t.Tarih.ToString("dd.MM.yyyy HH:mm")).SetFont(font)));

                    doc.Add(table);

                    doc.Add(new Paragraph("\n──────────────\n")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    doc.Add(new Paragraph("İyi Seyirler Dileriz")
                        .SetFont(font)
                        .SetFontSize(12)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
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

            var atanmisSalonlar = _db.Salonlar.Where(x => x.FilmId != null).ToList();
            if (!atanmisSalonlar.Any())
            {
                Console.WriteLine("Henüz film atanmış salon yok!");
                return;
            }

            foreach (var s in atanmisSalonlar)
            {
                var film = _db.Filmler.FirstOrDefault(f => f.Id == s.FilmId);
                if (film == null) continue; // Film bulunamazsa atla
                Console.WriteLine($"{s.Id}. Salon → {film.Ad}");
            }

            Console.Write("Salon ID: ");
            if (!int.TryParse(Console.ReadLine(), out int salonId))
            {
                Console.WriteLine("Geçersiz Salon ID!");
                return;
            }

            var salon = _db.Salonlar.FirstOrDefault(s => s.Id == salonId);
            if (salon == null)
            {
                Console.WriteLine("Salon bulunamadı!");
                return;
            }

            var filmData = _db.Filmler.FirstOrDefault(f => f.Id == salon.FilmId);
            if (filmData == null)
            {
                Console.WriteLine("Film bilgisi bulunamadı!");
                return;
            }

            if (salon.Seanslar == null || !salon.Seanslar.Any())
            {
                Console.WriteLine("Bu salonda seans bulunmamaktadır!");
                return;
            }

            Console.Write("Seans (14:00 / 18:00): ");
            string seans = Console.ReadLine();

            var sSeans = salon.Seanslar.FirstOrDefault(s => s.Saat == seans);
            if (sSeans == null)
            {
                Console.WriteLine("Seans bulunamadı!");
                return;
            }

            if (sSeans.DoluKoltuklar == null)
                sSeans.DoluKoltuklar = new List<int>();

            Console.WriteLine($"Dolu koltuklar: {(sSeans.DoluKoltuklar.Any() ? string.Join(",", sSeans.DoluKoltuklar) : "Hiç koltuk dolu değil")}");

            Console.Write("Koltuk Seç: ");
            if (!int.TryParse(Console.ReadLine(), out int koltuk))
            {
                Console.WriteLine("Geçersiz koltuk numarası!");
                return;
            }

            if (sSeans.DoluKoltuklar.Contains(koltuk))
            {
                Console.WriteLine("Bu koltuk dolu!");
                return;
            }

            Console.Write("Bilet Tipi (Tam / Ogrenci): ");
            string tip = Console.ReadLine()?.Trim().ToLower();

            switch (tip)
            {
                case "tam":
                    sSeans.SatilanTam++;
                    tip = "Tam";
                    break;
                case "ogrenci":
                    sSeans.SatilanOgrenci++;
                    tip = "Ogrenci";
                    break;
                default:
                    Console.WriteLine("Geçersiz bilet tipi!");
                    return;
            }

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
