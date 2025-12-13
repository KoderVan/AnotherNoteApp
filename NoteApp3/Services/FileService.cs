using NoteApp3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NoteApp3.Services
{
    internal class FileService
    {
        public void CreateFileIfNotExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                using (var writer = new StreamWriter(filePath))
                {
                    var emptyList = new List<User>();
                    writer.Write(JsonSerializer.Serialize(emptyList));
                }
            }
        }

        private void LoadFile(string filePath, List<object> list)
        {
            var readFile = File.ReadAllText(filePath);
            var fileContent = JsonSerializer.Deserialize<List<User>>(readFile);
            list.AddRange(fileContent);
        }
    }
}
