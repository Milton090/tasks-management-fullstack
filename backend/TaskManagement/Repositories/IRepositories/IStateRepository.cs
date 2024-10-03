using TaskManagement.Models;
using TaskManagement.Models.DTOs;

namespace TaskManagement.Repositories.IRepositories
{
    public interface IStateRepository
    {
        public IEnumerable<State> GetAllStates();
        public State GetStateById(Int32 id);
        public Boolean AddState(StateDto state);
        public Boolean CheckState(String name);
        public Boolean UpdateState(Int32 id, StateDto state);
        public Boolean DeleteState(Int32 id);
    }
}
