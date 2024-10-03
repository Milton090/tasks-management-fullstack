using TaskManagement.Models;
using TaskManagement.Models.DTOs;

namespace TaskManagement.Repositories.IRepositories
{
    public interface IAuthRepository
    {
        public Boolean Register(AuthDto user);
        public Auth CheckEmail(String email);
    }
}
