using NoteApp3.Abstractions;
using NoteApp3.Exсeptions;
using NoteApp3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


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

        public User Login(string username)
        {
            foreach(User registeredUser in _users)
            {
                if (registeredUser.Name == username)
                {
                    User user = registeredUser;
                    return user;
                }
            }  
            return null;
        }

        public void Register(string username)
        {
            foreach(User registeredUser in _users)
            {
                if (registeredUser.Name == username)
                {
                    throw new InvalidUserException("Такой пользователь уже существует!");
                }
            }
            User user = new User()
            {
                Name = username
            };
            _users.Add(user);
            SaveToFile();
            
            
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
