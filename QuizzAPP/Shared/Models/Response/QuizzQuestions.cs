using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class QuizzQuestions
    {
        public Quizzes Quizz { get; set; }
        public List<Questions> Question { get; set; }
    }
}
