using WebApp.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApp.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserToContact>()
                .HasKey(x => x.Id);
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.UserToContactUsers)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.UserToContactContacts)
                .WithOne(x => x.Contact)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.ScreenName)
                .IsUnique();
        }

        /// <summary>
        /// Таблица, устанавливающая связь между пользователями и их контактами
        /// </summary>
        public DbSet<UserToContact> UsersToContacts { get; set; }
    }
}
