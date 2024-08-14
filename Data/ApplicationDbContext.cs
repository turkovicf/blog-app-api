using BlogAppAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogAppAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }       
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {

        }

    }
}
