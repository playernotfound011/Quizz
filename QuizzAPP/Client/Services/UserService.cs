using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace QuizzAPP.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }
        public List<UserQuizz> UserQuizzs { get; set; } = new List<UserQuizz>();

        public async Task<ApiResponse<List<UserQuizz>>> GetUsers()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<List<UserQuizz>>>("api/user");

                if (result != null)
                    UserQuizzs = result.Data;
                    return result;

                throw new Exception("Failed to retrieved API result");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<List<UserQuizz>> ();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }
        }

        public async Task<ApiResponse<UserType>> GetUserType(UserToken token)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/user/role", token);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserType>>();

                if (apiResponse != null)
                    return apiResponse;

                throw new Exception("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<UserType>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";

                return apiResponse;
            }
        }

        public async Task<ApiResponse<LoginInfo>> UserLogin(UserLogin userLog)
        {
            ApiResponse<LoginInfo> error = new ApiResponse<LoginInfo>();

            try
            {
                var response = await _http.PostAsJsonAsync("api/user/login", userLog);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginInfo>>();

                if (apiResponse != null)
                    return apiResponse;

                throw new Exception("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<LoginInfo>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";//"Error occurred: " + ex.Message;

                return apiResponse;
            }         
        }

        public async Task<ApiResponse<UserQuizz>> UserRegistration(UserRegistration userReg)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/user", userReg);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserQuizz>>();

                if (apiResponse != null)
                    return apiResponse;

                throw new Exception("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<UserQuizz>();
                apiResponse.Data = null;
                apiResponse.Message = "Ocurrio un Error";//"Error occurred: " + ex.Message;

                return apiResponse;
            }
        }
    }
}
