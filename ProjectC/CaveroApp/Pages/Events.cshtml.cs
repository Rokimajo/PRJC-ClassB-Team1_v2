using System.Security.Claims;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class Events : PageModel
{
    public CaveroAppContext Context { get; }
    // Monday - Friday
    // First item is monday, last item is friday in dates.
    public (DateTime, DateTime) Week { get; set; }
    
    public static DateTime ChosenDay { get; set; }

    public int saved = 0;
    public Events(CaveroAppContext context)
    {
        Context = context;
    }

    public void GetCurrentWeek()
    {
        DateTime toUse = DateTime.SpecifyKind(ChosenDay, DateTimeKind.Utc);
        var week = new ValueTuple<DateTime, DateTime>();
        var success = DaysTillMonday.TryParse<DaysTillMonday>(toUse.DayOfWeek.ToString(), out var day);
        week.Item1 = toUse.Date.AddDays(-(int)day).Date;
        week.Item2 = week.Item1.AddDays(4).Date;
        Week = week;
    }
    
    public List<WeekInfo> GetWeekEvents()
    {
        var week = new List<WeekInfo>();
        var StartDay = Week.Item1;
        int count = 0;
        while (StartDay <= Week.Item2)
        {
            var events = new WeekInfo
                {
                    Date = StartDay,
                    allEvents = (from x in Context.Events where
                                x.date.Date.Equals(StartDay.Date) && x.admin_approval == true
                                select x).ToList()
                };
            week.Add(events);
            count++;
            StartDay = StartDay.AddDays(1);
        }
        return week;
    }

    public int GetEventParticipants(CaveroAppContext.Event ev)
    {
        return (from e in Context.Events
            join ea in Context.EventAttendances on e.ID equals ea.event_id
            where e.Equals(ev)
            select ea).Count();
    }
    
    
    public void OnGet()
    {
        
        bool actionAlreadyPerformed = bool.TryParse(HttpContext.Session.GetString("EventsInitialSet"), out var result);

        if (!actionAlreadyPerformed)
        {
            ChosenDay = DateTime.UtcNow;
            // Set the session variable to indicate that the action has been performed
            HttpContext.Session.SetString("EventsInitialSet", true.ToString());
        }
        GetCurrentWeek();
    }

    public IActionResult OnPostWeekForward()
    {
        ChosenDay = ChosenDay.AddDays(7);
        return RedirectToPage();
    }

    public IActionResult OnPostWeekBackward()
    {
        ChosenDay = ChosenDay.AddDays(-7);
        return RedirectToPage();
    }

    public int GetRandGradient()
    {
        var rand = new Random();
        return rand.Next(-45, 160);
    }

    public bool JoinedEvent(int eventID)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Context.EventAttendances.Any(x => x.user_id == userId && x.event_id == eventID);
    }

    public IActionResult OnPostEventSignUp(int eventID)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var evAtt = new CaveroAppContext.EventAttendance() { event_id = eventID, user_id = userId };
        Context.Add(evAtt);
        Context.SaveChanges();
        return RedirectToPage();
    }
    
    public IActionResult OnPostEventSignOut(int eventID)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var evAtt = (from ea in Context.EventAttendances
            where ea.event_id.Equals(eventID) && ea.user_id.Equals(userId)
                select ea).First();
        Context.Remove(evAtt);
        Context.SaveChanges();
        return RedirectToPage();
    }
}

/// <summary>
///     This enum is used to get the day of the week till monday as an integer.
///     So if its monday, it returns 0, tuesday returns 1, etc.
///     This is used to get the current week's dates.
/// </summary>
public enum DaysTillMonday
{
    Monday = 0,
    Tuesday = 1,
    Wednesday = 2,
    Thursday = 3,
    Friday = 4,
    Saturday = 5,
    Sunday = 6
}

public class WeekInfo
{
    public DateTime Date { get; set; }
    public List<CaveroAppContext.Event> allEvents { get; set; }
}