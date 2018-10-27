﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class ProviderRelationshipsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<User> Users { get; set; }
        
        public ProviderRelationshipsDbContext(DbContextOptions<ProviderRelationshipsDbContext> options) : base(options)
        {
        }

        protected ProviderRelationshipsDbContext()
        {
        }

        public virtual Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountLegalEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AccountLegalEntityProviderConfiguration());
            modelBuilder.ApplyConfiguration(new HealthCheckConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}