using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;

namespace QuizzAPP.Server.Services.Interfaces
{
    public interface IQuizzService
    {
        Task<ServiceResponse<QuizzQuestionsReg>> QuizRegistration(QuizzQuestionsReg quizzReg);
        Task<ServiceResponse<List<Quizzes>>> GetQuizRegistrationList();
        Task<ServiceResponse<QuizzQuestions>> GetQuizzQuestion(int Id);
        Task<ServiceResponse<QuizzResult>> SendAnswer(QuizzResponse quizzResponse);
        Task<ServiceResponse<QuizzResultUser>> GetSingleResult(int Id);
        Task<ServiceResponse<List<AllResults>>> GetAllResult(int Id);
        Task<ServiceResponse<List<QuizzResultUser>>> GetSingleListResult(int Id, string user);
        Task<ServiceResponse<QuestionsReg>> UpdateQuestion(QuestionUpdate questionUpdate);
        Task<ServiceResponse<QuizzQuestionsUpd>> GetQuizzQuestionUpd(int Id);
        Task<ServiceResponse<List<Quizzes>>> GetAuthorQuizzList(UserToken token);
    }
}
