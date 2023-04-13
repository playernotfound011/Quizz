using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;
using System.Threading.Tasks;

namespace QuizzAPP.Client.Services.Interfaces
{
    public interface IQuizzService
    {
        List<Quizzes> Quizzs { get; set; }
        Task<ApiResponse<QuizzQuestionsReg>> QuizRegistration(QuizzQuestionsReg quizzReg);
        Task<ApiResponse<List<Quizzes>>> GetQuizRegistrationList();
        Task<ApiResponse<QuizzQuestions>> GetQuizzQuestion(int Id);
        Task<ApiResponse<QuizzResult>> SendAnswer(QuizzResponse quizzResponse);
        Task<ApiResponse<QuizzResultUser>> GetSingleResult(int Id);
        Task<ApiResponse<List<AllResults>>> GetAllResult(int Id);
        Task<ApiResponse<List<QuizzResultUser>>> GetSingleListResult(int Id, string user);
        Task<ApiResponse<QuestionsReg>> UpdateQuestion(QuestionUpdate questionUpdate);
        Task<ApiResponse<QuizzQuestionsUpd>> GetQuizzQuestionUpd(int Id);
        Task<ApiResponse<List<Quizzes>>> GetAuthorQuizzList(UserToken token);
    }
}
