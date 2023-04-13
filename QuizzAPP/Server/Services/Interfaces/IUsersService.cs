using QuizzAPP.Shared;
using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;

namespace QuizzAPP.Server.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<UserQuizz>> UserRegistration(UserRegistration user);
        Task<ServiceResponse<LoginInfo>> UserLogin(UserLogin user);
        Task<ApiResponse<List<UserQuizz>>> GetUsers();
        Task<ServiceResponse<UserType>> GetUserType(UserToken token);
    }
}
