using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class QuizzResultUser
    {
        public string QuizzName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int Corrects { get; set; }
        public int TotalQuestions { get; set; }
        public int Score { get; set; }
    }
}
