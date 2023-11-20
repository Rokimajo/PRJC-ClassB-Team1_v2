using Microsoft.AspNetCore.Mvc.RazorPages;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CaveroApp.Pages;

public class testpage : PageModel
{
    public CaveroAppContext Context { get; }
    public void OnGet()
    {
        
    }
    public testpage(CaveroAppContext context)
    {
        Context = context;
    }

    public List<CaveroAppUser> Users()
    {
        return Context.Users.ToList();
    }
    public IActionResult OnPostUpdateUser(CaveroAppUser updatedUser)
    {
        var userToChange = Context.Users.First(x => x.Id == updatedUser.Id);
        userToChange.FirstName = updatedUser.FirstName;
        userToChange.LastName = updatedUser.LastName;
        userToChange.Email = updatedUser.Email;
        Context.SaveChanges();
        return RedirectToPage();
    }

    public IActionResult OnPostDeleteUser(CaveroAppUser userToDelete)
    {
        var user = Context.Users.First(x => x.Id == userToDelete.Id);
        Context.Remove(user);
        Context.SaveChanges();
        return RedirectToPage();
    }
}