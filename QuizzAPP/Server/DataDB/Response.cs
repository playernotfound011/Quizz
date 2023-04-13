using System;
using System.Collections.Generic;

namespace QuizzAPP.Server.DataDB;

public partial class Response
{
    public int Id { get; set; }

    public int QuizId { get; set; }

    public int QuestionId { get; set; }

    public int UserId { get; set; }

    public int SelectedAnswer { get; set; }

    public int CorrectAnswer { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
