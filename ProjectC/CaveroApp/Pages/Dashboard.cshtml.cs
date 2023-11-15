using System.Data.Entity;
using System.Security.Claims;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

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

        GetCurrentWeek();
        var event_count = (from u in Context.Events
            where u.date.Date >= Week.Item1.Date && u.date.Date <= Week.Item2.Date && u.admin_approval != false
            select u).Count();
        
        EmployeeCount = emp_count;
        EventCount = event_count;
    }
    
    /// <summary>
    ///     Gets the current week's dates based on the chosen day parameter.
    ///     It fills the Week tuple with the first item being monday and the last item being friday in dates.
    /// </summary>
    public void GetCurrentWeek()
    {
        var week = new ValueTuple<DateTime, DateTime>();
        var success = DaysTillMonday.TryParse<DaysTillMonday>(DateTime.UtcNow.DayOfWeek.ToString(), out var day);
        week.Item1 = DateTime.UtcNow.Date.AddDays(-(int)day).Date;
        week.Item2 = week.Item1.AddDays(4).Date;
        Week = week;
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

    // get method api endpoint for javascript validator
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
    public IActionResult OnPostDateCheckInUser()
    {
        var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var dateSplit = CheckModel.Date.Split("-").Select(x => Convert.ToInt32(x)).ToArray();
        var parsedDate = DateTime.SpecifyKind(new DateTime(dateSplit[2], dateSplit[1], dateSplit[0]), DateTimeKind.Utc);
        var att = new CaveroAppContext.Attendance()
        {
            user_id = userID,
            date = parsedDate,
        };
        Context.Add(att);
        Context.SaveChanges();
        return RedirectToPage("/Dashboard");
    }


}