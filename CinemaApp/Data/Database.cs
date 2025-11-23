using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using CinemaApp.Models;

namespace CinemaApp.Data
{
    public class Database
    {
        private const string FilePath = "cinema.json";

        public List<Film> Filmler { get; set; } = new List<Film>();
        public List<Salon> Salonlar { get; set; } = new List<Salon>();
        public List<Ticket> Biletler { get; set; } = new List<Ticket>();

        public static Database Load()
        {
            if (!File.Exists(FilePath))
                return new Database();

            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Database>(json);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}

