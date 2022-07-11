using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDB.Exceptions
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException()
            : base($"Server error.") { }
    }
}
