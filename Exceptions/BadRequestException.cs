using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string name)
            : base($"Request \"{name}\" so bad.") { }
    }
}
