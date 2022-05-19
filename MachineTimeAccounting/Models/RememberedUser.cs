using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineTimeAccounting.Models
{
    public class RememberedUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string WindowsUserName { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
