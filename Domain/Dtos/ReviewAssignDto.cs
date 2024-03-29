using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ReviewAssignDto
    {
        public long ContentId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsPositive { get; set; }
        public int Score { get; set; }
    }
}
