using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MachineTimeAccounting.Models
{
    public class ProductMachine
    {
        public ProductMachine()
        {
            Programs = new ObservableCollection<ProductMachineProgram>();
        }

        public int Id { get; set; }
        public string ProductCode { get; set; }

        public int Department { get; set; }

        public int MachineId { get; set; }
        public virtual Machine Machine { get; set; }

        public virtual ObservableCollection<ProductMachineProgram> Programs { get; set; }
    }
}
