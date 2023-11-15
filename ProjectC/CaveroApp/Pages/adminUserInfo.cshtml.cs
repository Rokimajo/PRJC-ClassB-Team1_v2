using Microsoft.AspNetCore.Mvc.RazorPages;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CaveroApp.Pages;

public class adminUserInfo : PageModel
{
    public CaveroAppContext Context { get; }
    public CaveroAppUser UserToEdit { get; set; }
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

    public CaveroAppUser GetChosenUser(string id)
    {
        
    }
    public IActionResult OnPostUpdateUser(CaveroAppUser updatedUser)
    {
        Context.Users.Update(updatedUser);
        Context.SaveChanges();
        return RedirectToPage();
    }
}