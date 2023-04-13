using System;
using System.Collections.Generic;

namespace QuizzAPP.Server.DataDB;

public partial class Question
{
    public int Id { get; set; }

    public int QuizId { get; set; }

    public string Statement { get; set; } = null!;

    public string Option1 { get; set; } = null!;

    public string Option2 { get; set; } = null!;

    public string Option3 { get; set; } = null!;

    public string Option4 { get; set; } = null!;

    public int CorrectAnswer { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual ICollection<Response> Responses { get; } = new List<Response>();
}
