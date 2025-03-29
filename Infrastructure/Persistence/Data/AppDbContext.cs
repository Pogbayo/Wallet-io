using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Domain.Entities;

namespace SpagWallet.Infrastructure.Persistence.Data
{
   public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Kyc> Kycs { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.BankAccount)
                .WithOne(b => b.User)
                .HasForeignKey<BankAccount>(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.BankAccount)
                .WithOne(b => b.Wallet)
                .HasForeignKey<Wallet>(w => w.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Wallet)
                .WithOne(w => w.Card)
                .HasForeignKey<Card>(c => c.WalletId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.BankAccount)
                .WithOne(b => b.Card)
                .HasForeignKey<Card>(c => c.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BankAccount>()
                .Property(b => b.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

    }
}

