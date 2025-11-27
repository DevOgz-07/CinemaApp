using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaApp.Helpers;

namespace CinemaApp.Services
{
    public static class AnimationService
    {
        // Sinema perdesi açılma animasyonu
        public static void SinemaPerdesi()
        {
            Console.Clear();
            string leftCurtain = "████████████████████████████████";
            string rightCurtain = leftCurtain;

            int steps = leftCurtain.Length;

            for (int i = 0; i < steps; i++)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(leftCurtain.Substring(i));
                Console.ResetColor();

                Console.Write(new string(' ', i * 2));

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(rightCurtain.Substring(i));
                Console.ResetColor();

                Thread.Sleep(40);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n          🎬 CİNEMA 🎟️");
            Console.WriteLine("          Hoş Geldiniz...\n");
            Console.ResetColor();

            Thread.Sleep(1000);
        }

        // Film rulosu animasyonu
        public static void FilmRulosu()
        {
            string[] frames = new string[]
            {
                "🎞️-----",
                "-🎞️----",
                "--🎞️---",
                "---🎞️--",
                "----🎞️-",
                "-----🎞️"
            };

            for (int i = 0; i < 12; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(frames[i % frames.Length]);
                Console.ResetColor();
                Thread.Sleep(80);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }

            Console.WriteLine();
        }

        // Yazı yazma efekti
        public static void Yaz(TypingEffect t)
        {
            foreach (char c in t.Text)
            {
                Console.Write(c);
                Thread.Sleep(t.Speed);
            }
            Console.WriteLine();
        }

        // Bilet basma animasyonu
        public static void BiletBas()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nBilet basılıyor...\n");
            Console.ResetColor();

            string[] printer =
            {
                "[=====     ]",
                "[========  ]",
                "[==========]"
            };

            foreach (var p in printer)
            {
                Console.WriteLine(p);
                Thread.Sleep(300);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }

            Console.WriteLine("[======== BİTTİ ========]\n");
        }
    }
}
