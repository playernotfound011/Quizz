using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class AllResults
    {
        public string QuizzName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Attempts { get; set; }
        public int TopScore { get; set; }
    }
}
