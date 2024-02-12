namespace tskmngr
{
    public class Task
    {
        public int TaskID { get; set; } 
        public string? TaskTitle { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public int TaskIndex { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }

    //думаю, впоследствии класс taskmanager можно разделить на два - один работает с окном, выводом и визуализацией в целом, второй работает конкретно с задачами
    interface ITaskManager
    {
        void ShowTasks();
        void AddTask();
        void MarkTaskAsImportant();
        void MarkTaskAsCompleted();
        void UpdateTask();
        void DeleteTask();
        void UpdateTaskIndexes(); //при удалении задачи, которая не стоит в конце, индексы смещаются. метод для того, чтобы их сменить на последовательные

    }
}
