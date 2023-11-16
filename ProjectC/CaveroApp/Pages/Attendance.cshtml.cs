using System.Security.Claims;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static CaveroApp.Services.CustomClasses;

namespace CaveroApp.Pages;

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
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!System.IO.File.Exists(@$"wwwroot/img/profilepictures/avatar{userID}.png"))
        {
            var user = Context.Users.First(x => x.Id == userID);
            ProfilePictureGenerator.MakePF(user.FirstName, user.LastName, userID!);
        }
    }
    
    
}