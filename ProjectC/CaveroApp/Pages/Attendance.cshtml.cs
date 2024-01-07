using System.Security.Claims;
using CaveroApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static CaveroApp.Services.CustomClasses;

namespace CaveroApp.Pages;

[Authorize]
public class Attendance : PageModel
{
    // Database context
    public CaveroAppContext Context { get; }
    
    // Monday - Friday
    // First item is monday, last item is friday in dates.
    public (DateTime, DateTime) Week { get; set; }
    
    // The day that the user has chosen to view events for
    public static DateTime ChosenDay { get; set; }
    
    // constructor
    public Attendance(CaveroAppContext context)
    {
        Context = context;
    }
    
    /// <summary>
    /// This function gets all the attendances for the current week, based on the dates listed in the Week Tuple.
    /// It returns a list of WeekInfo objects, which contain the date and all the attendances for that date.
    /// </summary>
    public List<WeekAttendances> GetWeekAttendances()
    {
        var week = new List<WeekAttendances>();
        var StartDay = Week.Item1;
        int count = 0;
        while (StartDay <= Week.Item2)
        {
            var events = new WeekAttendances
            {
                Date = StartDay,
                allUsers = (from x in Context.Attendances 
                    join u in Context.Users on x.user_id equals u.Id
                    where x.date.Date.Equals(StartDay.Date.Date) select u).ToList()
            };
            week.Add(events);
            count++;
            StartDay = StartDay.AddDays(1);
        }
        return week;
    }

    public bool IsAttending(string userID, DateTime date)
    {
        return Context.Attendances.Any(x => x.user_id == userID && x.date.Date == date.Date);
    }
    
    public void OnGet()
    {
        // This variable is used to set the event date to the current date, so the user starts at the current week instead of a random date.
        // This is set to true to signify that it shouldn't happen again unless the page is completely refreshed,
        // so the user doesn't keep going back to the initial start date when trying to switch the week view.
        bool actionAlreadyPerformed = bool.TryParse(HttpContext.Session.GetString("AttendanceInitialSet"), out var result);

        if (!actionAlreadyPerformed)
        {
            ChosenDay = DateTime.UtcNow;
            // Set the session variable to indicate that the action has been performed
            HttpContext.Session.SetString("AttendanceInitialSet", true.ToString());
        }
        Week = Services.DateServices.GetCurrentWeek(ChosenDay);
    }
    
    /// <summary>
    ///     This function is called when the user clicks the "Next Week" button.
    ///     It adds 7 Days to the ChosenDay variable, which means the next Get() call will get the events for that week.
    /// </summary>
    /// <returns>
    ///     A RedirectToPageResult, which redirects to the same page,
    ///     without keeping a post form blocking the user from refreshing the page.
    /// </returns>
    public IActionResult OnPostWeekForward()
    {
        ChosenDay = ChosenDay.AddDays(7);
        return RedirectToPage();
    }

    /// <summary>
    ///     This function is called when the user clicks the "Last Week" button.
    ///     It adds -7 Days to the ChosenDay variable, which means the next Get() call will get the events for that week.
    /// </summary>
    /// <returns>
    ///     A RedirectToPageResult, which redirects to the same page,
    ///     without keeping a post form blocking the user from refreshing the page.
    /// </returns>
    public IActionResult OnPostWeekBackward()
    {
        ChosenDay = ChosenDay.AddDays(-7);
        return RedirectToPage();
    }

    public IActionResult OnPostUserJoin(string userID, string date)
    {
        var splitDate = date.Split("-").Select(x => Convert.ToInt32(x)).ToArray();
        var newDate = DateTime.SpecifyKind(new DateTime(splitDate[2], splitDate[1], splitDate[0]), DateTimeKind.Utc);
        if (newDate.Date >= DateTime.UtcNow.Date)
        {
                        Context.Attendances.Add(new CaveroAppContext.Attendance()
                        {
                            user_id = userID,
                            date = newDate,
                        });
                        Context.SaveChanges();
        }
        return RedirectToPage();
    }
    
    public IActionResult OnPostUserLeave(string userID, string date)
    {
        var splitDate = date.Split("-").Select(x => Convert.ToInt32(x)).ToArray();
        var newDate = DateTime.SpecifyKind(new DateTime(splitDate[2], splitDate[1], splitDate[0]), DateTimeKind.Utc);
        if (newDate.Date >= DateTime.UtcNow.Date)
        {
             var Att = Context.Attendances.First(x => x.user_id == userID && x.date.Date == newDate.Date);
             Context.Remove(Att);
             Context.SaveChanges();
        }
 
        return RedirectToPage();
    }
    
    
}