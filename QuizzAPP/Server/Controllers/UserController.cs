using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizzAPP.Server.DataDB;
using QuizzAPP.Shared;
using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;

namespace QuizzAPP.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUsersService userSevice, ILogger<UserController> logger)
        {
            _userService = userSevice;
            _logger = logger;
        }

        [HttpGet, Authorize(Roles = "ADM")]
        public async Task<ActionResult<ApiResponse<List<UserQuizz>>>> GetUsers()
        {
            try
            {
                ApiResponse<List<UserQuizz>> response = await _userService.GetUsers();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController/GetUsers: " + ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserQuizz>>> UserRegistration([FromBody] UserRegistration userReg)
        {
            try
            {
                ApiResponse<UserQuizz> response = new ApiResponse<UserQuizz>();

                if (ModelState.IsValid)
                {
                    var serviceResponse = await _userService.UserRegistration(userReg);
                    response.Data = serviceResponse.Data;

                    if (serviceResponse.Status == 1)
                    {
                        response.Message = "¡Registro exitoso!";
                    }
                    else if (serviceResponse.Status == 2)
                    {
                        response.Message = "El nombre de usuario ya existe.";
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }
                }
                else
                {
                    response.Data = null;
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }               
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController/UserRegistration: " + ex.Message, ex);
                return StatusCode(500);
            }         
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginInfo>>> UserLogin([FromBody] UserLogin userLog)
        {
            try
            {
                ApiResponse<LoginInfo> response = new ApiResponse<LoginInfo>();

                if (ModelState.IsValid)
                {
                    var serviceResponse = await _userService.UserLogin(userLog);
                    response.Data = serviceResponse.Data;

                    if (serviceResponse.Status == 1)
                    {
                        response.Message = "Usuario Valido";
                        return Ok(response);
                    }
                    else if (serviceResponse.Status == 2)
                    {
                        response.Data = null;
                        response.Message = "Usuario & clave incorrecta.";
                        return NotFound(response);
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }
                }
                else
                {
                    response.Data = null;
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController/UserLogin: " + ex.Message, ex);
                return StatusCode(500);
            }            
        }

        [HttpPost("role"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<UserType>>> UserType([FromBody] UserToken token)
        {
            try
            {
                ApiResponse<UserType> response = new ApiResponse<UserType>();

                if (ModelState.IsValid)
                {
                    var serviceResponse = await _userService.GetUserType(token);
                    response.Data = serviceResponse.Data;

                    if (serviceResponse.Status == 1)
                    {
                        response.Message = "Usuario Valido";
                        return Ok(response);
                    }
                    else if (serviceResponse.Status == 2)
                    {
                        response.Data = null;
                        response.Message = "Usuario no existe.";
                        return NotFound(response);
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }
                }
                else
                {
                    response.Data = null;
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UserController/UserType: " + ex.Message, ex);
                return StatusCode(500);
            }           
        }
    }
}

