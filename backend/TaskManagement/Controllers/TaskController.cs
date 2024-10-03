using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Repositories.IRepositories;

namespace TaskManagement.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepo;
        public TaskController(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }


        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskRepo.GetAllTasks();

            var response = new ResponseAPI
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Tareas obtenidos exitosamente",
                Data = tasks
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetTaskById(Int32 id)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Message = "No existe una tarea con el ID especificado"
            };

            var task = _taskRepo.GetTaskById(id);

            if (task == null)
            {

                return NotFound(response);
            }

            response.Success = true;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Tarea obtenida exitosamente";
            response.Data = task;

            return Ok(response);
        }

        [HttpGet("state/{stateId}")]
        public IActionResult GetTasksByState(Int32 stateId)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
            };

            if (!_taskRepo.IsValidState(stateId))
            {
                response.Message = "No existe un estado con el ID especificado";
                return NotFound(response);
            }

            var tasks = _taskRepo.GetTasksByState(stateId);

            if (tasks == null)
            {
                response.Message = "No existen tareas con el estado especificado";
                return NotFound(response);
            }

            response.Success = true;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Tareas obtenidas exitosamente";
            response.Data = tasks;
            return Ok(response);
        }


        [HttpPost]
        public IActionResult AddTask(TaskDto task)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al agregar la tarea"
            };

            if (!_taskRepo.IsValidState(task.StateId))
            {
                response.Message = "No existe un estado con el ID especificado";
                return BadRequest(response);
            }

            task.UserId = this.User.FindFirst("userId")?.Value;

            if (_taskRepo.AddTask(task))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.Message = "Tarea agregada exitosamente";
                return Created("", response);
            }

            return BadRequest(response);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateTask(Int32 id, [FromBody] TaskDto task)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al actualizar la tarea"
            };

            if (_taskRepo.GetTaskById(id) == null)
            {
                response.Message = "No existe una tarea con el ID especificado";
                return BadRequest(response);
            }

            if (!_taskRepo.IsValidState(task.StateId))
            {
                response.Message = "No existe un estado con el ID especificado";
                return BadRequest(response);
            }

            if (_taskRepo.UpdateTask(id, task))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Tarea actualizada exitosamente";
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Int32 id)
        {
            var response = new ResponseAPI
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Ha ocurrido un error al eliminar la tarea"
            };

            if (_taskRepo.GetTaskById(id) == null)
            {
                response.Message = "No existe una tarea con el ID especificado";
                return BadRequest(response);
            }

            if (_taskRepo.GetTaskById(id).UserId != this.User.FindFirst("userId")?.Value)
            {
                response.Message = "No puedes eliminar una tarea que no te pertenece";
                return BadRequest(response);
            }

            if (_taskRepo.DeleteTask(id))
            {
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "Tarea eliminada exitosamente";
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}