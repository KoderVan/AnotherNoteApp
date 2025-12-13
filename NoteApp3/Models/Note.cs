using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp3.Models
{
    internal class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime {  get; set; }

        public bool IsCompleted {  get; set; }
        public string UserName { get; set; }
    }
}
