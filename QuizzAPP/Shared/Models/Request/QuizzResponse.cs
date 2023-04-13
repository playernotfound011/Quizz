using QuizzAPP.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Request
{
    public class QuizzResponse
    {
        [Required]
        public int QuizzId { get; set; }

        public string UserName { get; set; }

        [Required]
        public List<Questions> Questions { get; set; }
    }
}
