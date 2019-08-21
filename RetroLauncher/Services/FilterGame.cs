using RetroLauncher.Model;

namespace RetroLauncher.Services
{
    /// <summary>
    /// Класс для удобного фильтра игр из базы
    /// </summary>
    public class FilterGame
    {
        private string name { get; set; }
        public string Name { get { return name; } set { name = value != null ? value.ToLower() : null; } }

        public Platform Platform { get; set; }

        private string genre { get; set; }
        public string Genre { get { return genre; } set { genre = value != null ? value.ToLower() : null; } }
    }
}