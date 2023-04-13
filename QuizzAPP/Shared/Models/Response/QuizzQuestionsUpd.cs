using QuizzAPP.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class QuizzQuestionsUpd
    {
        public Quizzes Quizz { get; set; }
        public List<QuestionsReg> Question { get; set; }
    }
}
