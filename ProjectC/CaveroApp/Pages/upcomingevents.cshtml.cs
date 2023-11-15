using System.Data.Entity;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Areas.Identity.Pages.Account
{
    public class UpcomingEventsModel : PageModel
    {
        private readonly CaveroAppContext Context;

        public List<CaveroAppContext.Event> EventInfo { get; set; }

        public UpcomingEventsModel(CaveroAppContext context)
        {
            Context = context;
        }

        public void OnGet()
        {
            EventInfo = new List<CaveroAppContext.Event>();
            GetAllEvents();
        }

        public void GetAllEvents()
        {
            // Context.Events.Select(x => x);
            var Info = (from events in Context.Events
                        select events).Distinct().ToList();

            EventInfo = Info;
        }



    }
}