using Bank.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bank.Server.Persistence;

public class BankMasterDbContext(DbContextOptions<BankMasterDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
