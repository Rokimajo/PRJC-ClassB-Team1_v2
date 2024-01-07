using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaveroApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaveroApp.Data;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class CaveroAppContext : IdentityDbContext<CaveroAppUser>
{
    /// <summary>
    /// Gets or sets the DbSet of Event entities.
    /// </summary>
    public DbSet<Event> Events { get; set; }

    /// <summary>
    /// Gets or sets the DbSet of Info entities.
    /// </summary>
    public DbSet<Info> News { get; set; }

    /// <summary>
    /// Gets or sets the DbSet of Attendance entities.
    /// </summary>
    public DbSet<Attendance> Attendances { get; set; }

    /// <summary>
    /// Gets or sets the DbSet of EventAttendance entities.
    /// </summary>
    public DbSet<EventAttendance> EventAttendances { get; set; }

    /// <summary>
    /// Gets or sets the DbSet of Review entities.
    /// </summary>
    public DbSet<Review> Reviews { get; set; }

    /// <summary>
    /// Initializes a new instance of the CaveroAppContext class.
    /// </summary>
    /// <param name="options">The options to be used by a DbContext.</param>
    public CaveroAppContext(DbContextOptions<CaveroAppContext> options)
        : base(options)
    {

    }

    /// <summary>
    /// Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="builder">Provides a simple API surface for configuring a Microsoft.EntityFrameworkCore.Metadata.IMutableModel that defines the shape of your entities, the relationships between them, and how they map to the database.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<CaveroAppUser>().ToTable("Users");
        builder.Entity<CaveroAppUser>().Ignore(x => x.PhoneNumber)
            .Ignore(x => x.PhoneNumberConfirmed).Ignore(x => x.LockoutEnd)
            .Ignore(x => x.LockoutEnabled).Ignore(x => x.AccessFailedCount);
    }
    
        
    // news is a tricky name because it can't be singular. That's why the news table is called Info,
    // so the actual postgres table can be called news.
    public class Info
    {
        [Key]
        // Make the ID autoincrement instead of doing it manually.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime date { get; set; }
    }
    
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        
        public TimeSpan start_time { get; set; }
        
        public TimeSpan end_time { get; set; }
        public string location { get; set; }
        public bool admin_approval { get; set; }
    }
    
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey ("ID")]
        public string user_id { get; set; }
        public DateTime date { get; set; }
        
        [ForeignKey ("user_id")]
        public CaveroAppUser? User { get; set; }
    }
    
    public class EventAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int event_id { get; set; }
        public string user_id { get; set; }
        
        [ForeignKey ("event_id")]
        public Event? Event { get; set; }
        [ForeignKey ("user_id")]
        public CaveroAppUser? User { get; set; }
    }
    
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string user_id { get; set; }
        public int event_id { get; set; }
        public int rating { get; set; }
        public string feedback { get; set; }
        
        [ForeignKey ("event_id")]
        public Event? Event { get; set; }
        [ForeignKey ("user_id")]
        public CaveroAppUser? User { get; set; }
    }
}
