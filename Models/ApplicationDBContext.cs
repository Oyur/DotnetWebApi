using Microsoft.EntityFrameworkCore;

namespace DotnetWebApi.Models
{
    public class ApplicationDBContext : DbContext
    {
        //สร้างconstructor เชื่อมต่อ Dbcontext
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        //เรียก Model
        public DbSet<Category> Category { get; set;}
        public DbSet<Product> Product { get; set;}

    }
}