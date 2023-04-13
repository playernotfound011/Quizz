using QuizzAPP.Shared.Models.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Request
{
    public class QuizzQuestionsReg
    {
        [Required]
        public QuizzReg QuizzName { get; set; }

        [QuestionsList(ErrorMessage = "La lista de preguntas no puede estar vacía.")]
        public List<QuestionsReg> Questions { get; set; }
    }
}
