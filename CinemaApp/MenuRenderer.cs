using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp
{
    public static class MenuRenderer
    {
        public static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔════════════════════════════╗");
            Console.WriteLine("║        🎬 CINEMA MENU      ║");
            Console.WriteLine("╚════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" 1  📽️  - Film Ekle");
            Console.WriteLine(" 2  🎬  - Film Listele");
            Console.WriteLine(" 3  ❌  - Film Sil");
            Console.WriteLine(" 4  🏛️  - Salona Film Ata");
            Console.WriteLine(" 5  🎟️  - Bilet Sat");
            Console.WriteLine(" 6  🔙  - Bilet İptal Et");
            Console.WriteLine(" 7  📄  - Biletleri Listele");
            Console.WriteLine(" 8  📊  - Gün Sonu Raporu");
            Console.WriteLine(" 9  🚪  - Çıkış");
            Console.ResetColor();

            
        }
    }
}
