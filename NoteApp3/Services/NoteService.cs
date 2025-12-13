using NoteApp3.Abstractions;
using NoteApp3.Models;
using NoteApp3.Services;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

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
            MessageIfEmptyList();
            List<Note> FilteredNotes = _notes.Where(x => x.UserName == CurrentUser).ToList();
            foreach (Note note in FilteredNotes)//Нужно ли удалять заметку из отфильтрованного списка?
            {
                if(note.Id == NoteId)
                {
                    _notes.Remove(note);
                    SaveToFile();
                    return;
                }
            }
            Console.WriteLine($"Заметки с id {NoteId} нет!");
            
        }

        public List<Note> GetAllNotes(string CurrentUser) //Может лучше void?
        {
            int count = 1;
            MessageIfEmptyList();
            List<Note> FilteredNotes = _notes.Where(x=> x.UserName == CurrentUser).ToList();
            Console.WriteLine("Ваши заметки:");
            foreach(Note note in FilteredNotes)
            {
                Console.WriteLine($"Заметка {count}");
                Console.WriteLine($"{note.Title}");
                Console.WriteLine($"{note.Description}");
                count++;
            }
            
            return _notes;
        }

        public void ToggleCompleteness(int NoteId, string CurrentUser)//Надо сделать именно переключение. Подсмотреть у Андрея
        {
            MessageIfEmptyList();
            List<Note> FilteredNotes = _notes.Where(x => x.UserName == CurrentUser).ToList();
            foreach (Note note in FilteredNotes)
            {
                if( note.Id == NoteId)
                {
                    note.IsCompleted = true;
                    SaveToFile();
                    return;
                }
            }
            Console.WriteLine($"Заметки с id {NoteId} нет!");
        }

        public Note UpdateNote(int NoteId, string NewTitle, string NewDescription, string CurrentUser)//Может тоже void?
        {
            MessageIfEmptyList();
            List<Note> FilteredNotes = _notes.Where(x => x.UserName == CurrentUser).ToList();
            foreach (Note note in FilteredNotes)
            {
                if( note.Id == NoteId)
                {
                    note.Title = NewTitle;
                    note.Description = NewDescription;
                    SaveToFile();
                    return note;
                }
                Console.WriteLine($"Заметки с id {NoteId} нет!");
            }
            throw new NotImplementedException();
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
        public void MessageIfEmptyList()
        {
            if (_notes.Count == 0)
            {
                Console.WriteLine("У вас пока нет заметок!");
                return;
            }
        }
    }
}
