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
    }
}