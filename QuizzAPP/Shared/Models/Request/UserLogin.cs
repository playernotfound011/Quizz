using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Request
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Ingresa un nombre de usuario.")]
        [MaxLength(50, ErrorMessage = "Excede el máximo de caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario sólo puede contener letras, números, y (.), (_), (-)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ingresa una contraseña.")]
        [MaxLength(100, ErrorMessage = "Excede el máximo de caracteres.")]
        public string Password { get; set; }
    }
}
