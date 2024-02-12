using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tskmngr
{
    public class TaskManager : ITaskManager
    {
        private readonly TodoDbContext _context;
        private User _currentUser;

        public TaskManager(TodoDbContext context, User currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        public void AddTask()
        {
            Console.Write("Введите заголовок задачи: ");
            string title = Console.ReadLine();

            Console.Write("Это важная задача? (Да/Нет): ");
            string isImportantInput = Console.ReadLine();
            bool isImportant = isImportantInput.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                               isImportantInput.Equals("Да", StringComparison.OrdinalIgnoreCase);

            var lastTaskIndex = _context.Tasks.Any() ? _context.Tasks.Max(t => t.TaskIndex) : 0;

            var task = new Task
            {
                TaskTitle = title,
                IsCompleted = false,
                IsImportant = isImportant,
                TaskIndex = lastTaskIndex + 1,
                UserId = _currentUser.UserId  
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            Console.WriteLine("Задача успешно добавлена!");
        }

        public void ShowTasks()
        {
            var tasks = _context.Tasks.Where(t => t.UserId == _currentUser.UserId).ToList();
            Console.SetCursorPosition(43, 0);
            Console.WriteLine("Список задач: \n");
            Console.SetCursorPosition(36,2);
            Console.WriteLine("N Срочность Статус Задача");
            int n = 3;
            foreach (var task in tasks)
            {
                Console.SetCursorPosition(36, n++);

                if (task.IsImportant)
                {
                    if (task.IsCompleted)
                    {
                        Console.WriteLine($"{task.TaskIndex}.   !!!    [X]   {task.TaskTitle}");
                    }
                    else
                    {
                        Console.WriteLine($"{task.TaskIndex}.   !!!    [ ]   {task.TaskTitle}");

                    }
                }
                else
                {
                    if (task.IsCompleted)
                    {
                        Console.WriteLine($"{task.TaskIndex}.          [X]   {task.TaskTitle}");
                    }
                    else
                    {
                        Console.WriteLine($"{task.TaskIndex}.          [ ]   {task.TaskTitle}");
                    }
                }
            }
        }
        public void MarkTaskAsImportant()
        {
            Console.Write("Введите индекс задачи, которую вы хотите пометить как важную: ");
            if (int.TryParse(Console.ReadLine(), out var importantTaskId))
            {
                var task = _context.Tasks.FirstOrDefault(t => t.TaskID == importantTaskId && t.UserId == _currentUser.UserId);

                if (task != null)
                {
                    task.IsImportant = true;
                    _context.SaveChanges();
                    Console.WriteLine("Задача успешно помечена как важная!");
                }
                else
                {
                    Console.WriteLine("Задача с указанным индексом не найдена или не принадлежит текущему пользователю.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат индекса.");
            }
        }
        public void UpdateTask()
        {
            Console.WriteLine("Введите индекс задачи, которую вы хотите изменить: ");
            if (int.TryParse(Console.ReadLine(), out int updatedTaskId))
            {
                var task = _context.Tasks.FirstOrDefault(t => t.TaskID == updatedTaskId && t.UserId == _currentUser.UserId);

                if (task != null)
                {
                    Console.WriteLine("Введите новое значение задачи: ");
                    task.TaskTitle = Console.ReadLine();

                    try
                    {
                        _context.SaveChanges();
                        Console.WriteLine("Задача успешно изменена!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Задача с указанным индексом не найдена или не принадлежит текущему пользователю.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат индекса.");
            }
        }
        public void MarkTaskAsCompleted()
        {
            Console.Write("Введите индекс задачи, которую хотите пометить как выполненную: ");
            if (int.TryParse(Console.ReadLine(), out int completedTaskId))
            {
                var task = _context.Tasks.FirstOrDefault(t => t.TaskID == completedTaskId && t.UserId == _currentUser.UserId);


                if (task != null)
                {
                    task.IsCompleted = true;

                    try
                    {
                        _context.SaveChanges();
                        Console.WriteLine("Задача успешно помечена как выполненная!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}");
                    }

                }
                else
                {
                    Console.WriteLine("Задача с указанным индексом не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат индекса.");
            }
        }

        public void DeleteTask()
        {
            Console.WriteLine("Введите индекс задачи, которую хотите удалить: ");
            if (int.TryParse(Console.ReadLine(), out int deletedTaskId))
            {
                var task = _context.Tasks.FirstOrDefault(t => t.TaskID == deletedTaskId && t.UserId == _currentUser.UserId);

                if (task != null)
                {
                    _context.Remove(task);

                    try
                    {
                        _context.SaveChanges();
                        Console.WriteLine("Задача успешно удалена!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Задача с указанным индексом не найдена или не принадлежит текущему пользователю.");
                }

                UpdateTaskIndexes();
            }
            else
            {
                Console.WriteLine("Неверный формат индекса.");
            }
        }

        public void UpdateTaskIndexes()
        {
            var tasks = _context.Tasks.OrderBy(task => task.TaskIndex).ToList();

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].TaskIndex = i + 1;
            }

            _context.SaveChanges();
            Console.WriteLine("Порядковые номера задач успешно обновлены!");
        }
    }
}
