using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace ShoppingList.Web.Models
{
    public class ShoppingListDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ShoppingListDbContext"/> class.
        /// </summary>
        static ShoppingListDbContext()
        {
            Database.SetInitializer<ShoppingListDbContext>(new MigrateDatabaseToLatestVersion<ShoppingListDbContext, Configurations.ShoppingListMigrator>());
        }

        public ShoppingListDbContext() :
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

        public override int SaveChanges()
        {
            foreach (var shoppingList in this.ChangeTracker.Entries<ShoppingList>())
            {
                if (shoppingList.State == EntityState.Added)
                {
                    shoppingList.Entity.Owner =
                        this.UserProfiles.FirstOrDefault(
                            up => up.UserName.ToLower() == Thread.CurrentPrincipal.Identity.Name.ToLower());
                }
            }
            
            return base.SaveChanges();
        }

        public DbSet<ShoppingList> ShoppingList { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public class Configurations
        {
            public class ShoppingListMigrator : System.Data.Entity.Migrations.DbMigrationsConfiguration<ShoppingListDbContext>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="T:ShoppingListMigrator"/> class.
                /// </summary>
                public ShoppingListMigrator()
                {
                    this.AutomaticMigrationsEnabled = true;
                    this.AutomaticMigrationDataLossAllowed = true;
                }

                protected override void Seed(ShoppingListDbContext context)
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