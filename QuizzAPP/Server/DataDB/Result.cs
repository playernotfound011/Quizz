using System;
using System.Collections.Generic;

namespace QuizzAPP.Server.DataDB;

public partial class Result
{
    public int Id { get; set; }

    public int QuizId { get; set; }

    public int UserId { get; set; }

    public int Corrects { get; set; }

    public int Score { get; set; }

    public DateTime? DateCompleted { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
