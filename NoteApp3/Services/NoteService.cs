using NoteApp3.Abstractions;
using NoteApp3.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.IO;
using NoteApp3.Exсeptions;

namespace NoteApp3.Services
{
    internal class NoteService : INoteService
    {
        private readonly string _filepath = "Notes.json";
        private readonly List<Note> _notes;
        

        public NoteService()
        {
            CreateFileIfNotExists();
            _notes = new List<Note>();
            LoadFile();
            
        }

        public Note CreateNote(string Title, string Description, string currentUser)
        {
            List<Note> filteredNotes = _notes.Where(x => x.UserName == currentUser).ToList();
            Note note = new Note()
            {
                Id = filteredNotes.Count,
                Title = Title,
                Description = Description,
                CreationDateTime = DateTime.Now,
                IsCompleted = false,
                UserName = currentUser,
            };
            _notes.Add(note);
            SaveToFile();
            
            return note;
        }

        public void DeleteNote(int NoteId, string CurrentUser)
        {
            
            List<Note> filteredNotes = _notes.Where(x => x.UserName == CurrentUser).ToList();

            if (filteredNotes.Count == 0)
            {
                throw new EmptyNoteListException("У вас пока нет ни одной заметки");
            }

            foreach (Note note in filteredNotes)
            {
                if(note.Id == NoteId)
                {
                    _notes.Remove(note);
                    SaveToFile();
                    return;
                }
            }
            throw new InvalidNoteIdExeption($"Заметки с Id {NoteId} нет");
        }

        public List<Note> GetAllNotes(string CurrentUser)
        {
                        
            List<Note> filteredNotes = _notes.Where(note => note.UserName == CurrentUser).ToList();
            
            return filteredNotes;
        }

        public void ToggleCompleteness(int NoteId, string CurrentUser)
        {
            List<Note> filteredNotes = _notes.Where(x => x.UserName == CurrentUser).ToList();

            if (filteredNotes.Count == 0)
            {
                throw new EmptyNoteListException("У вас пока нет ни одной заметки");
            }

            foreach (Note note in filteredNotes)
            {
                if( note.Id == NoteId)
                {
                    bool completed = note.IsCompleted ? false : true;
                    note.IsCompleted = completed;
                    SaveToFile();
                    return;
                }
            }
            throw new InvalidNoteIdExeption($"Заметки с Id {NoteId} нет");
        }

        public Note UpdateNote(int NoteId, string NewTitle, string NewDescription, string CurrentUser)
        {
               
            List<Note> filteredNotes = _notes.Where(x => x.UserName == CurrentUser).ToList();
            if (filteredNotes.Count == 0)
            {
                throw new EmptyNoteListException("У вас пока нет ни одной заметки");
            }
            foreach (Note note in filteredNotes)
            {
                if( note.Id == NoteId)
                {
                    note.Title = NewTitle;
                    note.Description = NewDescription;
                    SaveToFile();
                    return note;
                }
            }
            throw new InvalidNoteIdExeption($"Заметки с Id {NoteId} нет");
        }
        public void CreateFileIfNotExists()
        {
            if (!File.Exists(_filepath))
            {
                using (var writer = new StreamWriter(_filepath))
                {
                    var emptyList = new List<Note>();
                    writer.Write(JsonSerializer.Serialize(emptyList));
                }
            }
        }

        public void LoadFile()
        {
            var readFile = File.ReadAllText(_filepath);
            var fileContent = JsonSerializer.Deserialize<List<Note>>(readFile);
            _notes.AddRange(fileContent);
        }
        public void SaveToFile()
        {
            var newFileContent = JsonSerializer.Serialize<List<Note>>(_notes);
            File.WriteAllText(_filepath, newFileContent);

        }
        
    }
}
