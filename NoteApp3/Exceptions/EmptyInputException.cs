using System;


namespace NoteApp3.Exсeptions
{
    internal class EmptyInputException: Exception
    {
        public EmptyInputException(string message): base(message) { }
    }
}
