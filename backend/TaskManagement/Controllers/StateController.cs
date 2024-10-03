using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Repositories.IRepositories;

namespace TaskManagement.Controllers
{
    [Authorize]
    [Route("api/states")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository _stateRepo;
        public StateController(IStateRepository stateRepo)
        {
            _stateRepo = stateRepo;
        }


        [HttpGet]
        public IActionResult GetAllStates()
        {
            var states = _stateRepo.GetAllStates();

            var response = new ResponseAPI
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Estados obtenidos exitosamente",
                Data = states
            };

            return Ok(response);
        }


        [HttpPost]
        public IActionResult AddState(StateDto state)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al agregar el estado"
            };

            if (_stateRepo.CheckState(state.Name))
            {
                response.Message = "Ya existe un estado con ese nombre";
                return BadRequest(response);
            }

            if (_stateRepo.AddState(state))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.Message = "Estado agregado exitosamente";
                response.Data = state;
                return Created("", response);
            }

            return BadRequest(response);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateState(Int32 id, [FromBody] StateDto state)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al actualizar el estado"
            };

            if (_stateRepo.GetStateById(id) == null)
            {
                response.Message = "No existe un estado con el ID especificado";
                return BadRequest(response);
            }

            if (_stateRepo.CheckState(state.Name))
            {
                response.Message = "Ya existe un estado con ese nombre";
                return BadRequest(response);
            }

            if (_stateRepo.UpdateState(id, state))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Estado actualizado exitosamente";
                response.Data = state;
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteState(Int32 id)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al eliminar el estado"
            };

            if(_stateRepo.GetStateById(id) == null)
            {
                response.Message = "No existe un estado con el ID especificado";
                return BadRequest(response);
            }

            if (_stateRepo.DeleteState(id))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Estado eliminado exitosamente";
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}