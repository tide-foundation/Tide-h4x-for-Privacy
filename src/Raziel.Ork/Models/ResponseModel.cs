using System.Collections.Generic;
using System;

namespace Raziel.Ork.Models
{

    public class ResponseModel
    {
        public string Private { get; set; }
        public IEnumerable<string> Public { get; set; }
    }
    public class SingleResponseModel
    {
        public string Private { get; set; }
        public string Public { get; set; }
    }

}