using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace CaveroApp.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Model
{
    public class MyContext : DbContext { 
        
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<EventAttendance> EventAttendances { get; set; }
        public DbSet<Review> Reviews { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("User ID = postgres; Password = root; Host = 145.24.222.213; port = 8101; Database = Cavero; Pooling = true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // modelBuilder.HasDefaultSchema("cd");
            
        }
    }
}

public class News
{
    public int ID { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public DateTime date { get; set; }
}

public class User
{
    [Key]
    public int ID { get; set; }
    [Required, Column(TypeName = "varchar(30)")]
    public string first_name { get; set; }
    [Required, Column(TypeName = "varchar(30)")]
    public string last_name { get; set; }
    
    public string job_title { get; set; }
    [Required]
    public string email { get; set; }
    //needs to be hashed
    [Required]
    public string password { get; set; }
    public bool is_admin { get; set; }
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
    
    //relationship attribute, navigation property
    [ForeignKey ("user_id")]
    public User? User { get; set; }
}

public class EventAttendance
{
    [Key]
    public int ID { get; set; }
    public int event_id { get; set; }
    public int user_id { get; set; }
    
    [ForeignKey ("event_id")]
    public Event? Event { get; set; }
    [ForeignKey ("user_id")]
    public User? User { get; set; }
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
    [ForeignKey ("user_id")]
    public User? User { get; set; }
}

