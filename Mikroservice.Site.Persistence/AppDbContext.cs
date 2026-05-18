using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Persistence;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Microservice.Site.Persistence
{
    public class AppDbContext : DbContext
    {

        // Cache for entity-type to SiteId property info (performance optimization)
        private static readonly ConcurrentDictionary<Type, PropertyInfo?> _siteIdPropertyCache = new();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BandLogo> BandLogos { get; set; }
        public DbSet<Banner> Bannerler { get; set; }
        public DbSet<Bilgi> Bilgiler { get; set; }
        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Dil> Diller { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Haber> Haberler { get; set; }
        public DbSet<Hedef> Hedefler { get; set; }
        public DbSet<Menu> Menuler { get; set; }
        public DbSet<PersonelTelefon> PersonelTelefonlar { get; set; }
        public DbSet<PersonelTip> PersonelTipleri { get; set; }
        public DbSet<Popup> Popuplar { get; set; }
        public DbSet<SikcaSorulanSoru> SikcaSorulanSorular { get; set; }
        public DbSet<Mikroservice.Site.Domain.Entities.Site> Siteler { get; set; }
        public DbSet<SiteOzellikleri> SiteOzellikleri { get; set; }
        public DbSet<SitePersonel> SitePersonelleri { get; set; }
        public DbSet<Template> Templateler { get; set; }
        public DbSet<Unvan> Unvanlar { get; set; }
        public DbSet<Video> Videolar { get; set; }
        public DbSet<YoneticiSite> YoneticiSiteler { get; set; }
        public DbSet<YonetimDuyuru> YonetimDuyurular { get; set; }
        public DbSet<YonetimDuyuruOkundu> YonetimDuyuruOkunduBilgileri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
            base.OnModelCreating(modelBuilder);
        }



        // JSON serialization options: ignore navigation properties to avoid circular refs & keep payload lean
        private static readonly JsonSerializerOptions _auditJsonOptions = new() {
            WriteIndented = false,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };


        // ──────────────────────────────────────────────
        //  Audit Log Engine (async)
        // ──────────────────────────────────────────────
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = CollectAuditEntries();
            var result = await base.SaveChangesAsync(cancellationToken);

            // After successful save, persist the audit logs
            if (auditEntries.Count > 0)
            {
                AuditLogs.AddRange(auditEntries);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }


        // ──────────────────────────────────────────────
        //  Core: Collect audit entries from tracked changes
        // ──────────────────────────────────────────────
        private List<AuditLog> CollectAuditEntries()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            if (entries.Count == 0)
                return [];

            var utcNow = DateTime.Now;
            var userId = AuditLogContext.UserId;
            var username = AuditLogContext.Username;
            var traceId = AuditLogContext.TraceId;
            var ipAddress = AuditLogContext.IpAddress;
            var auditLogs = new List<AuditLog>(entries.Count);

            foreach (var entry in entries)
            {
                var entityType = entry.Entity.GetType();

                // ⛔ Self-logging prevention: never audit AuditLog changes
                if (entityType == typeof(AuditLog))
                    continue;

                var action = entry.State.ToString();
                var entityName = entityType.Name;
                var entityId = GetEntityId(entry);
                var siteId = GetSiteId(entry, entityType);

                // Serialize old/new values (scalar properties only)
                string? oldValues = null;
                string? newValues = null;
                string? description = null;

                if (entry.State == EntityState.Modified)
                {
                    oldValues = SerializeModifiedProperties(entry, isOld: true);
                    newValues = SerializeModifiedProperties(entry, isOld: false);
                    description = BuildDescription(entityName, action, entry);
                }
                else if (entry.State == EntityState.Added)
                {
                    newValues = SerializeAllProperties(entry);
                    description = BuildDescription(entityName, action, entry);
                }
                else // Deleted
                {
                    oldValues = SerializeAllProperties(entry);
                    description = BuildDescription(entityName, action, entry);
                }

                auditLogs.Add(new AuditLog
                {
                    UserId = userId,
                    Username = username ?? string.Empty,
                    TraceId = traceId,
                    Action = action,
                    EntityName = entityName,
                    EntityId = entityId,
                    SiteId = siteId,
                    Description = description,
                    OldValues = oldValues,
                    NewValues = newValues,
                    IpAddress = ipAddress ?? "0.0.0.0",
                    CreatedAt = utcNow
                });
            }

            return auditLogs;
        }

        // ──────────────────────────────────────────────
        //  Extract primary-key value as string
        // ──────────────────────────────────────────────
        private static string? GetEntityId(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null) return null;

            var keyValues = key.Properties
                .Select(p => entry.Property(p.Name).CurrentValue?.ToString())
                .Where(v => v != null);
            return string.Join("-", keyValues);
        }

        // ──────────────────────────────────────────────
        //  Extract SiteId via reflection (cached)
        // ──────────────────────────────────────────────
        private static int? GetSiteId(EntityEntry entry, Type entityType)
        {
            // Fast path: no need to check AuditLog itself
            if (entityType == typeof(AuditLog))
                return null;

            var propInfo = _siteIdPropertyCache.GetOrAdd(entityType, t =>
                t.GetProperty("SiteId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase));

            if (propInfo == null)
                return null;

            var value = propInfo.GetValue(entry.Entity);
            return value is int intVal ? intVal : null;
        }

        // ──────────────────────────────────────────────
        //  Serialize only modified scalar properties
        // ──────────────────────────────────────────────
        private static string? SerializeModifiedProperties(EntityEntry entry, bool isOld)
        {
            var modifiedProps = entry.Properties
                .Where(p => p.IsModified && !IsNavigationOrCollection(p.Metadata))
                .ToList();

            if (modifiedProps.Count == 0) return null;

            var dict = new Dictionary<string, object?>(modifiedProps.Count);
            foreach (var prop in modifiedProps)
            {
                dict[prop.Metadata.Name] = isOld ? prop.OriginalValue : prop.CurrentValue;
            }

            return JsonSerializer.Serialize(dict, _auditJsonOptions);
        }

        // ──────────────────────────────────────────────
        //  Serialize all scalar properties
        // ──────────────────────────────────────────────
        private static string? SerializeAllProperties(EntityEntry entry)
        {
            var scalarProps = entry.Properties
                .Where(p => !IsNavigationOrCollection(p.Metadata))
                .ToList();

            if (scalarProps.Count == 0) return null;

            var dict = new Dictionary<string, object?>(scalarProps.Count);
            foreach (var prop in scalarProps)
            {
                dict[prop.Metadata.Name] = prop.CurrentValue;
            }

            return JsonSerializer.Serialize(dict, _auditJsonOptions);
        }

        // ──────────────────────────────────────────────
        //  Check if property is a navigation / collection
        // ──────────────────────────────────────────────
        private static bool IsNavigationOrCollection(Microsoft.EntityFrameworkCore.Metadata.IReadOnlyProperty property)
        {
            return property.IsShadowProperty()
                || (property.PropertyInfo?.IsDefined(typeof(System.Text.Json.Serialization.JsonIgnoreAttribute)) ?? false);
        }

        // ──────────────────────────────────────────────
        //  Build human-readable description
        // ──────────────────────────────────────────────
        private static string BuildDescription(string entityName, string action, EntityEntry entry)
        {
            var sb = new StringBuilder(256);
            var id = GetEntityId(entry);

            sb.Append(entityName);
            sb.Append(id != null ? $" (Id: {id})" : "");
            sb.Append(action switch
            {
                "Added"   => " eklendi",
                "Modified" => " güncellendi",
                "Deleted"  => " silindi",
                _ => $" {action}"
            });

            // For Modified entities, append changed field names
            if (action == "Modified")
            {
                var changedFields = entry.Properties
                    .Where(p => p.IsModified && !IsNavigationOrCollection(p.Metadata))
                    .Select(p => p.Metadata.Name)
                    .ToList();

                if (changedFields.Count > 0)
                {
                    sb.Append(" [Değişen alanlar: ");
                    sb.Append(string.Join(", ", changedFields));
                    sb.Append(']');
                }
            }

            return sb.ToString();
        }

     
    }
}
