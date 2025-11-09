using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Domain.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string OperationType { get; set; }
        public int RecordId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
    }
}
