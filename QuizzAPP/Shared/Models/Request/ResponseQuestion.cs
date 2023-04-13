using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Request
{
    public class ResponseQuestion
    {
        public int QuestionId { get; set; }

        [Required]
        public int SelectedAnswer { get; set; }
    }
}
