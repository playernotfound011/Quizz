using QuizzAPP.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.CustomValidations
{
    public class QuestionsListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var list = value as List<QuestionsReg>;
                if (list != null)
                {
                    if (list.Count == 0)
                    {
                        return new ValidationResult("La lista de preguntas no puede estar vacía.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

}
