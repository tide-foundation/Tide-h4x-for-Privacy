
using System;
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
            log.DateTime = DateTime.Now;
            _context.Add(log);
            _context.SaveChanges();
        }
    }
}