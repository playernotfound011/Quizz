using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class LoginInfo
    {
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
