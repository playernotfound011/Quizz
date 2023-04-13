using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QuizzAPP.Client.Pages;
using QuizzAPP.Client.Services;
using QuizzAPP.Server.DataDB;
using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;

namespace QuizzAPP.Server.Services
{
    public class QuizzServices : IQuizzService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<QuizzServices> _logger;
        private static readonly ResourceManager _resourceManager = new ResourceManager("Namespace.Messages", typeof(QuizzService).Assembly);
        public readonly QuizzBdContext _context;
        public readonly List<QuizzReg> empty = new List<QuizzReg>();

        public QuizzServices(QuizzBdContext context, IConfiguration configuration, ILogger<QuizzServices> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<Quizzes>>> GetQuizRegistrationList()
        {
            ServiceResponse<List<Quizzes>> resp = new ServiceResponse<List<Quizzes>>();
            List<Quizzes> empty = new List<Quizzes>();

            try
            {
                List<Quizzes> quizzes= await (from a in _context.Quizzes
                                               join b in _context.Users
                                               on a.CreatorId equals b.Id                                              
                                               select new Quizzes()
                                               {
                                                   Id= a.Id,
                                                   Name= a.Name,
                                                   Creator = b.Username,
                                                   CreationDate = b.CreationDate.HasValue ? b.CreationDate.Value.ToString("dd/MM/yyyy HH:mm") : "Sin Fecha"
                                               }).ToListAsync();

                if (quizzes.Any())
                {
                    resp.Data = quizzes;
                    resp.Status = 1;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                    // resp.Message = _resourceManager.GetString("NoQuizzes") ?? "";
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.Status = 0;
                _logger.LogError("QuizzService.GetQuizRegistrationList: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<QuizzQuestionsReg>> QuizRegistration(QuizzQuestionsReg quizzReg)
        {
            ServiceResponse<QuizzQuestionsReg> resp = new ServiceResponse<QuizzQuestionsReg>();

            try
            {
                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Name == quizzReg.QuizzName.Name);
                var userCheck = await _context.Users.SingleOrDefaultAsync(a => a.Username == quizzReg.QuizzName.Creator);

                if (quizzCheck == null && userCheck != null)
                {
                    var creator = (from a in _context.Users
                                   where a.Username == quizzReg.QuizzName.Creator
                                   select a.Id).FirstOrDefault();

                    Quiz newQuizz = new Quiz();

                    newQuizz.Name = quizzReg.QuizzName.Name;
                    newQuizz.CreatorId = creator;
                    newQuizz.CreationDate= DateTime.Now;

                    _context.Quizzes.Add(newQuizz);
                    await _context.SaveChangesAsync();

                    foreach (var questionReg in quizzReg.Questions)
                    {
                        var question = new Question
                        {
                            QuizId = newQuizz.Id,
                            Statement = questionReg.Statement,
                            Option1 = questionReg.Option1,
                            Option2 = questionReg.Option2,
                            Option3 = questionReg.Option3,
                            Option4 = questionReg.Option4,
                            CorrectAnswer = questionReg.CorrectAnswer,                          
                        };

                        _context.Questions.Add(question);
                    }

                    await _context.SaveChangesAsync();

                    resp.Data = quizzReg;
                    resp.Status = 1;

                    _logger.LogWarning("New_Quizz_Registration, " + "Author:" + creator + "," + "QuizzName:" + newQuizz.Name + "," + "CreationDate:" + newQuizz.CreationDate);
                }
                else
                {
                    QuizzQuestionsReg empty = new QuizzQuestionsReg();

                    resp.Data = empty;
                    resp.Status = 2;
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.QuizzRegistration: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<QuizzQuestions>> GetQuizzQuestion(int Id)
        {
            ServiceResponse<QuizzQuestions> resp = new ServiceResponse<QuizzQuestions>();

            try
            {
                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Id == Id);

                if (quizzCheck != null) 
                {
                    var quizzQuestions = (from a in _context.Quizzes
                                          join b in _context.Users on a.CreatorId equals b.Id
                                          join c in _context.Questions on a.Id equals c.QuizId into questions
                                          where a.Id == Id
                                          select new QuizzQuestions
                                          {
                                              Quizz = new Quizzes
                                              {
                                                  Id = a.Id,
                                                  Name = a.Name,
                                                  Creator = b.Username,
                                                  CreationDate = b.CreationDate.HasValue ? b.CreationDate.Value.ToString("dd/MM/yyyy HH:mm") : "Sin Fecha"
                                              },
                                              Question = questions.Select(q => new Questions
                                              {
                                                  Id = q.Id,
                                                  QuizzID = q.QuizId,
                                                  Statement = q.Statement,
                                                  Option1 = q.Option1,
                                                  Option2 = q.Option2,
                                                  Option3 = q.Option3,
                                                  Option4 = q.Option4
                                              }).ToList()
                                          }).FirstOrDefault();

                    resp.Data = quizzQuestions;
                    resp.Status = 1;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.GetQuizzQuestion: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<QuizzQuestionsUpd>> GetQuizzQuestionUpd(int Id)
        {
            ServiceResponse<QuizzQuestionsUpd> resp = new ServiceResponse<QuizzQuestionsUpd>();

            try
            {
                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Id == Id);

                if (quizzCheck != null)
                {
                    var quizzQuestions = (from a in _context.Quizzes
                                          join b in _context.Users on a.CreatorId equals b.Id
                                          join c in _context.Questions on a.Id equals c.QuizId into questions
                                          where a.Id == Id
                                          select new QuizzQuestionsUpd
                                          {
                                              Quizz = new Quizzes
                                              {
                                                  Id = a.Id,
                                                  Name = a.Name,
                                                  Creator = b.Username
                                              },
                                              Question = questions.Select(q => new QuestionsReg
                                              {
                                                  QuestionId = q.Id,
                                                  QuizzID = q.QuizId,
                                                  Statement = q.Statement,
                                                  Option1 = q.Option1,
                                                  Option2 = q.Option2,
                                                  Option3 = q.Option3,
                                                  Option4 = q.Option4,
                                                  CorrectAnswer = q.CorrectAnswer
                                              }).ToList()
                                          }).FirstOrDefault();

                    resp.Data = quizzQuestions;
                    resp.Status = 1;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                }

                return resp;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.GetQuizQuestionsUpd: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<QuizzResult>> SendAnswer(QuizzResponse quizzResponse)
        {
            ServiceResponse<QuizzResult> resp = new ServiceResponse<QuizzResult>();

            foreach (Questions question in quizzResponse.Questions)
            {
                string answerString = (question.Answer == "1" || question.Answer == "2" || question.Answer == "3" || question.Answer == "4") ? question.Answer : "0";
                question.Answer = answerString;
            }

            try
            { 
                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Id == quizzResponse.QuizzId);
                var userCheck = (from a in _context.Users
                                 where a.Username == quizzResponse.UserName
                                 select new { a.Id, a.Username }).FirstOrDefault();

                if (quizzCheck != null && userCheck != null)
                {
                    var questions = (from a in _context.Questions
                                     where a.QuizId == quizzResponse.QuizzId
                                     select a.CorrectAnswer).ToList();

                    var responses = (from q in quizzResponse.Questions
                                     join a in _context.Questions
                                     on q.Id equals a.Id
                                     where a.QuizId == quizzResponse.QuizzId
                                     select new DataDB.Response
                                     {
                                         QuizId = quizzResponse.QuizzId,
                                         UserId = userCheck.Id,
                                         QuestionId = q.Id,
                                         SelectedAnswer = int.Parse(q.Answer),
                                         CorrectAnswer = a.CorrectAnswer
                                     }).ToList();

                    foreach (var response in responses)
                    {
                        _context.Responses.Add(response);
                    }

                    var numCorrectAnswers = responses.Count(r => r.SelectedAnswer == r.CorrectAnswer);
                    var totalQuestions = responses.Count();
                    var score = (int)Math.Round((double)numCorrectAnswers / totalQuestions * 100);

                    var result = new Result
                    {
                        QuizId = quizzResponse.QuizzId,
                        UserId = userCheck.Id,
                        Corrects = numCorrectAnswers,
                        Score = score,
                        DateCompleted = DateTime.Now
                    };

                    _context.Results.Add(result);

                    await _context.SaveChangesAsync();

                    QuizzResult quizzResult = new QuizzResult();
                    quizzResult.Id = result.Id;

                    resp.Data = quizzResult;
                    resp.Status = 1;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                }
              
                return resp;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.SendAnswer: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<QuizzResultUser>> GetSingleResult(int Id)
        {
            ServiceResponse<QuizzResultUser> resp = new ServiceResponse<QuizzResultUser>();

            try
            {
                var resultCheck = await _context.Results.SingleOrDefaultAsync(a => a.Id == Id);

                if (resultCheck != null)
                {
                    var result = (from r in _context.Results
                                  where r.Id == Id
                                  select new
                                  {
                                      QuizId = r.QuizId,
                                      UserId = r.UserId,
                                      Score = r.Score,
                                      Corrects = r.Corrects,  
                                      TotalQuestions = _context.Questions.Count(resp => resp.QuizId == r.QuizId),
                                  } into g
                                  join q in _context.Quizzes on g.QuizId equals q.Id
                                  join u in _context.Users on g.UserId equals u.Id
                                  select new QuizzResultUser
                                  {
                                      QuizzName = q.Name,
                                      UserName = u.Username,
                                      TotalQuestions = g.TotalQuestions,
                                      Corrects = g.Corrects,
                                      Score = g.Score
                                  }).FirstOrDefault();

                    resp.Data = result;
                    resp.Status = 1;

                    return resp;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                    
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.GetSingleResult: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<List<AllResults>>> GetAllResult(int Id)
        {
            ServiceResponse<List<AllResults>> resp = new ServiceResponse<List<AllResults>>();

            try
            {
                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Id == Id);

                if (quizzCheck != null)
                {                  
                    var allResults = (from r in _context.Results
                                     join q in _context.Quizzes on r.QuizId equals q.Id
                                     join u in _context.Users on r.UserId equals u.Id
                                     where r.QuizId == Id
                                     group r by new { q.Name, u.Username } into g
                                     select new AllResults()
                                     {
                                         QuizzName = g.Key.Name,
                                         Username = g.Key.Username,
                                         Attempts = g.Count(),
                                         TopScore = g.Max(r => r.Score)
                                     }).ToList();                  

                    resp.Data = allResults;
                    resp.Status = 1;
                    return resp;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.GetAllResult: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<List<QuizzResultUser>>> GetSingleListResult(int Id, string user)
        {
            ServiceResponse<List<QuizzResultUser>> resp = new ServiceResponse<List<QuizzResultUser>>();

            try
            {
                var userCheck = (from a in _context.Users
                                 where a.Username == user
                                 select new { a.Id, a.Username }).FirstOrDefault();

                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Id == Id);

                if (userCheck != null && quizzCheck != null)
                {
                    var result = (from r in _context.Results
                                  where r.QuizId == Id && r.UserId == userCheck.Id
                                  select new
                                  {
                                      QuizId = r.QuizId,
                                      UserId = r.UserId,
                                      Score = r.Score,
                                      Corrects = r.Corrects,
                                      TotalQuestions = _context.Questions.Count(resp => resp.QuizId == r.QuizId),
                                  } into g
                                  join q in _context.Quizzes on g.QuizId equals q.Id
                                  join u in _context.Users on g.UserId equals u.Id
                                  select new QuizzResultUser
                                  {
                                      QuizzName = q.Name,
                                      UserName = u.Username,
                                      TotalQuestions = g.TotalQuestions,
                                      Corrects = g.Corrects,
                                      Score = g.Score
                                  }).ToList();

                    resp.Data = result;
                    resp.Status = 1;

                    return resp;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.GetSingleListResult: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<QuestionsReg>> UpdateQuestion(QuestionUpdate questionUpdate)
        {
            ServiceResponse<QuestionsReg> resp = new ServiceResponse<QuestionsReg>();

            try
            {
                var quizzCheck = await _context.Quizzes.SingleOrDefaultAsync(a => a.Id == questionUpdate.QuizzId);
                var question = await _context.Questions.FindAsync(questionUpdate.UpdateQuestion.QuestionId);

                if (quizzCheck != null && question != null)
                {
                    question.Statement = questionUpdate.UpdateQuestion.Statement;
                    question.Option1 = questionUpdate.UpdateQuestion.Option1;
                    question.Option2 = questionUpdate.UpdateQuestion.Option2;
                    question.Option3 = questionUpdate.UpdateQuestion.Option3;
                    question.Option4 = questionUpdate.UpdateQuestion.Option4;

                    await _context.SaveChangesAsync();

                    resp.Status = 1;
                    _logger.LogWarning("Update_Quizz= " + "DateTime: " + DateTime.Now + "OldQuestions: " +question + "," + "NewQuestions" + questionUpdate);

                    return resp;
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 2;
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogError("QuizzService.UpdateQuestion: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<List<Quizzes>>> GetAuthorQuizzList(UserToken token)
        {
            ServiceResponse<List<Quizzes>> resp = new ServiceResponse<List<Quizzes>>();
            List<Quizzes> empty = new List<Quizzes>();

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token.chainValue.Replace("\"", ""));
                string user = jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;

                var userCheck = await _context.Users.SingleOrDefaultAsync(a => a.Username == user);

                if (userCheck != null)
                {
                    List<Quizzes> quizzes = await (from a in _context.Quizzes
                                                   join b in _context.Users
                                                   on a.CreatorId equals b.Id
                                                   where a.CreatorId == userCheck.Id
                                                   select new Quizzes()
                                                   {
                                                       Id = a.Id,
                                                       Name = a.Name,
                                                       Creator = b.Username
                                                   }).ToListAsync();

                    if (quizzes.Any())
                    {
                        resp.Data = quizzes;
                        resp.Status = 1;
                    }
                    else
                    {
                        resp.Data = null;
                        resp.Status = 2;
                    }
                }
                else
                {
                    resp.Data = null;
                    resp.Status = 3;
                }               

                return resp;
            }
            catch (Exception ex)
            {
                resp.Status = 0;
                _logger.LogError("QuizzService.GetAuthorQuizzList: " + ex.Message, ex);
                return resp;
            }
        }
    }
}
