using System;
using Microsoft.Extensions.Logging;
using Raziel.Library.Models;

namespace Raziel.Library.Classes {
    public static class LogHelper {
        public static void LogMsg(this ILogger logger, string message, AuthenticationModel model = null, Exception ex = null) {
            var errorModel = new {hashedusername = model?.Username, convertedUsername = model?.Username?.ConvertToUint64(), userIp = model?.Ip, exception = ex};
            if (ex != null) logger.LogError(message, errorModel);
            else logger.LogWarning(message, errorModel);
        }
    }
}