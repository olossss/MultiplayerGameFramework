using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ServerException : Exception
    {
        public ServerException() : base(){}

        public ServerException(String message) : base(message) { }

        public ServerException(String message, Exception inner) : base(message, inner) { }
    }

    public class ClientException : Exception
    {
        public ClientException() : base() { }

        public ClientException(String message) : base(message) { }

        public ClientException(String message, Exception inner) : base(message, inner) { }
    }
}
