using tskmngr;


public class ApplicationRunner
{
    private readonly TodoDbContext _context;
    private User _currentUser;
    private readonly TaskManager _taskManager;

    public ApplicationRunner()
    {
        _context = new TodoDbContext();
        _context.Database.EnsureCreated();
        _taskManager = new TaskManager(_context, _currentUser);
    }

    public void Run()
    {
        Console.Write("Введите имя пользователя: ");
        string username = Console.ReadLine();

        // проверка существования пользователя в базе данных
        _currentUser = _context.Users.FirstOrDefault(u => u.UserName == username);

        if (_currentUser == null)
        {
            // создание нового пользователя, если не найден
            _currentUser = new User { UserName = username };
            _context.Users.Add(_currentUser);
            _context.SaveChanges();
        }

        _taskManager.SetCurrentUser(_currentUser);
            while (true)
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 100;
            Console.ForegroundColor = ConsoleColor.Green;
            _taskManager.UpdateTaskIndexes();
            Console.Clear();
            _taskManager.ShowTasks();
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Удалить задачу");
            Console.WriteLine("3. Пометить задачу выполненной");
            Console.WriteLine("4. Пометить задачу важной");
            Console.WriteLine("5. Изменить задачу");
            Console.WriteLine("6. Выйти");

            Console.WriteLine("\n\n\n");
            Console.SetCursorPosition(42, 25);
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            Console.SetCursorPosition(42, 28);
            Console.WriteLine("\r\n                   _ |\\_\r\n                   \\` ..\\\r\n              __,.-\" =__Y=\r\n            .\"        )\r\n      _    /   ,    \\/\\_\r\n     ((____|    )_-\\ \\_-`\r\n  `-----'`-----` `--`");

            switch (choice)
            {
                case "1":
                    _taskManager.AddTask();
                    break;
                case "2":
                    _taskManager.DeleteTask();
                    break;
                case "3":
                    _taskManager.MarkTaskAsCompleted();
                    break;
                case "4":
                    _taskManager.MarkTaskAsImportant();
                    break;
                case "5":
                    _taskManager.UpdateTask();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, выберите снова.");
                    break;
            }
        }
    }
}
