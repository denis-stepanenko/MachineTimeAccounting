namespace MachineTimeAccounting.Models
{
    public class ProductMachineProgram
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal Time { get; set; }
        public string Description { get; set; }

        public int ProductMachineId { get; set; }
        public virtual ProductMachine ProductMachine { get; set; }
    }
}
