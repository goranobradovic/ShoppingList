using System.Collections.Generic;
using System.Data.Entity;

namespace ShoppingList.Web.Models
{
    public class ŞhoppingListDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ŞhoppingListDbContext"/> class.
        /// </summary>
        static ŞhoppingListDbContext()
        {
            Database.SetInitializer<ŞhoppingListDbContext>(new MigrateDatabaseToLatestVersion<ŞhoppingListDbContext, Configurations.ShoppingListMigrator>());
        }

        public ŞhoppingListDbContext() :
            base("name=DefaultConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new Configurations.ShoppingListConfiguration());
            modelBuilder.Configurations.Add(new Configurations.ShoppingListItemConfiguration());
            //modelBuilder.Configurations.Add(new Configurations.UnitConfiguration());
        }

        public DbSet<ShoppingList> ShoppingList { get; set; }

        public class Configurations
        {
            public class ShoppingListMigrator : System.Data.Entity.Migrations.DbMigrationsConfiguration<ŞhoppingListDbContext>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="T:ShoppingListMigrator"/> class.
                /// </summary>
                public ShoppingListMigrator()
                {
                    this.AutomaticMigrationsEnabled = true;
                    this.AutomaticMigrationDataLossAllowed = true;
                }

                protected override void Seed(ŞhoppingListDbContext context)
                {
                    base.Seed(context);
                }
            }

            public class ShoppingListConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ShoppingList>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="T:ShoppingListConfiguration"/> class.
                /// </summary>
                public ShoppingListConfiguration()
                {
                    this.HasKey(p => p.Id);
                    this.HasMany(p => p.Items)
                        .WithRequired(i => i.ShoppingList)
                        .HasForeignKey(i => i.ShoppingList_Id);
                }
            }

            public class ShoppingListItemConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Item>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="T:ShoppingListItemConfiguration"/> class.
                /// </summary>
                public ShoppingListItemConfiguration()
                {
                    this.HasKey(t => t.Id);
                }
            }

            //public class UnitConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Unit>
            //{
            //    /// <summary>
            //    /// Initializes a new instance of the <see cref="T:UnitConfiguration"/> class.
            //    /// </summary>
            //    public UnitConfiguration()
            //    {
            //        this.HasKey(t => t.Id);
            //    }
            //}
        }
    }
}