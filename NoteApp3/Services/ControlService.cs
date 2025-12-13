using NoteApp3.Abstractions;
using NoteApp3.Models;
using System;
using System.Collections.Generic;

namespace NoteApp3.Services
{
    internal class ControlService : IControlService
    {
        private readonly IUserService _userService;
        private readonly INoteService _noteService;
        private bool _online = true;
        private string User = null;

        public ControlService(IUserService userService, INoteService noteService)//Ещё раз уточнить, почему передаю интерфейсы
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
                {"viewnotes", UIGetAllNotes},
                {"createnote", UICreateNote},
                {"updatenote", UIUpdateNote},
                {"deletenote", DeleteNote},
                {"exit", UIExit},
            };

            PrintHello();
            while (_online)
            {
                Console.WriteLine("Введите команду:");
                string input = Console.ReadLine().Trim().ToLower();
                if (commands.ContainsKey(input))
                {
                    commands[input]();
                }
                else
                {
                    Console.WriteLine("Неизвестная команда!");
                }
            }
        }

        public void PrintAllowedCommands()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("Зарегистрировать пользователя: Register");
            Console.WriteLine("Войти в систему: Login");
            Console.WriteLine("Создать заметку: CreateNote");
            Console.WriteLine("Посмотреть все свои заметки: ViewNotes");
            Console.WriteLine("Изменить заметку: UpdateNote");
            Console.WriteLine("Удалить заметку: DeleteNote");
            Console.WriteLine("Выйти из программы: Exit");
        }

        public void CheckIfUserLoggedIn()
        {
            if (User == null)
            {
                Console.WriteLine("Вы не вошли в систему!");
                return;
            }
        }
        public void UIRegister()
        {
            Console.WriteLine("Введите логин, который хотите зарегистрировать:");
            _userService.Register();
        }
        public void UILogin()
        {
            Console.WriteLine("Введите свой логин:");
            User = _userService.Login();

        }
        public void UIGetAllNotes()
        {
            CheckIfUserLoggedIn();

            _noteService.GetAllNotes(User);
            return;
        }
        public void UICreateNote()
        {
            CheckIfUserLoggedIn();// как выходить из следуещей функции?
            Console.WriteLine("Введите заголовок заметки:");
            string Title = Console.ReadLine();
            Console.WriteLine("Введите описание заметки:");
            string Description = Console.ReadLine();
            try
            {
                Note note = _noteService.CreateNote(Title, Description, User);
                Console.WriteLine($"Заметка с id {note.Id} создана!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка, не удалось создать заметку");
                return;
            }
        }
        public void UIUpdateNote()
        {
            int NoteId;
            CheckIfUserLoggedIn();
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

            Console.WriteLine("Введите  новый заголовок заметки:");
            string NewTitle = Console.ReadLine();
            Console.WriteLine("Введите новое описание заметки:");
            string NewDescription = Console.ReadLine();
            _noteService.UpdateNote(NoteId, NewTitle, NewDescription, User);
        }
        public void DeleteNote()
        {
            int NoteId;
            CheckIfUserLoggedIn();
            Console.WriteLine("Введите Id заметки для изменения:");
            try
            {
                NoteId = Convert.ToInt32(Console.ReadLine());
                _noteService.DeleteNote(NoteId, User);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Введите число!");
                return;
            }
        }
        public void UIToggleComplitness()
        {
            int NoteId;
            CheckIfUserLoggedIn();
            Console.WriteLine("Введите Id заметки для изменения отметки выполнения:");
            try
            {
                NoteId = Convert.ToInt32(Console.ReadLine());
                _noteService.ToggleCompleteness(NoteId, User);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Введите число!");
                return;
            }
        }
        public void UIExit()
        {
            _online = false;
            Console.WriteLine("Программа завершила работу");
        }

    }
}
