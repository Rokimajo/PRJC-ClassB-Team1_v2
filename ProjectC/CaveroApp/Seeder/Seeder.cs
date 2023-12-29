using CaveroApp.Data;

namespace CaveroApp.Seeder;

public class Seeder
{
    public CaveroAppContext db { get; }
    public Seeder(IServiceProvider Provider)
    {
        db = Provider.GetRequiredService<CaveroAppContext>(); 
    }
    
    public void Seed()
    {
        SeedEvents();
    }

    /// <summary>
    /// Seeds the database with a set of predefined events.
    /// </summary>
    /// <remarks>
    /// This method creates an array of Event objects, each representing an event with a predefined title, description, date, start time, location, and admin approval status.
    /// The events are scheduled for various dates, with the date of each event being calculated relative to the current date and time (DateTime.UtcNow).
    /// The start time for all events is set to 16:00 (4 PM).
    /// The location for all events is set to "somewhere".
    /// The admin approval status for all events is set to true, meaning that the events are approved by an admin and can be displayed to users.
    /// After creating the array of events, the method adds the events to the database and saves the changes.
    /// </remarks>
    public void SeedEvents()
    {
        var evArr = new CaveroAppContext.Event[]
        {
            new()
            {
                title = "Event1",
                description = "ev",
                date = DateTime.UtcNow,
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event2",
                description = "ev",
                date = DateTime.UtcNow.AddDays(1),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event3",
                description = "ev",
                date = DateTime.UtcNow.AddDays(3),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event4",
                description = "ev",
                date = DateTime.UtcNow.AddDays(7),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event5",
                description = "ev",
                date = DateTime.UtcNow.AddDays(9),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event6",
                description = "ev",
                date = DateTime.UtcNow.AddDays(8),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event7",
                description = "ev",
                date = DateTime.UtcNow.AddDays(2),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event8",
                description = "ev",
                date = DateTime.UtcNow.AddDays(2),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event9",
                description = "ev",
                date = DateTime.UtcNow.AddDays(2),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
        };
        db.AddRange(evArr);
        db.SaveChanges();
    }
}