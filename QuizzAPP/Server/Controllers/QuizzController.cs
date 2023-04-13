using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizzAPP.Client.Services;
using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;
using System.Data;
using System.Resources;

namespace QuizzAPP.Server.Controllers
{
    [Route("api/[controller]")]
    public class QuizzController : Controller
    {
        private readonly IQuizzService _quizzService;
        private readonly ILogger<QuizzController> _logger;
        private static readonly ResourceManager _resourceManager = new ResourceManager("Namespace.Messages", typeof(QuizzService).Assembly);
        public QuizzController(IQuizzService quizzSevice, ILogger<QuizzController> logger)
        {
            _quizzService = quizzSevice;
            _logger = logger;
        }

        [HttpGet, Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<List<Quizzes>>>> GetQuizz()
        {
            try
            {
                ApiResponse<List<Quizzes>> response = new ApiResponse<List<Quizzes>>();

                var serviceResponse = await _quizzService.GetQuizRegistrationList();
                response.Data = serviceResponse.Data;

                if (serviceResponse.Status == 1)
                {
                    response.Message = "Consulta exitosa.";
                    return Ok(response);
                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "No hay cuestionarios creados.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/GetQuizz: " + ex.Message, ex);
                return StatusCode(500);
            }            
        }

        [HttpPost, Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<QuizzQuestionsReg>>> RegQuizz([FromBody] QuizzQuestionsReg quizzReg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApiResponse<QuizzQuestionsReg> response = new ApiResponse<QuizzQuestionsReg>();

                    var serviceResponse = await _quizzService.QuizRegistration(quizzReg);

                    response.Data = serviceResponse.Data;

                    if (serviceResponse.Status == 1)
                    {
                        response.Message = "¡Registro de cuestionario exitoso!";
                        return Ok(response);
                    }
                    else if (serviceResponse.Status == 2)
                    {
                        response.Message = "El nombre del cuestionario ya existe.";
                        return BadRequest(response);
                    }
                    {
                        response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }
                }
                else
                {
                    var response = new ApiResponse<QuizzQuestionsReg>();
                    response.Data = null;
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/QuizzQuestionsReg: " + ex.Message, ex);
                return StatusCode(500);
            }
                
        }

        [HttpGet("{id}"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<QuizzQuestions>>> GetSingleQuizz(int id)
        {
            try
            {
                ApiResponse<QuizzQuestions> response = new ApiResponse<QuizzQuestions>();

                var serviceResponse = await _quizzService.GetQuizzQuestion(id);

                response.Data = serviceResponse.Data;

                if (serviceResponse.Status == 1)
                {
                    response.Message = "Consulta exitosa!";
                    return Ok(response);
                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "No se encontro el Quizz.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/GetSingleQuizz: " + ex.Message, ex);
                return StatusCode(500);
            }                  
        }

        [HttpGet("upd/{id}"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<QuizzQuestionsUpd>>> GetSingleQuizzUpd(int id)
        {
            try
            {
                ApiResponse<QuizzQuestionsUpd> response = new ApiResponse<QuizzQuestionsUpd>();
                var serviceResponse = await _quizzService.GetQuizzQuestionUpd(id);

                response.Data = serviceResponse.Data;

                if (serviceResponse.Status == 1)
                {
                    response.Message = "Quizz Actualizado exitosamente!";
                    return Ok(response);

                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "No se encontro el Quizz.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/GetSingleQuizzUpd: " + ex.Message, ex);
                return StatusCode(500);
            }           
        }

        [HttpPost("answers"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<QuizzResult>>> SendQuestions([FromBody] QuizzResponse quizzResponse)
        {
            try
            {
                ApiResponse<QuizzResult> response = new ApiResponse<QuizzResult>();

                if (ModelState.IsValid)
                {
                    var serviceResponse = await _quizzService.SendAnswer(quizzResponse);

                    response.Data = serviceResponse.Data;

                    if (serviceResponse.Status == 1)
                    {
                        response.Message = "Respuestas enviadas exitosamente!";
                        return Ok(response);
                    }
                    else if (serviceResponse.Status == 2)
                    {
                        response.Message = "No se encontro el Quizz.";
                        return NotFound(response);
                    }
                    else
                    {
                        response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/SendQuestions: " + ex.Message, ex);
                return StatusCode(500); ;
            }           
        }

        [HttpGet("result/{id}"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<QuizzQuestions>>> GetSingleResult(int id)
        {
            try
            {
                ApiResponse<QuizzResultUser> response = new ApiResponse<QuizzResultUser>();

                var serviceResponse = await _quizzService.GetSingleResult(id);

                response.Data = serviceResponse.Data;

                if (serviceResponse.Status == 1)
                {
                    response.Message = "Resultado obtenido exitosamente!";
                    return Ok(response);

                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "Resultado no encontrado.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/GetSingleResult: " + ex.Message, ex);
                return StatusCode(500); ;
            }                   
        }

        [HttpGet("result/all/{id}"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<List<AllResults>>>> GetAllResult(int id)
        {
            try
            {
                ApiResponse<List<AllResults>> response = new ApiResponse<List<AllResults>>();

                var serviceResponse = await _quizzService.GetAllResult(id);

                response.Data = serviceResponse.Data;
                if (serviceResponse.Status == 1)
                {
                    response.Message = "Consulta exitosa!";
                    return Ok(response);
                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "Resultado no encontrado.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/GetAllResults: " + ex.Message, ex);
                return StatusCode(500); ;
            }           
        }

        [HttpGet("result/{id}/{user}"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<List<QuizzResultUser>>>> GetAllResult(int id, string user)
        {
            try
            {
                ApiResponse<List<QuizzResultUser>> response = new ApiResponse<List<QuizzResultUser>>();

                var serviceResponse = await _quizzService.GetSingleListResult(id, user);

                response.Data = serviceResponse.Data;

                if (serviceResponse.Status == 1)
                {
                    response.Message = "Consulta exitosa!";
                    return Ok(response);
                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "Resultado no encontrado.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex) {
                _logger.LogError("QuizzController/GetAllResult: " + ex.Message, ex); 
                return StatusCode(500);  
            }          
        }

        [HttpPut, Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<QuestionsReg>>> UpdateQuestion([FromBody] QuestionUpdate questionUpdate)
        {
            try
            {
                ApiResponse<QuestionsReg> response = new ApiResponse<QuestionsReg>();

                if (ModelState.IsValid)
                {
                    var serviceResponse = await _quizzService.UpdateQuestion(questionUpdate);

                    response.Data = serviceResponse.Data;

                    if (serviceResponse.Status == 1)
                    {
                        response.Message = "Cuestionario modificado!";
                        return Ok(response);

                    }
                    else if (serviceResponse.Status == 2)
                    {
                        response.Message = "Cuestionario no encontrado.";
                        return NotFound(response);
                    }
                    else
                    {
                        response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/UpdateQuestion: " + ex.Message, ex);
                return StatusCode(500);
            }          
        }

        [HttpPost("author"), Authorize(Roles = "ADM,USR")]
        public async Task<ActionResult<ApiResponse<List<Quizzes>>>> GetAuthorQuizzList([FromBody] UserToken token)
        {
            try
            {
                ApiResponse<List<Quizzes>> response = new ApiResponse<List<Quizzes>>();

                var serviceResponse = await _quizzService.GetAuthorQuizzList(token);
                response.Data = serviceResponse.Data;

                if (serviceResponse.Status == 1)
                {
                    response.Message = "Consulta exitosa.";
                    return Ok(response);
                }
                else if (serviceResponse.Status == 2)
                {
                    response.Message = "No hay cuestionarios creados.";
                    return NotFound(response);
                }
                else
                {
                    response.Message = "¡Ocurrio un problema!, intenta nuevamente.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("QuizzController/GetAuthorQuizzList: " + ex.Message, ex);
                return StatusCode(500);
            }          
        }
    }
}
