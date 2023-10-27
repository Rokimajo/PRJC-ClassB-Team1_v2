using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class Events : PageModel
{
    public CaveroAppContext Context { get; }
    // Monday - Friday
    // First item is monday, last item is friday in dates.
    public (DateTime, DateTime) Week { get; set; }

    public Events(CaveroAppContext context)
    {
        Context = context;
    }

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
        GetCurrentWeek();
    }
}

/// <summary>
///     This enum is used to get the day of the week till monday as an integer.
///     So if its, monday, it returns 0, tuesday returns 1, etc.
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