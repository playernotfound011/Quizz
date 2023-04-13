using System;
using System.Collections.Generic;

namespace QuizzAPP.Server.DataDB;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; } = new List<Quiz>();

    public virtual ICollection<Response> Responses { get; } = new List<Response>();

    public virtual ICollection<Result> Results { get; } = new List<Result>();
}
