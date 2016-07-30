using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrEvalData
{
    public class TrEvalContext : DbContext
    {
        public DbSet<SessionEval> Evaluations { get; set; }
    }

    public class SessionEval
    {
        public int SessionEvalId { get; set; }
        public string SessionName { get; set; }
        public decimal SessionScore { get; set; }
        public DateTime ScoreDate { get; set; }

    }
}
