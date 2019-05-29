using Microsoft.EntityFrameworkCore;
using Raziel.Library.Models;

namespace Raziel.Logger
{
    public class LoggerContext : DbContext
    {
        public LoggerContext(DbContextOptions<LoggerContext> options)
            : base(options)
        {
        }

        public DbSet<TideLog> Logs { get; set; }
    }
}