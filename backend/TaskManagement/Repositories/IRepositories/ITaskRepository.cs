using TaskManagement.Models.DTOs;

namespace TaskManagement.Repositories.IRepositories
{
    public interface ITaskRepository
    {
        public IEnumerable<Models.Task> GetAllTasks();
        public Models.Task GetTaskById(Int32 id);
        public Boolean AddTask(TaskDto task);
        public Boolean UpdateTask(Int32 id, TaskDto task);
        public Boolean DeleteTask(Int32 id);
        public Boolean IsValidState(Int32 stateId);
    }
}
