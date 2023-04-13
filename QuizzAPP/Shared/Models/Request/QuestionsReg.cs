using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizzAPP.Shared.Models.Request
{
    public class QuestionsReg
    {
        public int QuestionId { get; set; }

        [Required]
        public int QuizzID { get; set; }

        [Required(ErrorMessage = "Ingresa el enunciado de la pregunta.")]
        public string Statement { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa una opcion.")]
        public string Option1 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa una opcion.")]
        public string Option2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa una opcion.")]
        public string Option3 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa una opcion.")]
        public string Option4 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa el numero de la alternativa correcta.")]
        [Range(1, 4, ErrorMessage = "Selecciona la numero de alternativa del 1 al 4.")]
        public int CorrectAnswer { get; set; }
        public string CorrectOption => $"Option{CorrectAnswer}";
    }
}
