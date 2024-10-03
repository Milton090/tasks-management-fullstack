using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Helpers;
using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Repositories.IRepositories;

namespace TaskManagement.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly AuthHelper _authHelper;
        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _authRepo = authRepo;
            _authHelper = new AuthHelper(config);
        }

        [HttpPost("register")]
        public IActionResult Register(AuthDto user)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al registrar el usuario"
            };

            if (_authRepo.CheckEmail(user.Email) != null)
            {
                response.Message = "Ya existe un usuario con ese email";
                return BadRequest(response);
            }

            String hashedPassword = _authHelper.HashPassword(user.Email, user.Password);
            user.Password = hashedPassword;

            if (_authRepo.Register(user))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.Message = "Usuario registrado exitosamente";
                return Created("", response);
            }

            return BadRequest(response);
        }

        [HttpPost("login")]
        public IActionResult Login(AuthDto user)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al iniciar sesión"
            };

            var userFromDb = _authRepo.CheckEmail(user.Email);

            if (userFromDb == null)
            {
                response.Message = "Credenciales incorrectas";
                return BadRequest(response);
            }

            if (!_authHelper.VerifyPassword(user.Email, user.Password, userFromDb.Password))
            {
                response.Message = "Credeniales incorrectas";
                return BadRequest(response);
            }

            String token = _authHelper.GenerateToken(userFromDb.Id);

            response.Success = true;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Inicio de sesión exitoso";
            response.Data = new { Token = token };
            return Ok(response);
        }
    }
}