using System.Security.Claims;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class admin_user_info : PageModel
{
    private CaveroAppContext Context { get; }
    public admin_user_info(CaveroAppContext context)
    {
        Context = context;
    }
    public void OnGet()
    {
        
    }
    
}