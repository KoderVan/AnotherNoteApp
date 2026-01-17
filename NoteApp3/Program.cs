using NoteApp3.Services;


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
