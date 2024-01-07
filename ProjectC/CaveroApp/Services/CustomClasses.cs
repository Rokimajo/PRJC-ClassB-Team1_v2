using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;

namespace CaveroApp.Services;

public class CustomClasses
{
    /// <summary>
    ///     This class is used to store the date and all the events for that date.
    ///     It is used to display the events for each day in the frontend.
    /// </summary>
    public class WeekEvents
    {
        public DateTime Date { get; set; }
        public List<CaveroAppContext.Event> allEvents { get; set; }
    }
    
    /// <summary>
    ///     This class is used to store the date and all the attendances (users) present on that date.
    ///     It is used to display the attendances for each day in the frontend.
    /// </summary>
    public class WeekAttendances
    {
        public DateTime Date { get; set; }
        public List<CaveroAppUser> allUsers { get; set; }
    }

    /// <summary>
    /// Represents a user's review for an event.
    /// </summary>
    public class UserReviews
    {
        public CaveroAppUser User { get; set; }
        public CaveroAppContext.Review Review { get; set; }
    }
}