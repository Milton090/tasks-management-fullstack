using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Repositories.IRepositories;

namespace TaskManagement.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly AppDbContext _context;
        public AuthRepository(IConfiguration config)
        {
            _context = new AppDbContext(config);
        }
        public Boolean Register(AuthDto user)
        {
            var sql = "INSERT INTO TaskManagement.dbo.Auth (Email, Password) " +
                        "VALUES (@Email, @Password)";

            return _context.Execute(sql, new { user.Email, user.Password });

        }


        public Auth CheckEmail(String email)
        {
            var sql = "SELECT Id, Email, Password FROM TaskManagement.dbo.Auth WHERE Email = @Email";

            return _context.LoadData<Auth>(sql, new { Email = email }).FirstOrDefault();
        }

    }
}
