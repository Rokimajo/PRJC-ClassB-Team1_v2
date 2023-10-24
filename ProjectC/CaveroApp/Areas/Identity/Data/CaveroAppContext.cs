using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaveroApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaveroApp.Data;

public class CaveroAppContext : IdentityDbContext<CaveroAppUser>
{
    public CaveroAppContext(DbContextOptions<CaveroAppContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<CaveroAppUser>().ToTable("Users");
        builder.Entity<CaveroAppUser>().Ignore(x => x.PhoneNumber);
        builder.Entity<CaveroAppUser>().Ignore(x => x.PhoneNumberConfirmed);
        builder.Entity<CaveroAppUser>().Ignore(x => x.LockoutEnd);
        builder.Entity<CaveroAppUser>().Ignore(x => x.LockoutEnabled);
        builder.Entity<CaveroAppUser>().Ignore(x => x.AccessFailedCount);
    }
    
        
    // news is a tricky name because it can't be singular. That's why the news table is called Info,
    // so the actual postgres table can be called news.
    public class Info
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime date { get; set; }
    }
    
    public class Event
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        // may be removed
        public string alt_info { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }
        public string location { get; set; }
        public bool admin_approval { get; set; }
    }
    
    public class Attendance
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey ("ID")]
        public int user_id { get; set; }
        public DateTime date { get; set; }
    }
    
    public class EventAttendance
    {
        [Key]
        public int ID { get; set; }
        public int event_id { get; set; }
        public int user_id { get; set; }
        
        [ForeignKey ("event_id")]
        public Event? Event { get; set; }
    }
    
    public class Review
    {
        [Key]
        public int ID { get; set; }
        public int user_id { get; set; }
        public int event_id { get; set; }
        public int rating { get; set; }
        public string feedback { get; set; }
        
        [ForeignKey ("event_id")]
        public Event? Event { get; set; }
    }
}
