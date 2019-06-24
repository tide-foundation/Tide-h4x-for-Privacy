using System;
using System.Collections.Generic;
using System.Text;
using Raziel.Library.Classes;

namespace Raziel.Library.Models
{
    public class TideLog
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public TideLogLevel TideLogLevel { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public DateTime DateTime { get; set; }
    }
}
