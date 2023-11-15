using Microsoft.AspNetCore.Mvc.RazorPages;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;

namespace CaveroApp.Pages;

public class adminUserInfo : PageModel
{
    public CaveroAppContext Context { get; }
    public void OnGet()
    {
        
    }
    public adminUserInfo(CaveroAppContext context)
    {
        Context = context;
    }

    public List<CaveroAppUser> Users()
    {
        return Context.Users.ToList();
    }
}