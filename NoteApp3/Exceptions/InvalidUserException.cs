using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp3.Exсeptions
{
    internal class InvalidUserException: Exception
    {
        public InvalidUserException(string message):
            base(message) { }
    }
}
