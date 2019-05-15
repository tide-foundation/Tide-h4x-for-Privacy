using Microsoft.EntityFrameworkCore;
using Raziel.Library.Models;

namespace Raziel.Vendor {
    public class RazielContext : DbContext {
        public RazielContext(DbContextOptions<RazielContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
    }
}