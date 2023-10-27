using System.Drawing;
using System.Security.Claims;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class Aanwezig_button : PageModel
{
    private CaveroAppContext Context { get; }
    public string ButtonText { get; set; }


    public void OnGet()
    {
        if (AvailableToday())
        {
            ButtonText = "Zet jezelf afwezig";
        }
        else
        {
            ButtonText = "Zet jezelf aanwezig";
        }
    }
    public Aanwezig_button(CaveroAppContext context)
    {
        Context = context;
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
                where a.user_id == userId select a).First();
            Context.Attendances.Remove(currentUser);
            Context.SaveChanges();
            ButtonText = "Zet jezelf aanwezig";
        }
        else
        {
            if (userId != null)
            {
                CaveroAppContext.Attendance newAttendance = new CaveroAppContext.Attendance(){user_id=userId, date=DateTime.UtcNow.Date};
                Context.Attendances.Add(newAttendance);
                Context.SaveChanges();
                ButtonText = "Zet jezelf afwezig";
            }
        }
        return "Attendance Changed";
    }
}