using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
