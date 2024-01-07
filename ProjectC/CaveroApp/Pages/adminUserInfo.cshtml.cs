using Microsoft.AspNetCore.Mvc.RazorPages;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaveroApp.Pages;
[Authorize(Roles = "Admin")]

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

    
    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>
    /// A list of CaveroAppUser objects, each representing a user in the database.
    /// </returns>
    public List<CaveroAppUser> Users()
    {
        return Context.Users.ToList();
    }
    
    /// <summary>
    /// Updates a user's information in the database.
    /// </summary>
    /// <param name="updatedUser">The user object containing the updated information.</param>
    /// <returns>
    /// A RedirectToPageResult that redirects the user to the current page.
    /// </returns>
    /// <remarks>
    /// This method is called when the admin submits the form to update a user's information.
    /// It retrieves the user from the database using the ID from the updatedUser parameter,
    /// updates the user's properties with the new values from the updatedUser object,
    /// and then saves the changes to the database.
    /// The user's email is also used to update the UserName, NormalizedEmail, and NormalizedUserName properties.
    /// </remarks>
    public IActionResult OnPostUpdateUser(CaveroAppUser updatedUser)
    {
        var userToChange = Context.Users.First(x => x.Id == updatedUser.Id);
        userToChange.FirstName = updatedUser.FirstName;
        userToChange.LastName = updatedUser.LastName;
        userToChange.Email = updatedUser.Email;
        userToChange.UserName = updatedUser.Email;
        userToChange.NormalizedEmail = updatedUser.Email.ToUpper();
        userToChange.NormalizedUserName = updatedUser.Email.ToUpper();
        Context.SaveChanges();
        return RedirectToPage();
    }
    
    /// <summary>
    /// Deletes a user from the database.
    /// </summary>
    /// <param name="userToDelete">The user to be deleted.</param>
    /// <returns>
    /// A RedirectToPageResult that redirects the user to the current page.
    /// </returns>
    /// <remarks>
    /// This method is called when the admin submits the form to delete a user.
    /// It retrieves the user from the database using the ID from the userToDelete parameter,
    /// removes the user from the database, and then saves the changes to the database.
    /// </remarks>
    public IActionResult OnPostDeleteUser(CaveroAppUser userToDelete)
    {
        
        var user = Context.Users.First(x => x.Id == userToDelete.Id);
        Context.Remove(user);
        Context.SaveChanges();
        return RedirectToPage();
    }

}