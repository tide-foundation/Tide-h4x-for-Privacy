
using Piwik.Tracker;
using Raziel.Library.Classes;
using Raziel.Library.Models;

namespace Raziel.Logger.Classes
{
    public class StoreTideLogger : ITideLogger
    {
        private readonly LoggerContext _context;
        private readonly PiwikTracker _tracker;

        public StoreTideLogger(LoggerContext context, PiwikTracker tracker)
        {
            _context = context;
            _tracker = tracker;
        }

        public void Log(TideLog log) {

            _tracker.SetCustomVariable(1, "User IP", "Some IP here");
            _tracker.DoTrackEvent($"Logger", $"Name: {log.Identifier}. Message: {log.Message}", log.Data);

            _context.Add(log);
            _context.SaveChanges();
        }
    }
}