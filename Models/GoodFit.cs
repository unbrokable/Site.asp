using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ExampleB.Models
{
    public partial class GoodFit : DbContext
    {
        public GoodFit()
            : base("name=GoodFit")
        {
        }

        public virtual DbSet<Diet> Diet { get; set; }
        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<History_user_condition> History_user_condition { get; set; }
        public virtual DbSet<History_user_diet> History_user_diet { get; set; }
        public virtual DbSet<UserDiet> UserDiet { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diet>()
                .HasMany(e => e.History_user_diet)
                .WithRequired(e => e.Diet)
                .HasForeignKey(e => e.Diet_Id);

            modelBuilder.Entity<Diet>()
                .HasMany(e => e.UserDiet)
                .WithOptional(e => e.Diet)
                .HasForeignKey(e => e.Diet_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Diet>()
                .HasMany(e => e.Dish)
                .WithMany(e => e.Diet)
                .Map(m => m.ToTable("Composition_Dish"));

            modelBuilder.Entity<Users>()
                .HasMany(e => e.History_user_condition)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.User_Id);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.UserDiet)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete();
        }
    }
}
