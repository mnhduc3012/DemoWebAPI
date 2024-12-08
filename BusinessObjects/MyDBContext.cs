using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyStoreDB"));
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual  DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName="Beverages"},
                new Category { CategoryId = 2, CategoryName="Seafood"},
                new Category { CategoryId = 3, CategoryName="Dairy Products"},
                new Category { CategoryId = 4, CategoryName="Condiments"},
                new Category { CategoryId = 5, CategoryName="Produce"},
                new Category { CategoryId = 6, CategoryName="Meat/Poultry"}
                );

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)  
                .WithMany()               
                .HasForeignKey(p => p.CategoryId)  
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
