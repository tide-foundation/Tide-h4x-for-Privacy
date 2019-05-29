using Raziel.Library.Classes;
using Raziel.Library.Models;

namespace Raziel.Logger.Classes
{
    public class StoreTideLogger : ITideLogger
    {
        private readonly LoggerContext _context;

        public StoreTideLogger(LoggerContext context)
        {
            _context = context;
        }

        public void Log(TideLog log) {
            _context.Add(log);
            _context.SaveChanges();
        }
    }
}