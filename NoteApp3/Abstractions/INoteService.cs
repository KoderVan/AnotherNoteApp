using NoteApp3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp3.Abstractions
{
    internal interface INoteService
    {
        Note CreateNote(string Title, string Description, string CurrentUser);
        List<Note> GetAllNotes(string CurrentUser);
        Note UpdateNote(int NoteId, string Title, string Description, string CurrentUser);
        void DeleteNote(int NoteId, string CurrentUser);

        void ToggleCompleteness(int NoteId, string CurrentUser);
    }
}
