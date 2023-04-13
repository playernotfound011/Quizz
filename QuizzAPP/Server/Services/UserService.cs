using Microsoft.EntityFrameworkCore;
using QuizzAPP.Server.DataDB;
using QuizzAPP.Shared.Models.Request;
using QuizzAPP.Shared.Models.Response;
using System.Collections.Generic;
using QuizzAPP.Server.Utility;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace QuizzAPP.Server.Services
{
    public class UserService : IUsersService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        public readonly QuizzBdContext _context;
        public readonly List<UserQuizz> empty = new List<UserQuizz>();

        public UserService(QuizzBdContext context, IConfiguration configuration, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ApiResponse<List<UserQuizz>>> GetUsers()
        {
            ApiResponse<List<UserQuizz>> resp = new ApiResponse<List<UserQuizz>>();
            List <UserQuizz> empty = new List<UserQuizz>();

            try
            {
                List<User> users = await _context.Users.ToListAsync();

                List<UserQuizz> userQ = users.Select(u => new UserQuizz 
                { 
                    Id = u.Id, 
                    UserName = u.Username, 
                    UserType = u.UserType,
                    CreationDate = u.CreationDate.HasValue ? u.CreationDate.Value.ToString("dd/MM/yyyy HH:mm") : "Sin Fecha"                   
                }).ToList();

                if (userQ.Count > 0)
                {
                    resp.Data = userQ;
                    resp.Message = "Users retrieved Successfully";
                }
                else
                {
                    resp.Data = userQ;
                    resp.Message = "no registered users";
                }
            }
            catch (Exception ex)
            {
                resp.Data = empty;
                resp.Message = "Server error";
                _logger.LogError("UserService.GetUsers: " + ex.Message, ex);
            }

            return resp;
        }

        public async Task<ServiceResponse<LoginInfo>> UserLogin(UserLogin user)
        {
            ServiceResponse<LoginInfo> resp = new ServiceResponse<LoginInfo>
            {
                Data = null
            };

            try
            {
                var userInfo = await (from u in _context.Users
                                      where u.Username == user.UserName
                                      select new
                                      {
                                          u.Id,
                                          u.Username,
                                          u.Password,
                                          u.UserType
                                      }).FirstOrDefaultAsync();

                if (userInfo == null)
                {
                    resp.Status = 2;
                    return resp;
                }

                bool validPassword = PasswordHasher.VerifyPassword(user.Password, userInfo.Password);

                if (!validPassword)
                {
                    resp.Status = 2;
                    return resp;
                }

                var userToken = CreateToken(new User
                {
                    Username = user.UserName,
                    UserType = userInfo.UserType
                });

                if (userToken == null)
                {
                    resp.Status = 0;
                    return resp;
                }

                resp.Data = userToken;
                resp.Status = 1;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Status = 0;
                _logger.LogError("UserService.UserLogin: " + ex.Message, ex);
                return resp;
            }
        }

        public async Task<ServiceResponse<UserQuizz>> UserRegistration(UserRegistration user)
        {
            ServiceResponse<UserQuizz> resp = new ServiceResponse<UserQuizz>();
            UserQuizz empty = new UserQuizz();

            try
            {
                var userCheck = await _context.Users.SingleOrDefaultAsync(a => a.Username == user.UserName);

                if (userCheck == null)
                {
                    User newUser = new User();

                    string passwordHash = PasswordHasher.HashPassword(user.Password);

                    newUser.Username = user.UserName;
                    newUser.UserType = "USR";
                    newUser.Password = passwordHash;
                    newUser.CreationDate = DateTime.Now;

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    resp.Data = empty;
                    resp.Status = 1;
                }
                else
                {
                    resp.Data = empty;
                    resp.Status = 2;
                }

                _logger.LogInformation("New_User_Registration:" + user.UserName + "," + "CreationDate:" + DateTime.Now.ToString());
                return resp;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Status = 0;
                _logger.LogWarning("UserService.UserRegistration: " + ex.Message, ex);
                return resp;
            }
        }

        private LoginInfo CreateToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserType)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSetting:Token").Value!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                LoginInfo loginInfo = new LoginInfo
                {
                    Role = user.UserType,
                    Token = jwt
                };

                return loginInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserService.CreateToken: " + ex.Message, ex);
                return null;
            }          
        }

        public async Task<ServiceResponse<UserType>> GetUserType(UserToken token)
        {
            ServiceResponse<UserType> resp = new ServiceResponse<UserType>();

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token.chainValue.Replace("\"", ""));
                string role = jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value;
                string user = jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;

                var userCheck = await _context.Users.SingleOrDefaultAsync(a => a.Username == user);

                if (userCheck != null && userCheck.UserType == role)
                {
                    UserType userType = new UserType()
                    {
                        Type = userCheck.UserType,
                        UserName = userCheck.Username,
                    };

                    resp.Data = userType;
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
                _logger.LogError("UserService.GetUserType: " + ex.Message, ex);
                return resp;
            }
        }
    }
}
