using System.Data.Entity;
using System.Globalization;
using System.Security.Claims;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using CaveroApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

[Authorize]
public class Dashboard : PageModel
{
    private CaveroAppContext Context { get; }
    
    // Monday - Friday
    // First item is monday, last item is friday in dates.
    public (DateTime, DateTime) Week { get; set; }
    public int EmployeeCount { get; set; }
    public int EventCount { get; set; }
    
    [BindProperty]
    public CheckInModel CheckModel { get; set; }

    // use dependency injection to get the database context
    public Dashboard(CaveroAppContext context)
    {
        Context = context;
    }

    public class CheckInModel
    {
        public string Date { get; set; }
    }
    //<summary>
    //    This function grabs stats from the database, namely how many users are present today,
    //    and how many events are taking place this week.
    //    It populates the fields EmployeeCount and EventCount with the results.
    //</summary>
    public void GetTodayStats()
    {
        var emp_count = (from a in Context.Attendances
            join u in Context.Users on a.user_id equals u.Id
            where DateTime.UtcNow.Date.Equals(a.date.Date) select u).ToList().Distinct().Count();

        Week = DateServices.GetCurrentWeek(DateTime.UtcNow);
        var event_count = (from u in Context.Events
            where u.date.Date >= Week.Item1.Date && u.date.Date <= Week.Item2.Date && u.admin_approval != false
            select u).Count();
        
        EmployeeCount = emp_count;
        EventCount = event_count;
    }
    
    /// <summary>
    /// Retrieves all events that are scheduled for the current week.
    /// </summary>
    /// <returns>
    /// A list of Event objects, each representing an event that is scheduled for the current week.
    /// </returns>
    /// <remarks>
    /// This method retrieves all events from the database where the event's date is within the current week.
    /// The current week is defined as the period from the date stored in the Week.Item1 property to seven days after that date.
    /// The method returns a list of distinct events, meaning that if an event occurs more than once in the same week, it is only included once in the list.
    /// </remarks>
    public List<CaveroAppContext.Event> GetAllEvents()
    {
        // Context.Events.Select(x => x);
        var Info = (from events in Context.Events
            where events.date.Date >= Week.Item1 && events.date.Date <= Week.Item1.AddDays(7)
            select events).Distinct().ToList();

        return Info;
    }
    
    /// <summary>
    ///    This function gets the number of participants for a given event.
    ///     It returns the count of how many participants that event has.
    /// </summary>
    /// <param name="ev">
    ///     The event to get the participants for.
    /// </param>
    public int GetEventParticipantsCount(CaveroAppContext.Event ev)
    {
        return (from e in Context.Events
            join ea in Context.EventAttendances on e.ID equals ea.event_id
            where e.Equals(ev)
            select ea).Count();
    }
    
    public void OnGet()
    {
        // Remove eventsinitialset if it was created in events, so the information is not stored between pages.
        // This is used so that the event week view starts on the date of today when you navigate back to it.
        HttpContext.Session.Remove("EventsInitialSet");
        HttpContext.Session.Remove("AttendanceInitialSet");
        // Call this function to populate the fields with how many employees are present today,
        // and how many events are happening  this week.
        GetTodayStats();
    }
    
    /// <summary>
    ///     This function checks if the user is already checked in on the given date in string format.
    /// </summary>
    /// <param name="date">
    ///    The date to check if the user is already checked in on. This date is given as string, in the format of dd-mm-yyyy.
    /// </param>
    /// <returns>
    ///     Returns a JsonResult, which is used by AJAX to check if the user is already checked in on the given date.
    /// </returns>
    public JsonResult OnGetCheckDate(string date)
    {
        var dateSplit = date.Split("-").Select(x => Convert.ToInt32(x)).ToArray();
        var parsedDate = DateTime.SpecifyKind(new DateTime(dateSplit[2], dateSplit[1], dateSplit[0]), DateTimeKind.Utc);
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var exists = Context.Attendances.Any(x => x.date.Date == parsedDate.Date && x.user_id == userID);
        // Invert the result, if exists is true, it means the user is already checked in on that date.
        // If we do not invert, returning true would mean the json is valid, and this would make the check in button submittable.
        return new JsonResult(!exists);
    }
    
    /// <summary>
    /// Checks in a user for a specific date.
    /// </summary>
    /// <remarks>
    /// This method is called when the user submits the form to check in for a specific date.
    /// It retrieves the user's ID from the current user's claims, and then tries to parse the date from the form.
    /// If the date is successfully parsed, it creates a new Attendance object with the user's ID and the parsed date,
    /// adds it to the database, and then saves the changes to the database.
    /// After the check-in process is completed, it redirects the user to the Dashboard page.
    /// </remarks>
    /// <returns>
    /// A RedirectToPageResult that redirects the user to the Dashboard page.
    /// </returns>
    public IActionResult OnPostDateCheckInUser()
    {
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(DateTime.TryParseExact(CheckModel.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            var att = new CaveroAppContext.Attendance()
            {
                user_id = userID,
                date = parsedDate,
            };
            Context.Add(att);
            Context.SaveChanges();
        }
        return RedirectToPage("/Dashboard");
    }


}