using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class QuizzResponses
    {
        public int Id { get; set; }
        public int QuizzId { get; set; }
        public string Question { get; set; } = string.Empty;
        public int SelectedAnswer { get; set; }
        public int CorrectAnswer { get; set; }
    }
}
