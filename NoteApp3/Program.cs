using NoteApp3.Abstractions;
using NoteApp3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace NoteApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserService userService = new UserService();
            NoteService noteService = new NoteService();
            ControlService controlService = new ControlService(userService, noteService);
            controlService.Run();

        }
    }
}
