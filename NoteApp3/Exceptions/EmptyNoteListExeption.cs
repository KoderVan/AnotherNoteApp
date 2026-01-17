using System;


namespace NoteApp3.Exсeptions
{
    internal class EmptyNoteListException: Exception
    {
        public EmptyNoteListException(string message): base(message) { }
    }
}
