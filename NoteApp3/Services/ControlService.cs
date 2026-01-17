using NoteApp3.Abstractions;
using NoteApp3.Models;
using NoteApp3.Exсeptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteApp3.Services
{
    internal class ControlService : IControlService
    {
        private readonly IUserService _userService;
        private readonly INoteService _noteService;
        private bool _online = true;
        private string _user = null;

        public ControlService(IUserService userService, INoteService noteService)
        {
            _userService = userService;
            _noteService = noteService;
        }
        public void PrintHello()
        {
            Console.WriteLine("Добро пожаловать в приложение заметки!");
            Console.WriteLine("Чтобы узнать доступные команды введите \"help\" ");
        }

        public void Run()
        {
            var commands = new Dictionary<string, Action>
            {
                {"help", PrintAllowedCommands},
                {"register", UIRegister},
                {"login", UILogin},
                {"view notes", UIGetAllNotes},
                {"create note", UICreateNote},
                {"update note", UIUpdateNote},
                {"delete note", UIDeleteNote},
                {"complete task", UIToggleComplitness},
                {"exit", UIExit},
            };

            PrintHello();
            while (_online)
            {
                Console.WriteLine("Введите команду:");
                string input = Console.ReadLine().Trim().ToLower();
                if (commands.ContainsKey(input))
                {
                    try
                    {
                        commands[input]();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестная команда!");
                }
            }
        }

        private void PrintAllowedCommands()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("Зарегистрировать пользователя: Register");
            Console.WriteLine("Войти в систему: Login");
            Console.WriteLine("Создать заметку: Create Note");
            Console.WriteLine("Посмотреть все свои заметки: View Notes");
            Console.WriteLine("Изменить заметку: Update Note");
            Console.WriteLine("Удалить заметку: Delete Note");
            Console.WriteLine("Изменить статус заметки: Complete task");
            Console.WriteLine("Выйти из программы: Exit");
        }

        private bool CheckIfUserLoggedIn()
        {
            if (_user == null)
            {
                return false;
            }
            return true;
        }
        private void UIRegister()
        {
            Console.WriteLine("Введите логин, который хотите зарегистрировать:");
            string username = Console.ReadLine().Trim();
            if (username == null) throw new EmptyInputException("Вы не ввели имя пользователя");
            try
            {
                _userService.Register(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось зарегистрировать пользоветля");
                return;
            }
            Console.WriteLine($"Пользователь с именем {username} успешно создан");
        }
        private void UILogin()
        {
            Console.WriteLine("Введите свой логин:");
            string username = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(username)) throw new EmptyInputException("Строка ввода пустая");
            User user  = _userService.Login(username);
            if (user == null) throw new InvalidUserException($"Пользователя {username} не зарегистрировано");
            Console.WriteLine("Вы успешно залогинились!");
            _user = user.Name;
        }
        private void UIGetAllNotes()
        {
            if (!CheckIfUserLoggedIn()) throw new InvalidUserException("Вы не вошли в систему");

            List<Note> notes = _noteService.GetAllNotes(_user);
            Console.WriteLine("Ваши заметки:");
            foreach (Note note in notes)
            {
                string completed = note.IsCompleted ? "V" : "X";
                Console.WriteLine("-------------");
                Console.WriteLine($"{completed} Заметка № {note.Id}");
                Console.WriteLine($"{note.Title}");
                Console.WriteLine($"{note.Description}");
            }
            return;
        }
        private void UICreateNote()
        {
            if(!CheckIfUserLoggedIn()) throw new InvalidUserException("Вы не вошли в систему");
            Console.WriteLine("Введите заголовок заметки:");
            string Title = Console.ReadLine();
            Console.WriteLine("Введите описание заметки:");
            string Description = Console.ReadLine();
            try
            {
                Note note = _noteService.CreateNote(Title, Description, _user);
                Console.WriteLine($"Заметка с id {note.Id} создана!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка, не удалось создать заметку");
                return;
            }
        }
        private void UIUpdateNote()
        {
            int NoteId;
            if (!CheckIfUserLoggedIn()) throw new InvalidUserException("Вы не вошли в систему");
            Console.WriteLine("Введите Id заметки для изменения:");
            try
            {
                NoteId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Введите число!");
                return;
            }
            List<Note> notes = _noteService.GetAllNotes(_user);
            if (notes.Count == 0) throw new EmptyNoteListException("У вас пока нет ни одной заметки!");
            if (!notes.Where(x => x.Id == NoteId).Any()) throw new InvalidNoteIdExeption("Заметки с таким id нет!");
            Console.WriteLine("Введите  новый заголовок заметки:");
            string NewTitle = Console.ReadLine();
            Console.WriteLine("Введите новое описание заметки:");
            string NewDescription = Console.ReadLine();
            _noteService.UpdateNote(NoteId, NewTitle, NewDescription, _user);
        }
        private void UIDeleteNote()
        {
            int NoteId;
            if (!CheckIfUserLoggedIn()) throw new InvalidUserException("Вы не вошли в систему");
            Console.WriteLine("Введите Id заметки для удаления:");
            try
            {
                NoteId = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Введите число!");
                return;
            }
            List<Note> notes = _noteService.GetAllNotes(_user);
            if (notes.Count == 0) throw new EmptyNoteListException("У вас пока нет ни одной заметки!");
            if (!notes.Where(x => x.Id == NoteId).Any()) throw new InvalidNoteIdExeption("Заметки с таким id нет!");
            _noteService.DeleteNote(NoteId, _user);
        }
        private void UIToggleComplitness()
        {
            int NoteId;
            if (!CheckIfUserLoggedIn()) throw new InvalidUserException("Вы не вошли в систему");
            Console.WriteLine("Введите Id заметки для изменения отметки выполнения:");
            try
            {
                NoteId = Convert.ToInt32(Console.ReadLine());
                _noteService.ToggleCompleteness(NoteId, _user);
            }
            catch (InvalidNoteIdExeption ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
        private void UIExit()
        {
            _online = false;
            Console.WriteLine("Программа завершила работу");
        }

    }
}
