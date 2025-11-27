using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaApp.Helpers
{
    public class TypingEffect
    {
        public string Text { get; set; }
        public int Speed { get; set; } = 20; 
        public TypingEffect(string text, int speed = 20)
        {
            Text = text;
            Speed = speed;
        }
    }
}
