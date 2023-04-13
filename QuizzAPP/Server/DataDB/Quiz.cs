using System;
using System.Collections.Generic;

namespace QuizzAPP.Server.DataDB;

public partial class Quiz
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CreatorId { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; } = new List<Question>();

    public virtual ICollection<Response> Responses { get; } = new List<Response>();

    public virtual ICollection<Result> Results { get; } = new List<Result>();
}
