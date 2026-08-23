using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RUYA_API.Domain.Common;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Context
{
    public class RuyaContext : IdentityDbContext<User>
    {
        public RuyaContext(DbContextOptions<RuyaContext> options) : base(options)
        {
        }

        public DbSet<Site> Sites => Set<Site>();
        public DbSet<Artifact> Artifacts => Set<Artifact>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Tour> Tours => Set<Tour>();
        public DbSet<Source> Sources => Set<Source>();
        public DbSet<TourStop> TourStops => Set<TourStop>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<MemoryAlbum> MemoryAlbums => Set<MemoryAlbum>();
        public DbSet<AlbumItem> AlbumItems => Set<AlbumItem>();
        public DbSet<ConversationAttachment> ConversationAttachments => Set<ConversationAttachment>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RuyaContext).Assembly);
        }

        public override int SaveChanges()
        {
            StampAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            StampAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void StampAuditFields()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }
    }
}
