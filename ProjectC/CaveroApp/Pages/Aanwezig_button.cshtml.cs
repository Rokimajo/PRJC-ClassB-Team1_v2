using System.Drawing;
using System.Security.Claims;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace CaveroApp.Pages;

public class Aanwezig_button : PageModel
{
    private CaveroAppContext Context { get; }
    public Aanwezig_button(CaveroAppContext context)
    {
        Context = context;
    }

    public bool AvailableToday()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var AreAtWork = (from a in Context.Attendances 
            join u in Context.Users on a.user_id equals u.Id
            where DateTime.UtcNow.Date.Equals(a.date.Date)  select u.Id).ToList();
        return AreAtWork.Contains(userId);
    }
    public IActionResult OnPostSetUnavailable()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null) 
        {
            List<CaveroAppContext.Attendance> currentUser = (from a in Context.Attendances 
                where a.user_id == userId && a.date == DateTime.UtcNow.Date select a).ToList();
            foreach (CaveroAppContext.Attendance attendance in currentUser)
            {
                Context.Attendances.Remove(attendance);
            }
            Context.SaveChanges();
            Console.WriteLine("afwezig gezet");
        }

        return RedirectToPage();
    }

    public IActionResult OnPostSetAvailable()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null) 
        {   
            CaveroAppContext.Attendance newAttendance = new CaveroAppContext.Attendance()
                { user_id = userId, date = DateTime.UtcNow.Date };
            Context.Attendances.Add(newAttendance);
            Context.SaveChanges();
            Console.WriteLine("aanwezig gezet");
        }

        return RedirectToPage();
    }

    public void OnGet()
    {
        
    }
}