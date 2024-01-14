using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;

namespace CaveroApp.Services;

public class EventServices
{
    /// <summary>
    /// Retrieves a list of reviews for a given event.
    /// </summary>
    /// <param name="ev">The event for which to retrieve the reviews.</param>
    /// <param name="Context">The database context that the function will use.</param>
    /// <returns>A list of UserReviews objects, each containing a user and their corresponding review for the event.</returns>
    public List<CustomClasses.UserReviews> GetEventReviews(CaveroAppContext Context, CaveroAppContext.Event ev)
    {
        return (from r in Context.Reviews 
            join u in Context.Users on r.user_id equals u.Id
            where r.event_id == ev.ID 
            select new CustomClasses.UserReviews
            {
                User = u,
                Review = r
            }).ToList();
    }
    

    /// <summary>
    /// Retrieves a list of users who are participants of a given event.
    /// </summary>
    /// <param name="ev">The event for which to retrieve the participants.</param>
    /// <param name="Context">The database context that the function will use.</param>
    /// <returns>A list of CaveroAppUser objects representing the participants of the event.</returns>
    public List<CaveroAppUser> GetEventParticipants(CaveroAppContext Context, CaveroAppContext.Event ev)
    {
        return (from ea in Context.EventAttendances
            join u in Context.Users on ea.user_id equals u.Id
            where ea.event_id.Equals(ev.ID)
            select u).ToList();
    }

}