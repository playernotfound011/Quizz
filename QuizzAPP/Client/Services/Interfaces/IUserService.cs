using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;

namespace QuizzAPP.Client.Services.Interfaces
{
    public interface IUserService
    {
        List<UserQuizz> UserQuizzs { get; set; }
        Task<ApiResponse<LoginInfo>> UserLogin(UserLogin user);
        Task<ApiResponse<UserQuizz>> UserRegistration(UserRegistration userReg);
        Task<ApiResponse<List<UserQuizz>>> GetUsers();
        Task<ApiResponse<UserType>> GetUserType(UserToken token);
    }
}
