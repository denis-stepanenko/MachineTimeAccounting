using MachineTimeAccounting.Models;
using System.Data.Entity;
using System.Linq;

namespace MachineTimeAccounting
{
    internal class EFContext : DbContext
    {
        public EFContext() : base("Data source = ; Initial catalog = ;")
        {
            Database.SetInitializer<EFContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //modelBuilder.Entity<ProductMachineProgram>()
            //    .HasOptional<ProductMachine>(x => x.ProductMachine)
            //    .WithMany(x => x.Programs)
            //    .WillCascadeOnDelete(true);

            //builder.Entity<ProductMachineProgram>()
            //    .HasRequired(x => x.ProductMachine)
            //    .WithMany(x => x.Programs)
            //    .WillCascadeOnDelete();

            //builder.Entity<ProductMachine>()
            //    .HasMany(x => x.Programs)
            //    .WithRequired(x => x.ProductMachine);

            //builder.Entity<ProductMachineProgram>()
            //    .HasRequired<ProductMachine>(x => x.ProductMachine)
            //    .WithMany(x => x.Programs)
            //    .HasForeignKey<int>(x => x.ProductMachineId);

            //base.OnModelCreating(builder);
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<ProductMachine> ProductMachines { get; set; }
        public DbSet<ProductMachineProgram> ProductMachinePrograms { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<RememberedUser> RememberedUsers { get; set; }

        public bool CheckPassword(int id, string password) => Users.Any(x => x.Id == id && x.Password == password);
    }
}
