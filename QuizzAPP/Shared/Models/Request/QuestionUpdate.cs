using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Request
{
    public class QuestionUpdate
    {
        public int QuizzId { get; set; }
        public QuestionsReg UpdateQuestion { get; set; }
    }
}
