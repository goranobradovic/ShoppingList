﻿using System.Collections.Generic;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new Configurations.ShoppingListConfiguration());
            modelBuilder.Configurations.Add(new Configurations.ShoppingListItemConfiguration());
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

                    var shoppingList = new ShoppingList()
                    {
                        Name = "Sample list",
                        SecretUrl = "SampleList",
                        Active = true,
                        Items = new List<Item>()
                        {
                            new Item()
                            {
                             Name="Eggs",
                             Amount = 18,
                             Unit = new Unit(){ Name="piece"}
                            },
                           new Item()
                            {
                             Name="Milk",
                             Amount = 8,
                             Unit = new Unit(){ Name="liter"}
                            },
                            new Item()
                            {
                                Name="Cheese",
                                Amount=0.3M,
                                Unit = new Unit(){ Name="kg"}
                            }
                        }
                    };

                    context.Set<ShoppingList>().Add(shoppingList);
                    context.SaveChanges();
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
                        .WithRequired(i => i.ParentList);
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
                    this.HasRequired(t => t.Unit)
                        .WithMany();
                }
            }

            public class UnitConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Unit>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="T:UnitConfiguration"/> class.
                /// </summary>
                public UnitConfiguration()
                {
                    this.HasKey(t => t.Id);
                }
            }
        }
    }
}