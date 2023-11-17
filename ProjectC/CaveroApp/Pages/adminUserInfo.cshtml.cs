using Microsoft.AspNetCore.Mvc.RazorPages;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CaveroApp.Pages;

public class adminUserInfo : PageModel
{
    public CaveroAppContext Context { get; }
    public UserInfoModel userInfoModel { get; set; }

    public void OnGet()
    {
        
    }
    public class UserInfoModel
    {
        public string userID { get; set; }
        
        public string firstName { get; set; }
        
        public string lastName { get; set; }
        
        public string email { get; set; }
    }
    public adminUserInfo(CaveroAppContext context)
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
}