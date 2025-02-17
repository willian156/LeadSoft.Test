using LeadSoft.Test.Commom.Models;
using LeadSoft.Test.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadSoft.Test.DAO
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Anime> Animes { get; set; }
    }
}
