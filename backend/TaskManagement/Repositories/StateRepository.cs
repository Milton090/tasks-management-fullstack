using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Repositories.IRepositories;

namespace TaskManagement.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly AppDbContext _context;
        public StateRepository(IConfiguration config)
        {
            _context = new AppDbContext(config);
        }

        public IEnumerable<State> GetAllStates()
        {
            var sql = "SELECT Id, Name FROM TaskManagement.dbo.State";
            return _context.LoadData<State>(sql, null);
        }

        public State GetStateById(Int32 id)
        {
            var sql = "SELECT Id, Name FROM TaskManagement.dbo.State WHERE Id = @Id";

            return _context.LoadData<State>(sql, new { Id = id }).FirstOrDefault();
        }

        public Boolean AddState(StateDto state)
        {
            var sql = "INSERT INTO TaskManagement.dbo.State (Name) VALUES (@Name)";

            return _context.Execute(sql, new { Name = state.Name });
        }


        public Boolean UpdateState(Int32 id, StateDto state)
        {
            var sql = "UPDATE TaskManagement.dbo.State SET Name = @Name WHERE Id = @Id";

            return _context.Execute(sql, new { Name = state.Name, Id = id });
        }

        public Boolean DeleteState(Int32 id)
        {
            var sql = "DELETE FROM TaskManagement.dbo.State WHERE Id = @Id";

            return _context.Execute(sql, new { Id = id });
        }
        public Boolean CheckState(String name)
        {
            var sql = "SELECT COUNT(1) FROM TaskManagement.dbo.State WHERE Name = @Name";

            return _context.LoadData<Int32>(sql, new { Name = name }).FirstOrDefault() > 0;
        }
    }
}
