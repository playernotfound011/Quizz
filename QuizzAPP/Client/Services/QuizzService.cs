using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace QuizzAPP.Client.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly HttpClient _http;

        public QuizzService(HttpClient http)
        {
            _http = http;
        }
        public List<Quizzes> Quizzs { get; set; } = new List<Quizzes>();

        public async Task<ApiResponse<List<Quizzes>>> GetQuizRegistrationList()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<List<Quizzes>>>("api/quizz");

                if (result != null)
                    Quizzs = result.Data;
                    return result;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<List<Quizzes>>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }           
        }

        public async Task<ApiResponse<QuizzQuestions>> GetQuizzQuestion(int id)
        {
            var result = await _http.GetFromJsonAsync<ApiResponse<QuizzQuestions>>($"api/quizz/{id}");

            if (result != null)
                return result;

            throw new NotImplementedException();
        }

        public async Task<ApiResponse<QuizzQuestionsReg>> QuizRegistration(QuizzQuestionsReg quizzReg)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/quizz", quizzReg);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<QuizzQuestionsReg>>();

                if (apiResponse != null)
                    return apiResponse;

                throw new Exception("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<QuizzQuestionsReg>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }
        }

        public async Task<ApiResponse<QuizzResult>> SendAnswer(QuizzResponse quizzResponse)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/quizz/answers", quizzResponse);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<QuizzResult>>();

                if (apiResponse != null)
                    return apiResponse;

                throw new Exception("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<QuizzResult>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }
        }

        public async Task<ApiResponse<QuizzResultUser>> GetSingleResult(int Id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<QuizzResultUser>>($"api/quizz/result/{Id}");

                if (result != null)
                    return result;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<QuizzResultUser>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }
        }

        public async Task<ApiResponse<List<AllResults>>> GetAllResult(int Id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<List<AllResults>>>($"api/quizz/result/all/{Id}");

                if (result != null)
                    return result;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception)
            {
                var apiResponse = new ApiResponse<List<AllResults>>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }                     
        }

        public async Task<ApiResponse<List<QuizzResultUser>>> GetSingleListResult(int Id, string user)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<List<QuizzResultUser>>>($"api/quizz/result/{Id}/{user}");

                if (result != null)
                    return result;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception)
            {
                var apiResponse = new ApiResponse<List<QuizzResultUser>> ();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }                    
        }

        public async Task<ApiResponse<QuestionsReg>> UpdateQuestion(QuestionUpdate questionUpdate)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("api/quizz", questionUpdate);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<QuestionsReg>>();

                if (apiResponse != null)
                    return apiResponse;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception)
            {
                var apiResponse = new ApiResponse<QuestionsReg>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }            
        }

        public async Task<ApiResponse<QuizzQuestionsUpd>> GetQuizzQuestionUpd(int Id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<QuizzQuestionsUpd>>($"api/quizz/upd/{Id}");

                if (result != null)
                    return result;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception)
            {
                var apiResponse = new ApiResponse<QuizzQuestionsUpd>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }           
        }

        public async Task<ApiResponse<List<Quizzes>>> GetAuthorQuizzList(UserToken token)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/quizz/author", token);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Quizzes>>>();

                if (apiResponse != null)
                    Quizzs = apiResponse.Data;
                    return apiResponse;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception)
            {
                var apiResponse = new ApiResponse<List<Quizzes>>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }
        }
    }
}