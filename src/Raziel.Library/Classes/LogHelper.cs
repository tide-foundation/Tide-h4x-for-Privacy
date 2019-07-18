// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Raziel.Library.Models;

namespace Raziel.Library.Classes {
    public static class LogHelper {
        public static void LogMsg(this ITideLogger logger, string message, AuthenticationModel model = null, Exception ex = null) {
            try {
                var tideLog = new TideLog()
                {
                    Data = JsonConvert.SerializeObject(new
                    {
                        Data = model,
                        Exception = ex,
                        HashedUsername = model?.Username,
                        ConvertedUsername = model?.Username?.ConvertToUint64(),
                        UserIp = model?.Ip
                    }),
                    Message = message,
                    TideLogLevel = ex == null ? TideLogLevel.Information : TideLogLevel.Error
                };

                logger.Log(tideLog);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}