﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzAPP.Shared.Models.Response
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public int Status { get; set; }
    }
}
