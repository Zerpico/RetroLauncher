namespace RetroLauncher.DAL.Model
{
    /// <summary>
    /// Класс для удобного фильтра игр из базы
    /// </summary>
    public class FilterGame
    {
        public int Count { get; set; }
        public int Skip { get; set; }

        private string name { get; set; }
        public string Name { get { return name; } set { name = value != null ? value.ToLower() : null; } }      

        public int[] Genre { get; set; }
        public int[] Platform { get; set; }

        public int OrderByName { get; set; }
        public int OrderByPlatform { get; set; }
        public int OrderByRating { get; set; }
        public int OrderByDownload { get; set; }
    }
}
