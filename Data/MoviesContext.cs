using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.Data;

namespace Movies.Data
{
    public class MoviesContext(DbContextOptions<MoviesContext> options) : IdentityDbContext<AuthUser>(options)
    {
        public DbSet<Movies.Movie> Movie { get; set; } = default!;
    }
}
