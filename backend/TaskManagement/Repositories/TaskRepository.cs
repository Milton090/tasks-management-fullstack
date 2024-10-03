using TaskManagement.Data;
using TaskManagement.Models.DTOs;
using TaskManagement.Repositories.IRepositories;

namespace TaskManagement.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(IConfiguration config)
        {
            _context = new AppDbContext(config);
        }

        public IEnumerable<Models.Task> GetAllTasks()
        {
            var sql = "SELECT t.Id, t.Title, t.StateId, t.Created, t.Updated, t.UserId, s.Name AS StateName " +
                        "FROM TaskManagement.dbo.Task AS t " +
                        "JOIN TaskManagement.dbo.State AS s " +
                        "ON s.Id = t.StateId";
            return _context.LoadData<Models.Task>(sql, null);
        }

        public TaskManagement.Models.Task GetTaskById(Int32 id)
        {
            var sql = "SELECT Id, Title, StateId, Created, Updated, UserId FROM TaskManagement.dbo.Task WHERE Id = @Id";

            return _context.LoadData<Models.Task>(sql, new { Id = id }).FirstOrDefault();
        }

        public Boolean AddTask(TaskDto task)
        {
            var sql = "INSERT INTO TaskManagement.dbo.Task (Title, StateId, Created, Updated, UserId) " +
                        "VALUES (@Title, @StateId, GETDATE(), GETDATE(), @UserId)";

            return _context.Execute(sql, new { task.Title, task.StateId, task.UserId  });
        }

        public Boolean UpdateTask(Int32 id, TaskDto task)
        {
            var sql = "UPDATE TaskManagement.dbo.Task " +
                        "SET Title = @Title, " +
                        "StateId = @StateId, " +
                        "Updated = GETDATE() " +
                        "WHERE Id = @Id";

            return _context.Execute(sql, new { task.Title, task.StateId, Id = id });
        }

        public Boolean DeleteTask(Int32 id)
        {
            var sql = "DELETE FROM TaskManagement.dbo.Task WHERE Id = @Id";

            return _context.Execute(sql, new { Id = id });
        }

        public Boolean IsValidState(Int32 id)
        {
            var sql = "SELECT COUNT(1) FROM TaskManagement.dbo.State WHERE Id = @Id";

            return _context.LoadData<Int32>(sql, new { Id = id }).FirstOrDefault() > 0;
        }
    }
}
