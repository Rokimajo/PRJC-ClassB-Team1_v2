using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaveroApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
         public DbSet<User> Users { get; set; }
         public DbSet<Event> Events { get; set; }
         public DbSet<Info> News { get; set; }
         public DbSet<Attendance> Attendances { get; set; }
         public DbSet<EventAttendance> EventAttendances { get; set; }
         public DbSet<Review> Reviews { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
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
}