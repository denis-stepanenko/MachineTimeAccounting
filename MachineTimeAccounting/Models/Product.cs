using System.ComponentModel.DataAnnotations.Schema;

namespace MachineTimeAccounting.Models
{
    [Table("ref_dse")]
    internal class Product
    {
        public int Id { get; set; }
        [Column("Decnum")]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
