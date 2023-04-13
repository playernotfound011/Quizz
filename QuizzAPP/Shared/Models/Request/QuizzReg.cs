using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Request
{
    public class QuizzReg
    {
        [Required(ErrorMessage = "Escribe un nombre para el cuestionario.")]
        public string Name { get; set; }

        public string Creator { get; set; } = string.Empty;
    }
}
