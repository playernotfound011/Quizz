using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class LoginResponse
    {
        public string token { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
