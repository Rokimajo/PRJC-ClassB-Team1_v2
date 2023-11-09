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

    public void SeedEvents()
    {
        var evArr = new CaveroAppContext.Event[]
        {
            new()
            {
                title = "Event1",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow,
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event2",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(1),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event3",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(3),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event4",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(7),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event5",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(9),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event6",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(8),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
new()
            {
                title = "Event7",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(2),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event8",
                description = "ev",
                alt_info = "notnull",
                date = DateTime.UtcNow.AddDays(2),
                start_time = new TimeSpan(0, 16, 0, 0 ),
                location = "somewhere",
                admin_approval = true
            },
            new()
            {
                title = "Event9",
                description = "ev",
                alt_info = "notnull",
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