using LeadSoft.Test.Commom.Models;

namespace LeadSoft.Test.Models
{
    public class Anime
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public User User { get; set; }
    }
}
