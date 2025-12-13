using NoteApp3.Abstractions;
using NoteApp3.Models;
using NoteApp3.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;


namespace NoteApp3.Services
{
    internal class UserService: IUserService
    {
        private const string _filePath = "User.json";
        private readonly List<User> _users;

        public UserService()
        {
            CreateFileIfNotExists();
            _users = new List<User>();
            LoadFile();
        }
        public void CreateFileIfNotExists()
        {
            if (!File.Exists(_filePath))
            {
                using(var writer = new StreamWriter(_filePath))
                {
                    var emptyList = new List<User>();
                    writer.Write(JsonSerializer.Serialize(emptyList));
                }
            }
        }

        public string Login()
        {
            string username = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Вы не ввели логин!");
                Login();
            }
            foreach(User registeredUser in _users)
            {
                if (registeredUser.Name == username)
                {
                    Console.WriteLine("Вы успешно вошли в систему!");
                    return registeredUser.Name;
                }
            }
            Console.WriteLine($"Пользователя с логином {username} не зарегистрировано");
            return "";
        }

        public void Register()
        {
            string username = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Вы не ввели логин!");
                return;
            }
            foreach(User registeredUser in _users)
            {
                if (registeredUser.Name == username)
                {
                    Console.WriteLine("Такой пользователь уже существует!");
                    Console.WriteLine("Введите другое имя пользователя");
                    Register();
                }
            }
            User user = new User()
            {
                Name = username
            };
            _users.Add(user);
            SaveToFile();
            Console.WriteLine($"Пользователь с именем {username} успешно создан");
            
        }

        private void LoadFile()
        {
            var readFile = File.ReadAllText(_filePath);
            var fileContent = JsonSerializer.Deserialize<List<User>>(readFile);
            _users.AddRange(fileContent);
        }

        private void SaveToFile()
        {
            var newFileContent = JsonSerializer.Serialize<List<User>>(_users);
            File.WriteAllText(_filePath, newFileContent);
        }
    }
}
