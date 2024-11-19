using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.DataAccess.Configurations;
using NLayerArchTemplate.Entities;
using NLayerArchTemplate.Entities.Attributes;
using System.Reflection;
using System.Security.Claims;

namespace NLayerArchTemplate.DataAccess;

/// <summary>
/// Nuget Pages are;
/// Microsoft.EntityFrameworkCore
/// Pomelo.EntityFrameworkCore.MySql
/// Microsoft.EntityFrameworkCore.Tools
/// Migration Commands
/// Add-Migration Db1
/// Update-Database Db1
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly IServiceProvider _serviceprovider;
    private IHttpContextAccessor _httpContextAccessor => _serviceprovider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

    //Migration için bu constructor kullanılıyor
    public ApplicationDbContext() : base()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                IServiceProvider serviceProvider) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = false;
        _serviceprovider = serviceProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configurasyon class'larını aşağıda ki gibi tek tek de ekleyebiliriz.
        //modelBuilder.ApplyConfiguration(new UserConfiguration());
        //Configurasyon class'larını aşağıda ki gibi tek seferde ekleyebiliriz.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = GetConnectonString();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
    private string GetConnectonString()
    {
        var dir = Directory.GetCurrentDirectory();
        var ui = Directory.GetParent(dir).GetDirectories().First(f => f.FullName.Contains("UI")).FullName;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(ui)
            .AddJsonFile($"appsettings.Development.json")
            .Build();
        return configuration.GetConnectionString("DefaultConnection");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var username = _httpContextAccessor.HttpContext.User.Claims.First(f => f.Type == ClaimTypes.NameIdentifier)?.Value;
        var deletedEntries = ChangeTracker.Entries().Where(w => w.State is EntityState.Deleted);
        var modifiedEntries = ChangeTracker.Entries().Where(w => w.State is EntityState.Modified);
        var addedEntries = ChangeTracker.Entries().Where(w => w.State is EntityState.Added);
        IList<TblAudit> audits = new List<TblAudit>();
        LogDeletedData(deletedEntries, audits, username);
        LogUpdatedData(modifiedEntries, audits, username);
        LogAddedData(addedEntries, audits, username);
        if (audits.Any())
            await AddRangeAsync(audits, cancellationToken);
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    private void LogAddedData(IEnumerable<EntityEntry> entries, IList<TblAudit> audits, string username)
    {
        foreach (EntityEntry entry in entries)
        {
            var entryType = entry.Entity.GetType();
            var audit = new TblAudit
            {
                TableName = entryType.Name,
                Data = entry.Entity.ToJSON(),
                Type = entry.State.ToString(),
                CreatedDate = DateTime.Now,
                CreatedBy = username
            };
            audits.Add(audit);
        }
    }
    private void LogUpdatedData(IEnumerable<EntityEntry> entries, IList<TblAudit> audits, string username)
    {
        foreach (EntityEntry entry in entries)
        {
            var entryType = entry.Entity.GetType();
            var id = Convert.ToInt64(entry.Property("Id").CurrentValue);
            var audit = new TblAudit
            {
                TableName = entryType.Name,
                Type = entry.State.ToString(),
                CreatedDate = DateTime.Now,
                CreatedBy = username
            };
            var dataArr = new Dictionary<string, object>
                    {
                        { "Id", id }
                    };

            foreach (PropertyEntry property in entry.Properties)
            {
                if (property.IsModified)
                {
                    dataArr.Add(property.Metadata.Name, property.CurrentValue);
                }
            }
            audit.Data = dataArr.ToJSON();
            audits.Add(audit);
        }
    }
    private void LogDeletedData(IEnumerable<EntityEntry> entries, IList<TblAudit> audits, string username)
    {
        foreach (EntityEntry entry in entries)
        {
            var entryType = entry.Entity.GetType();
            var deleteAttribute = entryType.GetCustomAttribute<DeleteAuditAttribute>();
            var audit = new TblAudit
            {
                TableName = entryType.Name,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            };
            if (deleteAttribute is not null && deleteAttribute.IsSoftDelete)
            {
                entry.State = EntityState.Modified;
                entryType.GetProperty("IsDeleted").SetValue(entry.Entity, true);
                var id = Convert.ToInt64(entry.Property("Id").CurrentValue);
                audit.Type = "SoftDelete";
                audit.Data = "{\"Id\":" + id + ",\"IsDeleted\":true}";
            }
            else
            {
                audit.Type = entry.State.ToString();
                audit.Data = entry.Entity.ToJSON();
            }
            audits.Add(audit);
        }
    }
    public virtual DbSet<TblUser> Users { get; set; }
    public virtual DbSet<TblAudit> Audits { get; set; }
}
