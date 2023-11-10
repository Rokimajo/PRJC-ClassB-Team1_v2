﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CaveroApp.Pages;

public class Events : PageModel
{
    // Database context
    public CaveroAppContext Context { get; }
    
    [BindProperty]
    public CreateEventModel EventModel { get; set; }
    
    // Monday - Friday
    // First item is monday, last item is friday in dates.
    public (DateTime, DateTime) Week { get; set; }
    
    // The day that the user has chosen to view events for
    public static DateTime ChosenDay { get; set; }
    
    public Events(CaveroAppContext context)
    {
        Context = context;
    }

    public class CreateEventModel
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Location { get; set; }
        
        public string Date { get; set; }
        
        public string StartTime { get; set; }
        
        public string EndTime { get; set; }
    }

    /// <summary>
    ///     Gets the current week's dates based on the chosen day parameter.
    ///     It fills the Week tuple with the first item being monday and the last item being friday in dates.
    /// </summary>
    public void GetCurrentWeek()
    {
        DateTime toUse = DateTime.SpecifyKind(ChosenDay, DateTimeKind.Utc);
        var week = new ValueTuple<DateTime, DateTime>();
        var success = DaysTillMonday.TryParse<DaysTillMonday>(toUse.DayOfWeek.ToString(), out var day);
        week.Item1 = toUse.Date.AddDays(-(int)day).Date;
        week.Item2 = week.Item1.AddDays(4).Date;
        Week = week;
    }
    
    /// <summary>
    /// This function gets all the events for the current week, based on the dates listed in the Week Tuple.
    /// It returns a list of WeekInfo objects, which contain the date and all the events for that date.
    /// </summary>
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

public List<CaveroAppUser> GetEventParticipants(CaveroAppContext.Event ev)
{
    return (from ea in Context.EventAttendances
        join u in Context.Users on ea.user_id equals u.Id
        where ea.event_id.Equals(ev.ID)
        select u).ToList();
}
    
/// <summary>
///     The Get() function has a bool that checks if the action has already been performed.
///     This is used so that we only set ChosenDay once, and not every time the page is refreshed.
///     This makes switching weeks possible.
/// </summary>
    public void OnGet()
    {
        // This variable is used to set the event date to the current date, so the user starts at the current week instead of a random date.
        // This is set to true to signify that it shouldn't happen again unless the page is completely refreshed,
        // so the user doesn't keep going back to the initial start date when trying to switch the week view.
        bool actionAlreadyPerformed = bool.TryParse(HttpContext.Session.GetString("EventsInitialSet"), out var result);

        if (!actionAlreadyPerformed)
        {
            ChosenDay = DateTime.UtcNow;
            // Set the session variable to indicate that the action has been performed
            HttpContext.Session.SetString("EventsInitialSet", true.ToString());
        }
        GetCurrentWeek();
    }

public IActionResult OnPostCreateEvent()
{
    // if this method goes trough, the javascript validation checker found no issues.
    // so there's no need for backend validation checking here.
    var splitDate = EventModel.Date.Split("-").Select(x => Convert.ToInt32(x)).ToArray();
    var startTime = EventModel.StartTime.Split(":").Select(x => Convert.ToInt32(x)).ToArray();
    var endTime = EventModel.EndTime.Split(":").Select(x => Convert.ToInt32(x)).ToArray();
    var newEvent = new CaveroAppContext.Event()
    {
        title = EventModel.Title,
        description = EventModel.Description,
        location = EventModel.Location,
        date = DateTime.SpecifyKind(new DateTime(splitDate[2], splitDate[1], splitDate[0]), DateTimeKind.Utc),
        start_time = new TimeSpan(startTime[0], startTime[1], 0),
        end_time = new TimeSpan(endTime[0], endTime[1], 0),
        // change this to false once admin works
        admin_approval = true
    };
    Context.Add(newEvent);
    Context.SaveChanges();
    return RedirectToPage("/Events");
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

    /// <summary>
    ///     This function creates a random integer so that the gradients in Events.cshtml are different for each event.
    /// </summary>
    public int GetRandGradient()
    {
        var rand = new Random();
        return rand.Next(-80, 210);
    }

    /// <summary>
    ///     This function checks if the user has already joined a event.
    ///     Returns a boolean, true if the user has already joined an event, false if they haven't.
    /// </summary>
    /// <param name="eventID">
    ///     The ID of the event passed along from the frontend to check if the currently logged in user has joined.
    /// </param>
    public bool JoinedEvent(int eventID)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Context.EventAttendances.Any(x => x.user_id == userId && x.event_id == eventID);
    }

    /// <summary>
    /// This function is called when the currently logged in user clicks on the 'Join' button for an event.
    /// It adds an EventAttendance object to the database, with the passed along eventID and the currently logged in user's ID.
    /// </summary>
    /// <param name="eventID">
    ///     The ID of the event passed along from the frontend to make an EventAttendance object with.
    /// </param>
    /// <returns>
    ///     A RedirectToPageResult, which redirects to the same page,
    ///     without keeping a post form blocking the user from refreshing the page.
    /// </returns>
    public IActionResult OnPostEventSignUp(int eventID)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var evAtt = new CaveroAppContext.EventAttendance() { event_id = eventID, user_id = userId };
        Context.Add(evAtt);
        Context.SaveChanges();
        return RedirectToPage();
    }
    
    /// <summary>
    /// This function is called when the currently logged in user clicks on the 'Leave' button for an event.
    /// It removes an EventAttendance object to the database, with the passed along eventID and the currently logged in user's ID.
    /// </summary>
    /// <param name="eventID">
    ///     The ID of the event passed along from the frontend to search the EventAttendance to remove from the database.
    /// </param>
    /// <returns>
    ///     A RedirectToPageResult, which redirects to the same page,
    ///     without keeping a post form blocking the user from refreshing the page.
    /// </returns>
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

/// <summary>
///     This class is used to store the date and all the events for that date.
///     It is used to display the events for each day in the frontend.
/// </summary>
public class WeekInfo
{
    public DateTime Date { get; set; }
    public List<CaveroAppContext.Event> allEvents { get; set; }
}