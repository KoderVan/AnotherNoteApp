using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp3.Exсeptions
{
    internal class InvalidNoteIdExeption: Exception
    {
        public InvalidNoteIdExeption(string message):
            base(message) { }
    }
}
