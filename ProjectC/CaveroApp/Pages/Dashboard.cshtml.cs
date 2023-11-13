using System.Security.Claims;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class Dashboard : PageModel
{
    private CaveroAppContext Context { get; }
    
    public int EmployeeCount { get; set; }
    public int EventCount { get; set; }
    public string ButtonText { get; set; }

    // use dependency injection to get the database context
    public Dashboard(CaveroAppContext context)
    {
        Context = context;
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

        var event_count = (from u in Context.Events
            let weekFromNow = DateTime.UtcNow.AddDays(7).Date
            where u.date.Date >= DateTime.UtcNow.Date && u.date.Date <= weekFromNow && u.admin_approval != false
            select u).ToList().Distinct().Count();
        
        EmployeeCount = emp_count;
        EventCount = event_count;
    }
    
    public void OnGet()
    {
        // Call this function to populate the fields with how many employees are present today,
        // and how many events are happening this week.
        GetTodayStats();
        if (AvailableToday())
        {
            ButtonText = "Zet jezelf afwezig";
        }
        else
        {
            ButtonText = "Zet jezelf aanwezig";
        }
    }
    public bool AvailableToday()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AreAtWork = (from a in Context.Attendances 
            join u in Context.Users on a.user_id equals u.Id
            where DateTime.UtcNow.Date.Equals(a.date.Date) select u.Id).ToList();
        return AreAtWork.Contains(userId);
    }

    public string ChangeStatus()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (AvailableToday())
        {
            CaveroAppContext.Attendance currentUser = (from a in Context.Attendances 
                where a.user_id == userId && a.date == DateTime.UtcNow.Date select a).First();
            Context.Attendances.Remove(currentUser);
            Context.SaveChanges();
            ButtonText = "Zet jezelf aanwezig";
        }
        else
        {
            if (userId != null)
            {
                CaveroAppContext.Attendance newAttendance = new CaveroAppContext.Attendance()
                    { user_id = userId, date = DateTime.UtcNow.Date };
                Context.Attendances.Add(newAttendance);
                Context.SaveChanges();
                ButtonText = "Zet jezelf afwezig";
            }
        }
        return "Attendance Changed";
    }
}