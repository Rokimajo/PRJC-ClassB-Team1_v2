namespace CaveroApp.Services;

public static class DateServices
{
    /// <summary>
    ///     Gets the current week's dates based on the chosen day parameter.
    ///     It fills the Week tuple with the first item being monday and the last item being friday in dates.
    ///     Function is made to be maintainable and flexible, if you add 6 days to week.Item2 instead of 4,
    ///     you can easily and quickly change the event tab from mon-fri to mon-sun.
    /// </summary>
    public static (DateTime, DateTime) GetCurrentWeek(DateTime ChosenDay)
    {
        DateTime toUse = DateTime.SpecifyKind(ChosenDay, DateTimeKind.Utc);
        var week = new ValueTuple<DateTime, DateTime>();
        var success = DaysTillMonday.TryParse<DaysTillMonday>(toUse.DayOfWeek.ToString(), out var day);
        week.Item1 = toUse.Date.AddDays(-(int)day).Date;
        week.Item2 = week.Item1.AddDays(4).Date;
        return week;
    }
}

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