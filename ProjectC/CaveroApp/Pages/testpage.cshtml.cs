using System.Reflection;
using System.Security.Claims;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class testpage : PageModel
{
    [BindProperty]
    public RecurringDays recurringDays { get; set; }
    public CaveroAppContext Context { get; }
    public class RecurringDays
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
    }
    
    public testpage(CaveroAppContext context)
    {
        Context = context;
    }
    public void OnGet()
    {
       GetUserDays();
    }

    public void GetUserDays()
    {
        var newRecurringDays = new RecurringDays();
        string userID = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = Context.Users.Find(userID);
        PropertyInfo[] properties = typeof(RecurringDays).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            string name = property.Name;
            if (user!.RecurringDays!.Contains(name.ToLower()))
            {
                property.SetValue(newRecurringDays, true);
            }
        }

        recurringDays = newRecurringDays;
    }
    public IActionResult OnPostSetRecurringDays()
    {
        string userID = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = Context.Users.Find(userID);
        PropertyInfo[] properties = typeof(RecurringDays).GetProperties();
        List<string> newRecurringDays = new List<string>();
        foreach (PropertyInfo property in properties)
        {
            string name = property.Name;
            bool value = (bool)property.GetValue(recurringDays)!;
            if (value)
            {
                newRecurringDays.Add(name.ToLower());
            }
        }

        user!.RecurringDays = newRecurringDays;
        Context.SaveChanges();
        return RedirectToPage();
    }
}